using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class CategoryResponse
{
    public string Id { get; set; } = null!;

    public static CategoryResponse FromEntity(Category category)
    {
        return new()
        {
            Id = category.Id,
        };
    }
}
