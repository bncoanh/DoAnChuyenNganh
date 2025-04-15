using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public string? Description { get; set; }

    public int? ProductTypeId { get; set; }
}
