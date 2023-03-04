using Webshop.Api.Entities;

namespace Webshop.Api.Dtos
{
    public class OrderRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AddressRequest Address { get; set; }
        public List<OrderProductRequest> Products { get; set; } = new();

        public Order ToEntity()
        {
            return new()
            {
                Id = Id,
                UserId = UserId,
                Address = Address.ToEntity(),
                Products = Products.Select(v => v.ToEntity()).ToList()
            };
        }
    }
}
