
using FrenCircle.Shared.ConfigModels;

namespace FrenCircle.Infra.Telegram
{
    public class TelegramService(FcConfig config, HttpClient httpClient) : ITelegramService
    {
        private readonly string _botToken = config.TeleConfig.BotToken;

        public async Task SendToMultipleAsync(IEnumerable<string> chatIds, string message)
        {
            foreach (var chatId in chatIds)
            {
                await SendMessageAsync(chatId, message);
            }
        }

        public async Task SendToOneAsync(string chatId, string message)
        {
            await SendMessageAsync(chatId, message);
        }

        private async Task SendMessageAsync(string chatId, string message)
        {
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("chat_id", chatId),
            new KeyValuePair<string, string>("text", message)
        });

            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
    }

}
