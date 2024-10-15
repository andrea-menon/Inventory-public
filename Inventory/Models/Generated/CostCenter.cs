using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class CostCenter
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string? Name { get; set; }
    [MaxLength(30)]
    public string? Description { get; set; }
    public virtual ICollection<User>? Users { get; set; } = new List<User>();
}
