using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class Model
{
    public int Id { get; set; }
    [MaxLength(30)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Description { get; set; }
    public int? BrandId { get; set; }
    public virtual Brand? Brand { get; set; }
    public virtual ICollection<Device>? Devices { get; set; } = new List<Device>();
}
