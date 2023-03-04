using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class CategoryRequest
{
    public string Id { get; set; } = null!;

    public Category ToEntity()
    {
        return new()
        {
            Id = Id,
        };
    }
}