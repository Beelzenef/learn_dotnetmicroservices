using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repos
{
    public class ItemsRepository
    {
        private const string COLLECTION_NAME = "items";

        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var db = mongoClient.GetDatabase("Catalog");
            dbCollection = db.GetCollection<Item>(COLLECTION_NAME);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, id);
            return await dbCollection.Find(filter).FirstAsync();
        }

        public async Task CreateAsync(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            await dbCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, item.Id);
            await dbCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}