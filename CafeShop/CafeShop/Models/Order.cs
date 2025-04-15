using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }
    public string? OrderCode { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public int? Status { get; set; }

    public DateTime CreateDate { get; set; }

    public string? CreateBy { get; set; }

    public int AccountId { get; set; }
}
