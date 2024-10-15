using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class User
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? Name { get; set; }
    public virtual ICollection<Relation>? Relations { get; set; } = new List<Relation>();
    public int? CostCenterId { get; set; }
    public virtual CostCenter? CostCenter { get; set; }
    [MaxLength(15)]
    public string? Di { get; set; }
}
