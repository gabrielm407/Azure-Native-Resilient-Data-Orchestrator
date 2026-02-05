using System.Diagnostics;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DataOrchestrator
{
    public class ProcessJob
    {
        private readonly ILogger<ProcessJob> _logger;

        public ProcessJob(ILogger<ProcessJob> logger)
        {
            _logger = logger;
        }

        [Function("ProcessJob")]
        public async Task Run(
            [ServiceBusTrigger("job-queue", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            // 1. Start a timer for Telemetry (observability)
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation($"Processing message: {message.MessageId}");

            try
            {
                // Deserialize the job
                var body = message.Body.ToString();
                var jobData = JsonSerializer.Deserialize<JobPayload>(body);

                // 2. Simulate complex data processing
                await Task.Delay(500); // Fake CPU work

                // 3. CHAOS ENGINEERING (The "Senior Engineer" move)
                // Randomly fail 20% of the time to demonstrate Azure's Retry & Dead Letter Queue
                var random = new Random();
                if (random.Next(1, 10) > 8) 
                {
                    throw new InvalidOperationException("Simulated Transient Failure!");
                }

                _logger.LogInformation($"Job {jobData?.JobId} processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing job: {ex.Message}");
                // We re-throw the exception so Azure Service Bus knows to retry later!
                throw; 
            }
            finally
            {
                stopwatch.Stop();
                
                // 4. Custom Metric: "ProcessingLatency"
                // This will show up in your Azure Dashboard charts
                _logger.LogMetric("JobProcessingLatencyMs", stopwatch.ElapsedMilliseconds);
            }
        }
    }
}