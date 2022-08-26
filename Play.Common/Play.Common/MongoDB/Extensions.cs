using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Common.Settings;

namespace Play.Common.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoDatabase>(options =>
               {
                   var mongoSettings = configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
                   var serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
                   var client = new MongoClient(mongoSettings.ConnectionString);
                   return client.GetDatabase(serviceSettings.ServiceName);
               });

            return services;
        }

        public static IServiceCollection AddMongRepo<T>(this IServiceCollection services, string collectionName)
            where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(options =>
            {
                var db = options.GetService<IMongoDatabase>();
                return new MongoRepository<T>(db, collectionName);
            });

            return services;
        }
    }
}