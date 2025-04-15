using System;
using System.Collections.Generic;

namespace CafeShop.Models;

public partial class AccountImage
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public int? ImageId2 { get; set; }

    public bool? IsAvatar { get; set; }
}
