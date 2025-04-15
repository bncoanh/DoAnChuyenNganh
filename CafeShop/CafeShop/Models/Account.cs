using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string? PassWord { get; set; }

    public int? Role { get; set; } 

    public string? FullName { get; set; }

    public int? Gender { get; set; } 

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
    public int? IsActive { get; set; } 
}
