using Hubee.MessageBroker.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System.Collections.Generic;
using System.IO;

namespace Hubee.MessageBroker.Sdk.Tests.Core
{
    public class BaseTest
    {
        private static IConfiguration GetConfiguration(string nameSetting, string path = "")
        {
            return new ConfigurationBuilder()
                  .SetBasePath($"{Directory.GetCurrentDirectory()}\\Core\\Settings\\{path}")
                  .AddJsonFile($"{nameSetting}.json")
                  .Build();
        }

        public HubeeMessageBrokerConfig GetConfig(string nameSetting, string path = "")
        {
            var configuration = GetConfiguration(nameSetting, path);
            var config = new HubeeMessageBrokerConfig();

            configuration.GetSection("HubeeMessageBrokerConfig").Bind(config);

            return config;
        }

        public IConfiguration ConfigBuilder(string messageBroker = "InMemory")
        {
            var configuration = new ConfigurationBuilder().Add(new MemoryConfigurationSource
            {
                InitialData = new Dictionary<string, string>
                {
                    ["HubeeMessageBrokerConfig:MessageBroker"] = $"{messageBroker}",
                    ["HubeeMessageBrokerConfig:ApplicationName"] = "test-service",
                    ["HubeeMessageBrokerConfig:HostName"] = "localhost",
                    ["HubeeMessageBrokerConfig:UserName"] = "guest",
                    ["HubeeMessageBrokerConfig:Password"] = "guest",
                }
            }).Build();

            return configuration;
        }
    }
}