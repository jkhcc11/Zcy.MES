using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Zcy.MongoDB
{
    public class ZcyMongodbContext
    {
        public readonly IMongoDatabase Database;
        public readonly IServiceCollection ServiceCollection;

        public ZcyMongodbContext(IMongoClient mongoClient, string database,
            IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
            Database = mongoClient.GetDatabase(database);
        }
    }
}
