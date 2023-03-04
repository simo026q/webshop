using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class Order : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid AddressId { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Address? Address { get; set; }
    public User? User { get; set; }
    public List<OrderProduct> Products { get; set; } = new();
}

public enum OrderStatus
{
    Open,
    Confirmed,
    Processing,
    Completed,
    Canceled
}
