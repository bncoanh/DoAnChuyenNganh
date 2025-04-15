using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalMoney { get; set; }

    public int? OrderId { get; set; }

    public int? ProductDetailId { get; set; }
}
