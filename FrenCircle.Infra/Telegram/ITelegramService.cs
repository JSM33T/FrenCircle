namespace FrenCircle.Infra.Telegram
{
    public interface ITelegramService
    {
        Task SendToMultipleAsync(IEnumerable<string> chatIds, string message);
        Task SendToOneAsync(string chatId, string message);
    }

}
