using Webshop.Api.Entities;

namespace Webshop.Api.Dtos;

public class AddressRequest
{
    public string City { get; set; }
    public string StreetName { get; set; }
    public string StreetNumber { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public Address ToEntity() 
        => new()
        {
            Id = Guid.Empty,
            City = City,
            StreetName = StreetName,
            StreetNumber = StreetNumber,
            ZipCode = ZipCode,
            Country = Country,
        };
}