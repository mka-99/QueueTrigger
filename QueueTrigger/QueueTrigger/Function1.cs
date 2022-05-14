using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Produktverwaltung.Models;

namespace QueueTrigger
{
    public class ExpensiveProduct
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Text { get; set; }
    }

    public class Function1
    {
        [FunctionName("Function1")]
        [return: Table("ExpensiveProducts")]
        public ExpensiveProduct Run([QueueTrigger("productqueue", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            var prod = Newtonsoft.Json.JsonConvert.DeserializeObject<Product>(myQueueItem);

            if (prod.Price > 100)
            {
                return new ExpensiveProduct { PartitionKey = "expProducts", RowKey = Guid.NewGuid().ToString(), Text = prod.Name };
            }
            return null;
        }
    }
}
