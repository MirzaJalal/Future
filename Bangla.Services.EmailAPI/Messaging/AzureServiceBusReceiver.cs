using Azure.Messaging.ServiceBus;
using Bangla.Services.EmailAPI.Message;
using Bangla.Services.EmailAPI.Models.Dto;
using Bangla.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Bangla.Services.EmailAPI.Messaging
{
    public class AzureServiceBusReceiver : IAzureServiceBusReceiver
    {
        private readonly string _connectionString;

        // email topic and subscription
        private readonly string _queueName;
        private readonly string _registrationUserQueue;

        // order topic and subscription
        private readonly string _ordercreatedTopicName;
        private readonly string _orderCreatedEmailSubscription;

        private readonly ILogger<AzureServiceBusReceiver> _logger;
        private readonly EmailService _emailService;

        private readonly ServiceBusProcessor _emailOrderPlacedProcessor;
        private readonly ServiceBusProcessor _emailBusProcessor;
        private readonly ServiceBusProcessor _registrationUserBusProcessor;

        // Constructor where the ServiceBusProcessor is injected and initialized
        public AzureServiceBusReceiver(IConfiguration configuration,
                                       ILogger<AzureServiceBusReceiver> logger,
                                       EmailService emailService)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"];
            _queueName = configuration["AzureServiceBus:EmailShoppingCart_QueueName"];
            _registrationUserQueue = configuration["AzureServiceBus:RegistrationUserQueueName"];
            _orderCreatedEmailSubscription = configuration["AzureServiceBus:OrderCreated_EmailSubscription"];
            _ordercreatedTopicName = configuration["AzureServiceBus:Ordercreated_TopicName"];

            var client = new ServiceBusClient(_connectionString);
            _emailBusProcessor = client.CreateProcessor(_queueName);
            _registrationUserBusProcessor = client.CreateProcessor(_registrationUserQueue);

            _emailOrderPlacedProcessor = client.CreateProcessor(_ordercreatedTopicName, _orderCreatedEmailSubscription);


            _emailService = emailService;
            _logger = logger;
        }

        // Method to start processing messages
        public async Task StartReceiveMessagesAsync()
        {
            // Subscribe to message and error handlers here --> (QUEUE)
            _emailBusProcessor.ProcessMessageAsync += MessageHandler;
            _emailBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailBusProcessor.StartProcessingAsync();


            // Subscribe to Registration user bus message and error handlers --> (QUEUE)
            _registrationUserBusProcessor.ProcessMessageAsync += OnUserRegistrationMessageHandler;
            _registrationUserBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _registrationUserBusProcessor.StartProcessingAsync();

            // Subscribe to order places bus message and error handlers --> (TOPIC)
            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedMessageHandler;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();

            _logger.LogInformation("Started processing messages...");
        }

        // Method to stop processing messages
        public async Task StopReceiveMessagesAsync()
        {
            await _emailBusProcessor.StopProcessingAsync();
            await _emailBusProcessor.DisposeAsync();

            await _registrationUserBusProcessor.StopProcessingAsync();
            await _registrationUserBusProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        #region Handlers for receving message from Cart
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


        #endregion 

        #region OnUserRegistration MessageHandler
        private async Task OnUserRegistrationMessageHandler(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            string body = Encoding.UTF8.GetString(message.Body);

            string registrationEmail = JsonConvert.DeserializeObject<string>(body);

            _logger.LogInformation($"Received: {registrationEmail}");

            try
            {
                // logging the email in the db
                await _emailService.RegistrationUserEmailAndLog(registrationEmail);
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
        #endregion

        #region Handlers for receving message from Order Created
        private async Task OnOrderPlacedMessageHandler(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            string body = Encoding.UTF8.GetString(message.Body);

            var rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);

            _logger.LogInformation($"Received: {rewardMessage}");

            try
            {
                // logging the email in the db
                await _emailService.OrderPlacedLog(rewardMessage);
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
        #endregion

        // Handler for processing errors
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError($"Message handler encountered an error: {args.Exception}");
            return Task.CompletedTask;
        }

    }

}
