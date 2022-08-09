namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, string Desc, int Price, DateTimeOffset CreatedTime);
    public record CreateItemDto(string Name, string Desc, int Price);
    public record UpdateItemDto(string Name, string Desc, int Price);
}