using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Redis
{
    public static class Extensions
    {
        public static readonly string SectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RedisOptions>(configuration.GetSection(SectionName));
            var options = configuration.GetOptions<RedisOptions>(SectionName);

            services.AddDistributedRedisCache(
                x =>
                {
                    x.Configuration = options.ConnectionString;
                    x.InstanceName = options.Instance;
                });

            return services;
        }

        public static IServiceCollection AddRedisClient(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RedisOptions>(configuration.GetSection(SectionName));
            var options = configuration.GetOptions<RedisOptions>(SectionName);

            services.AddSingleton<IRedisClientsManager>(c => new RedisManagerPool(options.ConnectionString));
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();

            return services;
        }
    }
}
