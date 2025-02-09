using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models.External;
using App.Infra.Data.Context;
using App.Jobs;
using App.Jobs.Consumer;
using App.Jobs.Rotina;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;


namespace App.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {
            




            ///     variables
            ///     
            services.AddSingleton<AwsConfig>(_ =>
               new AwsConfig
               {
                   S3Access = config["AWS_ACCESS_KEY_ID"],
                   S3Secret = config["AWS_SECRET_ACCESS_KEY"]
               });

         


            ////=======================================================================
            ///
            ///  INSTACIAS DE SERVICES
            /// 
            ///

            services.AddTransient<IVideosService, VideosService>();
            services.AddTransient<IAwsService,AwsService>();
            ////=======================================================================
            ///
            ///  INSTACIAS DE REPOSITORY
            /// 
            ///
            services.AddTransient<IVideosRepository, VideosRepository>();

            
            //rabitMQ 

            
         //   services.AddSingleton<RabbitMQConfig>(_ =>
         //new RabbitMQConfig
         //{
         //    Uri = new Uri(config["UrlRabbit"]),

         //});
            var fac = new ConnectionFactory()
            {
                Uri = new Uri(config["UrlRabbit"]),
                AutomaticRecoveryEnabled = true,

                NetworkRecoveryInterval = TimeSpan.FromSeconds(1000),
                DispatchConsumersAsync = true

            };


            services.AddSingleton(fac);
            services.AddSingleton(_ => _.GetRequiredService<ConnectionFactory>().CreateConnection());
            services.AddTransient(_ => _.GetRequiredService<IConnection>().CreateModel());



            //HOSTED SERVICES
            
            services.AddHostedService<JobVideos>();
            services.AddSingleton<IVideosConsumer, VideosConsumer>();




            ////=======================================================================
            ///
            ///  INSTACIAS DE CONTEXTO
            /// 
            ///
            services.AddSingleton<MySQLContextFactory>();

            //     services.AddDbContext<MySQLContext>(options =>
            //options.UseMySql(config["ConnectionVideos"], ServerVersion.AutoDetect(config["ConnectionVideos"])));

            //services.AddScoped<MySQLContext>();

        }
    }
}
