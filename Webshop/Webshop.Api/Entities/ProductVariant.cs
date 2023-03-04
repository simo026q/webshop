using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class ProductVariant : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public double? PurchasePrice { get; set; }
    public double SellingPrice { get; set; }
    public bool? IsActive { get; set; }

    public List<ProductOffer> Offers { get; set; } = new();
}
