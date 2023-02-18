using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

using Spents.EventSourcing.CrossCuting.Models;

namespace Spents.EventSourcing.CrossCuting.Extensions
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings mongoSettings)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                var options = sp.GetService<MongoSettings>();
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
                return new MongoClient(mongoSettings.ConnectionString);
            });

            services.AddSingleton(sp =>
            {
                var mongoClient = sp.GetService<IMongoClient>() ?? throw new Exception("MongoDB was not injectable.");
                var db = mongoClient.GetDatabase(mongoSettings.Database);
                return db;
            });

            return services;
        }
    }
}
