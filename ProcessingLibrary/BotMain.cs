using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace ProcessingLibrary
{
    public class BotMain
    {
        // Список для вывода клавиатуры с кнопками.
        private static ReplyKeyboardMarkup _replyKeyboardMarkup = new(new[] {
            new KeyboardButton[] { "загрузка csv", "загрузка json" },
            new KeyboardButton[] { "выгрузка csv", "выгрузка json" },
            new KeyboardButton[] { "сортировка по AdmArea по алфавиту" },
            new KeyboardButton[] { "сортировка по AdmArea в обратном порядке" },
            new KeyboardButton[] { "выборка по AdmArea", "выборка по District" },
            new KeyboardButton[] { "выборка по AdmArea, Longitude_WGS84 и Latitude_WGS84" },
            new KeyboardButton[] { "информация обо мне" }
        })
        { ResizeKeyboard = true, IsPersistent = true };
        // Словарь для обработки апдейтов разных пользователей.
        private Dictionary<long, BotFunctional> _myBotForChats = new Dictionary<long, BotFunctional>();
        // Конструктор без параметров.
        public BotMain() { }
        /// <summary>
        /// Запуск бота.
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            var botClient = new TelegramBotClient("6631730580:AAFyhvK9SkOXtHN8IFWDan0LDGpRbbw0K8k");
            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                // Receive all update types except ChatMember related updates.
                AllowedUpdates = Array.Empty<UpdateType>() 
            };
            botClient.StartReceiving(
                    updateHandler: HandleUpdateAsync,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            // Send cancellation request to stop bot.
            cts.Cancel();
        }
        /// <summary>
        /// Обработка апдейтов.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var message = update.Message;
                long chatId = message.Chat.Id;
                if (!_myBotForChats.ContainsKey(chatId))
                {
                    _myBotForChats.Add(chatId, new BotFunctional(_replyKeyboardMarkup));
                }
                if (update.Type == UpdateType.Message)
                {
                    switch (message.Type)
                    {
                        case MessageType.Text:
                            await _myBotForChats[chatId].ProcessingText(botClient, update, cancellationToken);
                            break;
                        case MessageType.Document:
                            await _myBotForChats[chatId].ProcessingFiles(botClient, update, cancellationToken);
                            break;
                    }
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Null-значение");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Ошибка, связанная с данными...");
            }
            catch (IOException)
            {
                Console.WriteLine("Возникла ошибка, связанная с чтением или записью файла");
            }
            catch (Exception)
            {
                Console.WriteLine("Возникла непредвиденная ошибка...");
            }
        }
        /// <summary>
        /// Обработка ошибок.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                  => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}

