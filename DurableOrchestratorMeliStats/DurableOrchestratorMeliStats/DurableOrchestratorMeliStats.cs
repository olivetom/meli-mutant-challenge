using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DurableOrchestratorMeliStats
{
    public static class DurableOrchestratorMeliStats
    {
        private const string Timeout = "timeout";
        private const string RetryInterval = "retryInterval";

        [FunctionName("DurableOrchestratorMeliStats")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("DurableOrchestratorMeliStats_Hello", "mutant"));
            outputs.Add(await context.CallActivityAsync<string>("DurableOrchestratorMeliStats_Hello", "human"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("DurableOrchestratorMeliStats_Hello")]
        public static string SayHello([ActivityTrigger] string partitionKey, ILogger log,
            [CosmosDB(ConnectionStringSetting =
            "melidbsql_DOCUMENTDB")]DocumentClient client)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("dnaDatabase", "MyDNACollection");

            var query = client.CreateDocumentQuery(collectionUri, new SqlQuerySpec()
            {
                QueryText = "SELECT COUNT (1) as count from c where c.PartitionKey = @pkey",
                Parameters = new SqlParameterCollection()
                    {
                        new SqlParameter("@pkey", partitionKey)
                    }
            });


            /*"mutantCount: {\n  \"count\": 2\n}",
                "humanCount: {\n  \"count\": 1\n}"
            */
            Object cosmosDBCount = null;

            if (query != null)
            {
                foreach (var item in query)
                {
                    cosmosDBCount = JsonConvert.DeserializeObject(item.ToString());
                    break;
                }
            }

            log.LogInformation($"returning {partitionKey}Count: {cosmosDBCount?.ToString()}.");


            return $"{partitionKey}Count: {cosmosDBCount?.ToString()}";
        }


        [FunctionName("DurableOrchestratorMeliStats_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stats")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableOrchestratorMeliStats", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            TimeSpan timeout = GetTimeSpan(req, Timeout) ?? TimeSpan.FromSeconds(10);
            TimeSpan retryInterval = GetTimeSpan(req, RetryInterval) ?? TimeSpan.FromSeconds(2);

            _ = await starter.WaitForCompletionOrCreateCheckStatusResponseAsync(
                req,
                instanceId,
                timeout,
                retryInterval);

            DurableOrchestrationStatus durableOrchestrationStatus = await starter.GetStatusAsync(instanceId);

            //IList<string> mutantCount = durableOrchestrationStatus.Output["count"].Select(t => (string)t).ToList();
            //log.LogInformation($"____________________________ '{mutantCount[0]}'.");


            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(durableOrchestrationStatus.Output))
            };

            return httpResponseMessage;
        }

        private static TimeSpan? GetTimeSpan(HttpRequestMessage request, string queryParameterName)
        {
            string queryParameterStringValue = request.RequestUri.ParseQueryString()[queryParameterName];
            if (string.IsNullOrEmpty(queryParameterStringValue))
            {
                return null;
            }

            return TimeSpan.FromSeconds(double.Parse(queryParameterStringValue));
        }
    }

    /*"mutantCount: {\n  \"count\": 2\n}",
        "humanCount: {\n  \"count\": 1\n}"
        */
    internal sealed class CosmosDBCountResult
    {
        [JsonProperty(PropertyName = "mutantCount")]
        public string mutantCount { get; set; }

        [JsonProperty(PropertyName = "humanCount")]
        public string humanCount { get; set; }

    }
}
 