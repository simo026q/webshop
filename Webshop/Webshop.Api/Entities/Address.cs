using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class Address : IUniqueEntity<Guid>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string City { get; set; }
    public string StreetName { get; set; }
    public string StreetNumber { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
}
