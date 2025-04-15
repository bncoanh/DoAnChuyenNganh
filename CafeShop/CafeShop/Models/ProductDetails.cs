using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class ProductDetails
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ProductSizeId { get; set; }

    public decimal? Price { get; set; }
}
