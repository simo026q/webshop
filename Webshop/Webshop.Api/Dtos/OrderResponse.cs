using Webshop.Api.Entities;

namespace Webshop.Api.Dtos
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public Address? Address { get; set; }
        public List<OrderProductResponse> Products { get; set; } = new();
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        

        public static OrderResponse FromEntity(Order entity)
        {
            return new OrderResponse
            {
                Id = entity.Id,
                AddressId = entity.AddressId,
                UserId = entity.UserId,
                Status = entity.Status,
                Address = entity.Address,
                Products = entity.Products.Select(v => OrderProductResponse.FromEntity(v)).ToList(),
                CreatedAt = DateTime.UtcNow,
                TotalPrice = entity.Products.Select(v => OrderProductResponse.FromEntity(v)).ToList().Sum(v => v.TotalPrice)
            };
        }
    }
}
