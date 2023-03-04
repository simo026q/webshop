using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class ProductOffer : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid ProductVariantId { get; set; }
    public double OffPercentage { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
}
