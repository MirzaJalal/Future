using Azure.Messaging.ServiceBus;
using Bangla.Services.EmailAPI.Models.Dto;
using Bangla.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Bangla.Services.EmailAPI.Messaging
{
    public class AzureServiceBusReceiver : IAzureServiceBusReceiver
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly ILogger<AzureServiceBusReceiver> _logger;
        private readonly ServiceBusProcessor _emailBusProcessor;
        private readonly EmailService _emailService;

        // Constructor where the ServiceBusProcessor is injected and initialized
        public AzureServiceBusReceiver(IConfiguration configuration,
                                       ILogger<AzureServiceBusReceiver> logger,
                                       EmailService emailService)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"];
            _queueName = configuration["AzureServiceBus:EmailShoppingCart_QueueName"];

            var client = new ServiceBusClient(_connectionString);
            _emailBusProcessor = client.CreateProcessor(_queueName);

            _emailService = emailService;
            _logger = logger;
        }

        // Method to start processing messages
        public async Task StartReceiveMessagesAsync()
        {
            // Subscribe to message and error handlers here
            _emailBusProcessor.ProcessMessageAsync += MessageHandler;
            _emailBusProcessor.ProcessErrorAsync += ErrorHandler;

            // No need to create the processor again; just start processing messages
            await _emailBusProcessor.StartProcessingAsync();
            _logger.LogInformation("Started processing messages...");
        }

        // Method to stop processing messages
        public async Task StopReceiveMessagesAsync()
        {
            await _emailBusProcessor.StopProcessingAsync();
            await _emailBusProcessor.DisposeAsync();
        }

        #region Handlers for receving message
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            string body = Encoding.UTF8.GetString(message.Body);

            var shoppingCartDto = JsonConvert.DeserializeObject<ShoppingCartDto>(body);

            _logger.LogInformation($"Received: {shoppingCartDto}");

            try
            {
                // logging the email in the db
                await _emailService.EmailCartAndLog(shoppingCartDto);
                // Complete the message, removing it from the queue
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            // Complete the message after processing it
            await args.CompleteMessageAsync(args.Message);
        }

        // Handler for processing errors
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError($"Message handler encountered an error: {args.Exception}");
            return Task.CompletedTask;
        }
        #endregion

    }

}
