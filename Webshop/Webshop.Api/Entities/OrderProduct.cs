using System.ComponentModel.DataAnnotations.Schema;

namespace Webshop.Api.Entities;

public class OrderProduct
{
    public Guid OrderId { get; set; }
    public Guid ProductVariantId { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public ProductVariant ProductVariant { get; set; } = null!;
}
