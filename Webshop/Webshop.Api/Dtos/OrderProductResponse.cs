using Webshop.Api.Entities;

namespace Webshop.Api.Dtos
{
    public class OrderProductResponse
    {
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }
        public double TotalPrice { get; set; }
        public int Quantity { get; set; }
        public ProductVariantResponse? ProductVariant { get; set; }
        
        public static OrderProductResponse FromEntity(OrderProduct entity)
        {
            return new OrderProductResponse
            {
                OrderId = entity.OrderId,
                ProductVariantId = entity.ProductVariantId,
                TotalPrice = (entity.ProductVariant?.SellingPrice ?? 0) * entity.Quantity,
                Quantity = entity.Quantity,
                ProductVariant = ProductVariantResponse.FromEntity(entity.ProductVariant)
            };
        }
    }
}
