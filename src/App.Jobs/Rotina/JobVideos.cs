using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Jobs.Rotina
{
    public class JobVideos : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public JobVideos(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
                using (var scope = _serviceScopeFactory.CreateScope()) // Cria um escopo
                {
                    var consumerVideo = scope.ServiceProvider.GetRequiredService<IVideosConsumer>();
                    await consumerVideo.ConsumirVideo();
                }

               // await Task.Delay(2000000, stoppingToken);
            //}
        }
    }
}
