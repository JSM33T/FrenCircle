using FrenCircle.Entities;
using Microsoft.Extensions.Options;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrenCircle.Infra
{
    public interface ITelegramService
    {
        public Task SendMessageAsync(string message);
    }
    public class TelegramService(HttpClient httpClient, IOptions<FcConfig> config) : ITelegramService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly FcConfig _config = config.Value;

        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrEmpty(_config.telegramSettings.BotToken)
                || string.IsNullOrEmpty(_config.telegramSettings.ChatId)
                || string.IsNullOrEmpty(_config.telegramSettings.ApiUrl))
                return;

            var url = $"{_config.telegramSettings.ApiUrl}/bot{_config.telegramSettings.BotToken}/sendMessage";
            var payload = new { chat_id = _config.telegramSettings.ChatId, text = message };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

        }
    }
}
