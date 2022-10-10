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

namespace MachinePark.Server
{
    public static class MachineParkAPI
    {
        [FunctionName("devices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Location location = new() { Name = "Vega", Country = "Sweden" };
            DeviceType type = new() { Name = "Weather Sensor", Description = "temperature, humidity" };
            IEnumerable<Device> devices = new List<Device>()
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



            return new OkObjectResult(devices);
        }
    }
}
