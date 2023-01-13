using AuthService.Controllers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TestAuthService
{
    [TestClass]
    public class TestAuthController
    {
        IConfiguration config;
        IMapper mapper;
        AuthController controller;

        [TestInitialize]
        public void Setup()
        {
            this.mapper = null;

            // Configuration
            var text = File.ReadAllText("appsettings.Development.json");
            Dictionary<string, string> configJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { })
                .Build();

            this.config = configuration;
            this.controller = new AuthController(mapper, config);
        }

        public TestAuthController()
        {

        }
        public TestAuthController(IMapper mapper, IConfiguration config)
        {
            this.mapper = mapper;
            this.config = config;
            this.controller = new AuthController(mapper, config);
        }
        
        [TestMethod]
        public void TestGeneratedRedirectUrl()
        {
            // Arrange
            var url = controller.Auth();

            // Act
            //var clientSecret = config["Spotify:ClientId"];

            // Assert
            Assert.IsTrue(url is string);
        }
    }
}