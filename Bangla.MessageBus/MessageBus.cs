using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace Bangla.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connectionString;

        public MessageBus(IConfiguration configuration)
        {
            _connectionString = "Endpoint=sb://futurebanglashop.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=m5Q6zYyHg5lm3txykA0JS6qEOyP7yJlrN+ASbCyoOiA=";
        }

        // Sending Message to Service Bus
        public async Task SendMessageAsync(object message, string topic_queue_name)
        {
            await using (ServiceBusClient client = new ServiceBusClient(_connectionString))
            {
                ServiceBusSender sender = client.CreateSender(topic_queue_name);

                var jsonMessage = JsonConvert.SerializeObject(message);
                ServiceBusMessage busMessage = new ServiceBusMessage(Encoding
                    .UTF8.GetBytes(jsonMessage))
                { 
                    CorrelationId = Guid.NewGuid().ToString()
                };

                await sender.SendMessageAsync(busMessage);
                await client.DisposeAsync();
            }
        }
    }
}
