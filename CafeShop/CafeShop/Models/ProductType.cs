using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? GroupType { get; set; }

    public string? Description { get; set; }
}
