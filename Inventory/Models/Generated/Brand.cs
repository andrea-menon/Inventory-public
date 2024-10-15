using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class Brand
{
    public int Id { get; set; }
    [MaxLength(30)]
    public string? Name { get; set; }
    public virtual ICollection<Model>? Models { get; set; } = new List<Model>();
}
