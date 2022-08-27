using Play.Common;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Clients;

namespace Play.Inventory.Service.Controllers
{

    [ApiController]
    [Route("api/inventory")]
    public class ItemsController : Controller
    {
        private readonly IRepository<InventoryItem> inventoryRepository;
        private readonly CatalogClient catalogClient;

        public ItemsController(IRepository<InventoryItem> repo,
            CatalogClient client)
        {
            inventoryRepository = repo ?? throw new ArgumentNullException(nameof(repo));
            catalogClient = client;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await inventoryRepository.GetAllAsync(item => item.UserId == userId);

            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Desc);
            });

            // var items = (await inventoryRepository
            //                 .GetAllAsync(item => item.UserId == userId))
            //                 .Select(item => item.AsDto());
            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await inventoryRepository
                        .GetAsync(item => item.UserId == grantItemsDto.UserId
                                && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await inventoryRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity = grantItemsDto.Quantity;
                await inventoryRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}