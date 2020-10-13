using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace NetCoreAPI.ElasticsearchConfiguration
{
    public static class ElasticseaarchExtensions
    {
        public static void AddElasticsearch(
                this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ElasticSearchConfig:Uri"];
            var defaultIndex = configuration["ElasticSearchConfig:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}