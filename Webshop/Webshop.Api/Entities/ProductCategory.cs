using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Api.Entities;

public class ProductCategory
{
    public Guid ProductId { get; set; }
    public string CategoryId { get; set; } = null!;

    public Product Product { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
