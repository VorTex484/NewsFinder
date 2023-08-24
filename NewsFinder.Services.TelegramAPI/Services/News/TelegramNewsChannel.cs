using Microsoft.Extensions.Options;
using NewsFinder.Services.ChatGPT.Models.News;
using NewsFinder.Services.TelegramAPI.Services.News.Templates;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NewsFinder.Services.TelegramAPI.Services.News;

public class TelegramNewsChannel : ITelegramNewsChannel
{
    private readonly ITelegramBotClient _botClient;
    private readonly string _channelName;

    public TelegramNewsChannel(ITelegramBotClient botClient, IOptions<TelegramSendingOptions> options)
    {
        _botClient = botClient;
        _channelName = options.Value.NewsChannel;
    }

    public async Task<int> SendMessageAsync(OpenAiResponse receivedNews)
    {
        var message = MessageTemplate.FormatNewsMessage(
            receivedNews.Impact,
            receivedNews.Coin,
            receivedNews.Summary,
            receivedNews.Importance
        );
        var messageToBeSent = await _botClient.SendTextMessageAsync(_channelName, message, ParseMode.Markdown);
        return messageToBeSent.MessageId;
    }
}