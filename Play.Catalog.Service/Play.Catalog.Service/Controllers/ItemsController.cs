using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new List<ItemDto>{
            new ItemDto(Guid.NewGuid(), "Potion", "Restores 10 health points", 5, DateTime.UtcNow),
            new ItemDto(Guid.NewGuid(), "Rage", "Increases attack", 15, DateTime.UtcNow),
            new ItemDto(Guid.NewGuid(), "Poison", "Removes 10 health points", 25, DateTime.UtcNow)
        };

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(x => x.Id == id).SingleOrDefault();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<ItemDto> Create(CreateItemDto dto)
        {
            var item = new ItemDto(Guid.NewGuid(), dto.Name, dto.Desc, dto.Price, DateTime.UtcNow);
            items.Add(item);

            return CreatedAtAction("Created item", items.Last());
        }

        [HttpPut]
        [Route("[action]")]
        public ActionResult<ItemDto> Update(Guid id, UpdateItemDto dto)
        {
            var itemToUpdate = items.Where(x => x.Id == id).SingleOrDefault();

            if (itemToUpdate == null)
            {
                return NotFound();
            }

            var updatedItem = itemToUpdate with
            {
                Name = dto.Name,
                Desc = dto.Desc,
                Price = dto.Price
            };

            // var index = items.FindIndex(x => x.Id == updatedItem.Id);
            var index = items.IndexOf(itemToUpdate);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult Remove(Guid id)
        {
            // var index = items.FindIndex(x => x.Id == id);
            // items.RemoveAt(index);
            var itemToDelete = items.Where(x => x.Id == id).SingleOrDefault();

            if (itemToDelete == null)
            {
                return NotFound();
            }

            items.Remove(itemToDelete);

            return NoContent();
        }
    }
}