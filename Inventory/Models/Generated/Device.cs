using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class Device
{
    public int Id { get; set; }
    [MaxLength(30)]
    public string? Sn { get; set; }
    public int? ModelId { get; set; }
    public virtual Model? Model { get; set; }
    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }
    public virtual ICollection<Relation>? Relations { get; set; } = new List<Relation>();
    [MaxLength(20)]
    public string? Zhe { get; set; }
    [MaxLength(20)]
    public string? Stato { get; set; }
    public DateOnly? Data { get; set; }
    public string? Note { get; set; }
}
