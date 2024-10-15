using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models;

public partial class Relation
{
    public int Id { get; set; }
    public int? DeviceId { get; set; }
    public virtual Device? Device { get; set; }
    public int? UserId { get; set; }
    public virtual User? User { get; set; }

}
