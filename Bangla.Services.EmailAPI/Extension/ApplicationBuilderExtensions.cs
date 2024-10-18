using Bangla.Services.EmailAPI.Messaging;

namespace Bangla.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtensions
    {
        // This property will hold the instance of the receiver
        private static IAzureServiceBusReceiver AzureServiceBusReceiver { get; set; }

        // Extension method to use the AzureServiceBusReceiver in the application pipeline
        public static IApplicationBuilder UseAzureServiceBusReceiver(this IApplicationBuilder app)
        {
            // Resolve the AzureServiceBusReceiver from the app's service provider
            AzureServiceBusReceiver = app.ApplicationServices.GetService<IAzureServiceBusReceiver>();

            // Start the message receiving process
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            // When the application starts, start receiving messages from Service Bus
            lifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () => await AzureServiceBusReceiver.StartReceiveMessagesAsync());
            });

            // Optionally, stop processing when the application is shutting down
            lifetime.ApplicationStopping.Register(() =>
            {
                // Here you can add logic to gracefully stop the message processor
                Console.WriteLine("Application stopping, stopping AzureServiceBusReceiver...");
            });

            return app;
        }
    }

}
