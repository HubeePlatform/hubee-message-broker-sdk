using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Hubee.MessageBroker.Sdk.Extensions;
using Hubee.MessageBroker.Sdk.Core.Exceptions;

namespace Hubee.MessageBroker.Sdk.Tests.Core
{
    public class ConfigurationTest : BaseTest
    {
        [Theory]
        [InlineData("invalid_application_name", true)]
        [InlineData("invalid_in_memory", true)]
        [InlineData("invalid_rabbitmq", true)]
        public void Should_DoNotAcceptSettings_When_Invalid(string nameSetting, bool expected)
        {
            var config = GetConfig(nameSetting, "Invalid");
            Assert.Equal(expected, config.IsInvalid());
        }

        [Theory]
        [InlineData("valid_in_memory", false)]
        [InlineData("valid_rabbitmq", false)]
        public void Should_AcceptSettings_When_valid(string nameSetting, bool expected)
        {
            var config = GetConfig(nameSetting, "Valid");

            Assert.Equal(expected, config.IsInvalid());
        }

        [Theory]
        [InlineData("Kafka")]
        [InlineData("ActiveMQ")]
        [InlineData("")]
        public void Should_ThrowException_When_MessageBrokerNotSupported(string nameMessageBroker)
        {
            var serviceCollection = new ServiceCollection();
            Assert.Throws<MessageBrokerNotSupportedException>(() => serviceCollection.AddEventBus(ConfigBuilder(nameMessageBroker)));
        }
    }
}