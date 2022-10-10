using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using MachinePark.Shared;
using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations.Schema;
using TableAttribute = Microsoft.Azure.WebJobs.TableAttribute;

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Queryable;
using CloudTable = Microsoft.Azure.Cosmos.Table.CloudTable;
using MachinePark.Server.Entities;
using MachinePark.Server.Extensions;
using System.Linq;
using TableOperation = Microsoft.Azure.Cosmos.Table.TableOperation;

namespace MachinePark.Server
{
    public static class MachineParkAPI
    {
        private static Location location = new() { Name = "Vega", Country = "Sweden" };

        private static DeviceType type = new() { Name = "Weather Sensor", Description = "temperature, humidity" };

        //private static List<Device> devices = new List<Device>()
        //{
            //new Device()
            //    {
            //        Name = "Maskin A",
            //        Location = location,
            //        LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
            //        Type = type,
            //        IsOnline = Random.Shared.Next(2) == 0 ? true : false
            //    },
            //    new Device()
            //    {
            //        Name = "Maskin B",
            //        Location = location,
            //        LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
            //        Type = type,
            //        IsOnline = Random.Shared.Next(2) == 0 ? true : false
            //    },
            //    new Device()
            //    {
            //        Name = "Maskin C",
            //        Location = location,
            //        LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
            //        Type = type,
            //        IsOnline = Random.Shared.Next(2) == 0 ? true : false
            //    },
            //    new Device()
            //    {
            //        Name = "Maskin D",
            //        Location = location,
            //        LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
            //        Type = type,
            //        IsOnline = Random.Shared.Next(2) == 0 ? true : false
            //    }
        //};


        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("devices", Connection = "AzureWebJobsStorage")] CloudTable deviceTable,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var query = new Microsoft.Azure.Cosmos.Table.TableQuery<DeviceTableEntity>();
            var result = await deviceTable.ExecuteQuerySegmentedAsync(query, null);

            var response = result.Select(Mapper.ToItem).ToList();

            return new OkObjectResult(response);

            //return new OkObjectResult(devices);
        }


        [FunctionName("CreateDevice")]
        public static async Task<IActionResult> Create(
            //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Table("devices", Connection = "AzureWebJobsStorage")] CloudTable deviceTable, // IAsyncCollector<ItemTableEntity> itemTable,
            ILogger log)
        {
            log.LogInformation("Create item");


            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //var createDevice = JsonConvert.DeserializeObject<CreateDevice>(requestBody);

            //if (createDevice == null || string.IsNullOrWhiteSpace(createDevice.Name)) return new BadRequestResult();

            //var device = new Device
            //{
            //    Name = createDevice.Name,
            //    Location = location,
            //    Type = type,
            //    LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
            //    IsOnline = false
            //};

            //return new OkObjectResult(null);



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var createDevice = JsonConvert.DeserializeObject<CreateDevice>(requestBody);

            if (createDevice == null || string.IsNullOrWhiteSpace(createDevice.Name)) return new BadRequestResult();

            var device = new Device
            {
                Name = createDevice.Name,
                Location = location,
                Type = type,
                LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
                IsOnline = false
            };

            // await itemTable.AddAsync(item.ToTableEntity());

            var operation = TableOperation.Insert(device.ToTableEntity());
            var res = await deviceTable.ExecuteAsync(operation);

            return new OkObjectResult(device);
        }




        //[FunctionName("DeleteDevice")]
        //public static async Task<IActionResult> Delete(
        //    //[HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req,
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteDevice/{id}")] HttpRequest req,
        //    //[Table("items", "Todo", "{id}", Connection = "AzureWebJobsStorage")] ItemTableEntity itemTableToDelete,
        //    //[Table("items", Connection = "AzureWebJobsStorage")] CloudTable itemTable,
        //    ILogger log, string id)
        //{
        //    log.LogInformation("Delete item");


        //    //if (itemTableToDelete == null || string.IsNullOrWhiteSpace(itemTableToDelete.Text)) return new BadRequestResult();

        //    //var operation = TableOperation.Delete(itemTableToDelete);
        //    //await itemTable.ExecuteAsync(operation);
        //    return new NoContentResult();
        //}


    }
}
