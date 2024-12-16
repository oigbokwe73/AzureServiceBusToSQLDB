using System;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Extensions.Logging;
using Xenhey.BPM.Core.Net8;
using Xenhey.BPM.Core.Net8.Implementation;
using Microsoft.Azure.Functions.Worker;

namespace AzureServiceBusToSQL
{
    public class fileprocessor
    {
        private readonly ILogger _logger;

        public fileprocessor(ILogger<fileprocessor> logger)
        {
            _logger = logger;
        }

        [Function("fileprocessor")]
        public void Run([BlobTrigger("processed/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name)
        {
            string ApiKeyName = "x-api-key";
            _logger.LogInformation("C# blob trigger function processed a request.");
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add(ApiKeyName, "43EFE991E8614CFB9EDECF1B0FDED37E");
            nvc.Add("ContainerName", name);
            IOrchestrationService orchrestatorService = new ManagedOrchestratorService(nvc);
            var processFiles = orchrestatorService.Run(myBlob);
            _logger.LogInformation(processFiles);
        }
    }
}
