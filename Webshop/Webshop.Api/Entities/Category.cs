using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Api.Interfaces;

namespace Webshop.Api.Entities;

public class Category : IUniqueEntity<string>
{
    public string Id { get; set; } = null!;

    public List<ProductCategory> Products { get; set; } = new();
}
