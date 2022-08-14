using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Desc, int Price, DateTimeOffset CreatedTime);
    public record CreateItemDto([Required] string Name, string Desc, [Range(0, 100)] int Price);
    public record UpdateItemDto([Required] string Name, string Desc, [Range(0, 100)] int Price);
}