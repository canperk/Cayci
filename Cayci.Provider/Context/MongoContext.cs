using Cayci.Entities.Models;
using MongoDB.Driver;
using System;

namespace Cayci.Provider.Context
{
    public class MongoContext
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static bool IsSSL { get; set; }

        private IMongoDatabase _database { get; }

        public MongoContext()
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }
        public IMongoDatabase Database { get { return _database; } }
        public IMongoCollection<User> Users { get { return _database.GetCollection<User>("Users"); } }
        public IMongoCollection<UserRequest> UserRequests { get { return _database.GetCollection<UserRequest>("UserRequests"); } }
        public IMongoCollection<RequestType> RequestTypes { get { return _database.GetCollection<RequestType>("RequestTypes"); } }
        public IMongoCollection<Location> Locations { get { return _database.GetCollection<Location>("Locations"); } }
    }
}
