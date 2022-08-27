using Play.Common;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Play.Inventory.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : Controller
    {
        private readonly IRepository<InventoryItem> inventoryRepository;

        public ItemsController(IRepository<InventoryItem> repo)
        {
            inventoryRepository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var items = (await inventoryRepository
                            .GetAllAsync(item => item.UserId == userId))
                            .Select(item => item.AsDto());
            return Ok(items);
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