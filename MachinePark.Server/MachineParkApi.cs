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

namespace MachinePark.Server
{
    public static class MachineParkAPI
    {
        private static Location location = new() { Name = "Vega", Country = "Sweden" };

        private static DeviceType type = new() { Name = "Weather Sensor", Description = "temperature, humidity" };

        private static List<Device> devices = new List<Device>()
        {
            new Device()
                {
                    Name = "Maskin A",
                    Location = location,
                    LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
                    Type = type,
                    IsOnline = Random.Shared.Next(2) == 0 ? true : false
                },
                new Device()
                {
                    Name = "Maskin B",
                    Location = location,
                    LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
                    Type = type,
                    IsOnline = Random.Shared.Next(2) == 0 ? true : false
                },
                new Device()
                {
                    Name = "Maskin C",
                    Location = location,
                    LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
                    Type = type,
                    IsOnline = Random.Shared.Next(2) == 0 ? true : false
                },
                new Device()
                {
                    Name = "Maskin D",
                    Location = location,
                    LastUpdated = DateTime.Now.AddDays(Random.Shared.Next(20)).Date,
                    Type = type,
                    IsOnline = Random.Shared.Next(2) == 0 ? true : false
                }
        };


        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
 
            return new OkObjectResult(devices);
        }


        [FunctionName("CreateDevice")]
        public static async Task<IActionResult> Create(
            //[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            //[Table("items", Connection = "AzureWebJobsStorage")] CloudTable itemTable, // IAsyncCollector<ItemTableEntity> itemTable,
            ILogger log)
        {
            log.LogInformation("Create item");


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


            devices.Add(device);

            return new OkObjectResult(device);

            //var operation = TableOperation.Insert(device.ToTableEntity());
            //var res = await itemTable.ExecuteAsync(operation);

            //return new OkObjectResult(item);
        }


    }
}
