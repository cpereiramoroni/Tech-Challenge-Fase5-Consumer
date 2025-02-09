using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Infra.CrossCutting.IoC;
using App.Test.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace App.Test._4_Infra._4._2___CrossCutting
{
    public class NativeInjectorBootStrapperTest
    {
        public IServiceCollection services;

        public NativeInjectorBootStrapperTest()
        {

            var myConfiguration = new Dictionary<string, string>
            {
                {"ConnectionProdutos", "Value1"},

                
                {"UrlRabbit", "amqps://sasss:ssss@jose.ee.aa.com/aaa"},


            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();


            services = new ServiceCollection();
            NativeInjectorBootStrapper.RegisterServices(services, configuration);
        }

        [Trait("Categoria", "NativeInjectorBootStrapper")]
        [Theory(DisplayName = "NativeInjectorBootStrapper")]
        [InlineData(typeof(IVideosRepository))]

        [InlineData(typeof(IVideosService))]



        public void NativeInjectorBootStrapperValido(System.Type type)
        {

            var isValid = services.GetEnumerator().ToList().Any(_ => _.ServiceType == type);
            Assert.NotNull(services);
            Assert.True(isValid);
        }
    }
}
