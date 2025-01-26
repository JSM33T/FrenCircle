using Microsoft.AspNetCore.SignalR;

namespace FrenCircle.Base.Hubs
{
    public class AudioStreamHub : Hub
    {
        public async Task StartStream(string streamerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, streamerId);
            await Clients.Group(streamerId).SendAsync("StreamStarted", streamerId);
        }

        public async Task StopStream(string streamerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, streamerId);
            await Clients.Group(streamerId).SendAsync("StreamStopped", streamerId);
        }

        public async Task SendAudioChunk(string streamerId, byte[] audioChunk)
        {
            await Clients.Group(streamerId).SendAsync("ReceiveAudioChunk", audioChunk);
        }
    }
}
