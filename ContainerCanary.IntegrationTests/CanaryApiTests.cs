using RestAssured.Net;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace ContainerCanary.IntegrationTests
{
    public class CanaryApiTests
    {
        private readonly string _baseUrl;

        public CanaryApiTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _baseUrl = configuration["BaseUrls:CanaryApi"];
        }

        [Fact]
        public void GetCanaryEndpoint_ShouldReturnSuccess()
        {
            // Arrange
            string endpoint = "/canary";

            // Act & Assert
            RestAssured
                .Given()
                .When()
                .Get(_baseUrl + endpoint)
                .Then()
                .StatusCode(200);
        }
    }
}