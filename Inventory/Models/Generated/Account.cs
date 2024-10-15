using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class Account
{
    public int Id { get; set; }
    [MaxLength(15)]
    public string? Oa { get; set; }
    [MaxLength(15)]
    public string? Rda { get; set; }
    [MaxLength(20)]
    public string? Car { get; set; }
    public virtual ICollection<Device>? Devices { get; set; } = new List<Device>();
}
