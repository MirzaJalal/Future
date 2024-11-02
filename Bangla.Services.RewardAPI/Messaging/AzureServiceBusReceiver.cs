using Azure.Messaging.ServiceBus;
using Bangla.Services.RewardAPI.Services;
using Newtonsoft.Json;
using System.Text;
using Bangla.Services.RewardAPI.Message;

namespace Bangla.Services.RewardAPI.Messaging
{
    public class AzureServiceBusReceiver : IAzureServiceBusReceiver
    {
        private readonly string _connectionString;
        private readonly string _orderCreatedTopicName;
        private readonly string _orderCreatedRewardsSubscriptionTopic;
        private readonly ILogger<AzureServiceBusReceiver> _logger;
        private readonly RewardsService _rewardsService;

        private readonly ServiceBusProcessor _rewardsServiceBusProcessor;

        // Constructor where the ServiceBusProcessor is injected and initialized
        public AzureServiceBusReceiver(IConfiguration configuration,
                                       ILogger<AzureServiceBusReceiver> logger,
                                       RewardsService rewardsService)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"];
            _orderCreatedTopicName = configuration["AzureServiceBus:Ordercreated_TopicName"];
            _orderCreatedRewardsSubscriptionTopic = configuration["AzureServiceBus:OrderCreated_RewardsSubscription"];

            var client = new ServiceBusClient(_connectionString);
            _rewardsServiceBusProcessor = client.CreateProcessor(_orderCreatedTopicName, _orderCreatedRewardsSubscriptionTopic);

            _rewardsService = rewardsService;
            _logger = logger;
        }

        // Method to start processing messages
        public async Task StartReceiveMessagesAsync()
        {
            // Subscribe to message and error handlers here
            _rewardsServiceBusProcessor.ProcessMessageAsync += MessageHandler;
            _rewardsServiceBusProcessor.ProcessErrorAsync += ErrorHandler;
            // No need to create the processor; just start processing messages
            await _rewardsServiceBusProcessor.StartProcessingAsync();

            _logger.LogInformation("Started processing messages...");
        }

        // Method to stop processing messages
        public async Task StopReceiveMessagesAsync()
        {
            await _rewardsServiceBusProcessor.StopProcessingAsync();
            await _rewardsServiceBusProcessor.DisposeAsync();

            await _rewardsServiceBusProcessor.StopProcessingAsync();
            await _rewardsServiceBusProcessor.DisposeAsync();
        }

        #region Handlers for receving message
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            string body = Encoding.UTF8.GetString(message.Body);

            var rewardMessage = JsonConvert.DeserializeObject<RewardMessage>(body);

            _logger.LogInformation($"Received: {rewardMessage}");

            try
            {
                // logging the email in the db
                await _rewardsService.UpdateRewards(rewardMessage);
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
