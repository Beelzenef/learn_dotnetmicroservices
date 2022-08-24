using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repos
{
    public interface IItemsRepository
    {
        Task CreateAsync(Item item);
        Task DeleteAsync(Guid id);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task UpdateAsync(Item item);
    }
}