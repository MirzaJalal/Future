namespace Bangla.Services.EmailAPI.Messaging
{
    public interface IAzureServiceBusReceiver
    {
        Task StartReceiveMessagesAsync();
        Task StopReceiveMessagesAsync();
    }
}
