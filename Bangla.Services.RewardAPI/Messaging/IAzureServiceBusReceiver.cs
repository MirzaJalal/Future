namespace Bangla.Services.RewardAPI.Messaging
{
    public interface IAzureServiceBusReceiver
    {
        Task StartReceiveMessagesAsync();
        Task StopReceiveMessagesAsync();
    }
}
