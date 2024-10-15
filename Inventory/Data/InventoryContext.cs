using System;
using System.Collections.Generic;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;

public partial class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<CostCenter> CostCenters { get; set; }
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<Model> Models { get; set; }
    public virtual DbSet<Relation> Relations { get; set; }
    public virtual DbSet<User> Users { get; set; }

    
}
