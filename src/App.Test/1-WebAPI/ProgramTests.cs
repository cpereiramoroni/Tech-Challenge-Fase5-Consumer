using App.Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace APICliente.Tests
{
    internal class MoroniWebApplication : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            _ = builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IVideosService, VideosService>();
            });
        }
    }
    public class ProgramTests
    {

        [Fact]
        public async Task ShoudReturns200_WhenGetValidPath()
        {
            // Arrange
            await using var application = new MoroniWebApplication();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/swagger");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }


}
