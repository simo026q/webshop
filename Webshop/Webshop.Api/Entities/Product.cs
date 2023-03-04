using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class Product : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ProductCategory> Categories { get; set; } = new();
    public List<ProductVariant> Variants { get; set; } = new();
}
