using Webshop.Api.Entities;

namespace Webshop.Api.Dtos
{
    public class OrderProductRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }

        public OrderProduct ToEntity()
        {
            return new OrderProduct
            {
                OrderId = OrderId,
                ProductVariantId = ProductVariantId,
                Quantity = Quantity,
            };
        }
    }
}
