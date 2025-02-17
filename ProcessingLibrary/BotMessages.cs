using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProcessingLibrary
{
	public static class BotMessages
	{
        /// <summary>
        /// Вывод инфы о боте.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		public static async Task DefaultInfo(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
            Message message = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Привет! Я бот для обработки csv и json файлов.",
                disableNotification: true,
                replyToMessageId: update.Message.MessageId,
                replyMarkup: new InlineKeyboardMarkup(
                InlineKeyboardButton.WithUrl(
                text: "My honest reaction",
                url: "https://usagif.com/wp-content/uploads/gify/chipi-chipi-chapa-chapa-cat-hd-version-full.gif")),
                cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Посылание сообщений.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task SendMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string text)
        {
            Message message = await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: text,
                disableNotification: true,
                replyToMessageId: update.Message.MessageId,
                cancellationToken: cancellationToken);
        }
        /// <summary>
        /// Постоянное поддержвиание состояния клавиутуры с кнопками.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="replyKeyboardMarkup"></param>
        /// <returns></returns>
        public static async Task ReplyWithMenuButtons(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, ReplyKeyboardMarkup replyKeyboardMarkup)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
               chatId: update.Message.Chat.Id,
               text: "Выберите пункт меню из кнопок",
               disableNotification: true,
               replyMarkup: replyKeyboardMarkup,
               cancellationToken: cancellationToken);
        }

    }
}

