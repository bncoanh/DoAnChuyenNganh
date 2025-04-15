using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string? ImageUrl { get; set; }
    public string? FileName { get; set; }

    public int? ProductId { get; set; }
}
