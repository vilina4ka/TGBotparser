using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
namespace ProcessingLibrary
{
	public class BotFunctional
	{
		// Поле, хранящее список объектов.
        private ReplyKeyboardMarkup _replyKeyboard;
		// Значения из файла для совершения сортировки.
		private List<string> _valuesForSample;
		// Список объектов типа MyElectrocarPower.
        private List<MyElectrocarPower> _electrocarPower = new List<MyElectrocarPower>();
		// Выбранное действие для хранения состояния, в котором бот сейчас находится.
        private BotActions _currentAction = BotActions.defaultInfo;
		// Список значений для выборки 3 значений.
		private List<string> _valuesForThreeSample = new List<string>();
		// Показывает успешность чтения.
		private bool _successfulRead = false;
		/// <summary>
		/// Конструктор с параметром.
		/// </summary>
		/// <param name="reply">Ответ </param>
		/// <exception cref="ArgumentNullException"></exception>

		public BotFunctional(ReplyKeyboardMarkup reply)
		{
			if (reply is null)
			{
				throw new ArgumentNullException();
			}
			_replyKeyboard = reply;
		}
		// Конструктор без параметров.

		public BotFunctional() { }
		/// <summary>
		/// Выводит текст об успешности сортировки и клавиатуру.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
        private async Task TextAfterSort(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _currentAction = BotActions.defaultInfo;
            await BotMessages.SendMessage(botClient, update, cancellationToken, "Сортировка проведена успешно");
            await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
        }
		/// <summary>
		/// Выводит текст после успешной выборки.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
        private async Task TextAfterSample(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
			
            _currentAction = BotActions.defaultInfo;
			// Очистка 
			_valuesForSample.Clear();
			_valuesForThreeSample.Clear();
			if (_electrocarPower.Count == 0)
			{
				await BotMessages.SendMessage(botClient, update, cancellationToken, "Выборка проведена успешно, результат пуст");
			}
			else
			{
				await BotMessages.SendMessage(botClient, update, cancellationToken, "Выборка проведена успешно");
			}
            await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
        }
		/// <summary>
		/// Метод для загрузки файла.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="readFile"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
        public async Task DownloadFile(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken,
			Func<Stream, List<MyElectrocarPower>> readFile, string fileName)
		{
            var fileId = update.Message.Document.FileId;
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
			await using (Stream fileStream = System.IO.File.Create(fileName))
			{
				await botClient.DownloadFileAsync(
					filePath: filePath,
					destination: fileStream,
					cancellationToken: cancellationToken);
				try
				{
					_electrocarPower = readFile(fileStream);
					_successfulRead = true;
					
					await BotMessages.SendMessage(botClient, update, cancellationToken, "Файл считан успешно");
				}
				catch(ArgumentException)
				{
					await BotMessages.SendMessage(botClient, update, cancellationToken, "Файл некорректного формата.");
				}
				await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
			}
        }
        /// <summary>
		/// Метод для скачивания файла.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellation"></param>
		/// <param name="writeToFile">Делегат записи в файл.</param>
		/// <param name="fileName"></param>
		/// <returns></returns>		
		public async Task UploadFile(ITelegramBotClient botClient, Update update, CancellationToken cancellation,
			Func<List<MyElectrocarPower>, Stream> writeToFile, string fileName)
		{
			using (Stream stream = writeToFile(_electrocarPower))
			{
				Message message = await botClient.SendDocumentAsync(
				chatId: update.Message.Chat.Id,
				disableNotification: true,
				document: InputFile.FromStream(stream: stream, fileName: fileName),
				caption: "Обработанный файл");
            }
            await BotMessages.ReplyWithMenuButtons(botClient, update, cancellation, _replyKeyboard);
        }
		/// <summary>
		/// Обработка файлов.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task ProcessingFiles(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
            switch (_currentAction)
            {
                case BotActions.downloadCSV:
                    await DownloadFile(botClient, update, cancellationToken, CSVProcessing.Read, "electrocarpower_downloaded.csv");
                    _currentAction = BotActions.defaultInfo;
                    break;
                case BotActions.downloadJSON:
                    await DownloadFile(botClient, update, cancellationToken, JSONProcessing.Read, "electrocarpower_downloaded.json");
                    _currentAction = BotActions.defaultInfo;
                    break;
				// если некорректный - просто дефолт фраза и меню.
                default:
                    await BotMessages.DefaultInfo(botClient, update, cancellationToken);
                    await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
                    break;
            }
        }
		/// <summary>
		/// Проверка на пустоту для кнопрок выгрузки файла.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="writeToFile"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public async Task CheckEmptiness(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken,
            Func<List<MyElectrocarPower>, Stream> writeToFile, string fileName)
		{
            if (_electrocarPower.Count == 0)
            {
                await BotMessages.SendMessage(botClient, update, cancellationToken, "Нет данных для обработки");
            }
			else
			{
				await UploadFile(botClient, update, cancellationToken, writeToFile, fileName);
			}
        }
		/// <summary>
		/// Перегрузка метода для проверки на пустоту для выборок.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="field"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public async Task CheckEmptiness(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string field, int n=1)
		{
			if (_electrocarPower.Count == 0)
			{
				if (n == 1)
					await BotMessages.SendMessage(botClient, update, cancellationToken, "Нет данных для обработки");
            }
			else
			{
				await TextForSample(botClient, update, cancellationToken, field);
			}
		}
		/// <summary>
		/// Перегрузка метода для проверку на пустоту для сортировки
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="reverse"></param>
		/// <returns></returns>
        public async Task CheckEmptiness(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, bool reverse=false)
        {
            if (_electrocarPower.Count == 0)
            {
                await BotMessages.SendMessage(botClient, update, cancellationToken, "" +
					"Нет данных для обработки");
            }
            else
            {
				DataProcessing.Sorting(ref _electrocarPower, reverse);
				await TextAfterSort(botClient, update, cancellationToken);
            }
        }
		/// <summary>
		/// Выводит допустимые для выборки значения по опрделенному полю.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="field"></param>
		/// <returns></returns>
        public async Task TextForSample(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string field)
		{
			_valuesForSample = DataProcessing.SelectValuesForSample(_electrocarPower, field);
			await BotMessages.SendMessage(botClient, update, cancellationToken, ($"Введите значение для поля {field}\n\nДопустимые значения:"
					+ String.Join("\n", _valuesForSample)));
		}
		/// <summary>
		/// Метод для обработки текстовых значений пользователя.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
        public async Task ProcessingText(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
		{
			Message message = update.Message;
			switch(message.Text)
			{
				case "загрузка csv":
					await BotMessages.SendMessage(botClient, update, cancellationToken, "загрузите файл csv для считывания");
					_currentAction = BotActions.downloadCSV;
					break;
				case "загрузка json":
                    await BotMessages.SendMessage(botClient, update, cancellationToken, "загрузите файл json для считывания");
                    _currentAction = BotActions.downloadJSON;
                    break;
                case "выгрузка csv":
					_currentAction = BotActions.uploadCSV;
					await CheckEmptiness(botClient, update, cancellationToken, CSVProcessing.Write, "electrocarpower_result.csv");
                    break;
				case "выгрузка json":
                    _currentAction = BotActions.uploadJSON;
                    await CheckEmptiness(botClient, update, cancellationToken, JSONProcessing.Write, "electrocarpower_result.json");
                    break;
				case "сортировка по AdmArea по алфавиту":
					_currentAction = BotActions.sortAdmAreaAscending;
					await CheckEmptiness(botClient, update, cancellationToken);
					break;
				case "сортировка по AdmArea в обратном порядке":
                    _currentAction = BotActions.sortAdmAreaDescending;
                    await CheckEmptiness(botClient, update, cancellationToken, true);
                    break;

				case "выборка по AdmArea":
					_currentAction = BotActions.sampleAdmArea;
					await CheckEmptiness(botClient, update, cancellationToken, "AdmArea");
					break;
				case "выборка по District":
					_currentAction = BotActions.sampleDistrict;
                    await CheckEmptiness(botClient, update, cancellationToken, "District");
                    break;
				case "выборка по AdmArea, Longitude_WGS84 и Latitude_WGS84":
					_currentAction = BotActions.threeAdmArea;
                    await CheckEmptiness(botClient, update, cancellationToken, "AdmArea");
                    break;
                case "информация обо мне":
					_currentAction = BotActions.defaultInfo;
                    await BotMessages.DefaultInfo(botClient, update, cancellationToken);
                    break;
				case "/start":
					await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
					break;
				default:
					await TextForSampleProcessing(botClient, update, cancellationToken, message);
					break;
            }
		}
		/// <summary>
		/// Метод для обработки выборки.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="update"></param>
		/// <param name="cancellationToken"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task TextForSampleProcessing(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, Message message)
		{
            if (_valuesForSample.Contains(message.Text))
            {
                switch (_currentAction)
                {
                    case BotActions.sampleAdmArea:
                        DataProcessing.SampleAdmArea(ref _electrocarPower, message.Text);
                        await TextAfterSample(botClient, update, cancellationToken);
                        break;
                    case BotActions.sampleDistrict:
                        DataProcessing.SampleDistrict(ref _electrocarPower, message.Text);
                        await TextAfterSample(botClient, update, cancellationToken);
                        break;
					// Когда три значения - последовательно выводится информация о каждом поле и запрашивается ввод, выборка производится после введения всех значений.
                    case BotActions.threeAdmArea:
                        _currentAction = BotActions.threeLongitude;
                        _valuesForThreeSample.Add(message.Text);
                        await CheckEmptiness(botClient, update, cancellationToken, "Longitude_WGS84");
                        break;
                    case BotActions.threeLongitude:
                        _currentAction = BotActions.threeLatitude;
                        _valuesForThreeSample.Add(message.Text);
                        await CheckEmptiness(botClient, update, cancellationToken, "Latitude_WGS84");
                        break;
                    case BotActions.threeLatitude:
                        _currentAction = BotActions.defaultInfo;
                        _valuesForThreeSample.Add(message.Text);
                        DataProcessing.SampleThreeParametres(ref _electrocarPower, _valuesForThreeSample);

                        await TextAfterSample(botClient, update, cancellationToken);

                        break;
                }
            }
            else if (_currentAction == BotActions.sampleAdmArea || _currentAction == BotActions.sampleDistrict ||
                _currentAction == BotActions.sampleThreeParametres || _currentAction == BotActions.threeAdmArea ||
				_currentAction == BotActions.threeLongitude || _currentAction == BotActions.threeLatitude)
            {
                await BotMessages.SendMessage(botClient, update, cancellationToken, "Вы ввели некорректные данные для выборки");

            }
            else
            {
                await BotMessages.DefaultInfo(botClient, update, cancellationToken);
                await BotMessages.ReplyWithMenuButtons(botClient, update, cancellationToken, _replyKeyboard);
            }
        }

	}
}

