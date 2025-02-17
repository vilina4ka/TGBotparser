namespace CsvJsonTgBot6V;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ProcessingLibrary;

class Program
{
    static async Task Main(string[] args)
    {
        var myBot = new BotMain();
        await myBot.Start();
    }

}

