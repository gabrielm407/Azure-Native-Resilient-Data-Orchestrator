using System.Net;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DataOrchestrator
{
    public class IngestJob
    {
        private readonly ILogger _logger;
        private readonly ServiceBusClient _serviceBusClient;

        // Constructor Injection (Best Practice for Testability)
        public IngestJob(ILoggerFactory loggerFactory, ServiceBusClient serviceBusClient)
        {
            _logger = loggerFactory.CreateLogger<IngestJob>();
            _serviceBusClient = serviceBusClient;
        }

        [Function("IngestJob")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Received new data job request.");

            // 1. Parse Request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var jobData = JsonSerializer.Deserialize<JobPayload>(requestBody);

            if (jobData == null || string.IsNullOrEmpty(jobData.JobId))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid Job Data");
                return badResponse;
            }

            // 2. Telemetry: Log the specific operation metric
            // This directly addresses "maintains telemetry pipelines" in the JD
            _logger.LogMetric("JobIngestionStart", 1); 

            // 3. Send to Service Bus (Distributed System Pattern)
            var sender = _serviceBusClient.CreateSender("job-queue");
            var message = new ServiceBusMessage(requestBody)
            {
                // Set a Message ID for deduplication (Resilience)
                MessageId = jobData.JobId,
                Subject = "DataProcessingJob"
            };

            try 
            {
                await sender.SendMessageAsync(message);
                _logger.LogInformation($"Job {jobData.JobId} queued successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to queue job: {ex.Message}");
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            var response = req.CreateResponse(HttpStatusCode.Accepted); // 202 Accepted is correct for async processing
            await response.WriteStringAsync($"Job {jobData.JobId} accepted for processing.");
            return response;
        }
    }

    // Simple Data Contract
    public class JobPayload
    {
        public string JobId { get; set; }
        public string OperationType { get; set; }
        public int DataSizeKB { get; set; }
    }
}