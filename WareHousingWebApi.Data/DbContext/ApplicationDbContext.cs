using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.Entities;

namespace WareHousingWebApi.Data.DbContext;

public class ApplicationDbContext : IdentityDbContext<Users, Roles, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


    #region DbSet
    public DbSet<Products> Products_tbl { get; set; }
    public DbSet<Country> Countries_tbl { get; set; }
    public DbSet<Supplier> Suppliers_tbl { get; set; }
    public DbSet<FiscalYear> FiscalYears_tbl { get; set; }
    public DbSet<WareHouse> wareHouses_tbl { get; set; }
    public DbSet<Inventory> Inventories_tbl { get; set; }
    public DbSet<ProductPrice> ProductPrices_tbl{ get; set; }
    #endregion



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Users>(entity =>
        {
            entity.ToTable("Users_tbl");
            entity.Property(c => c.Id).HasColumnName("UserId");
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
        });

        builder.Entity<Roles>(entity =>
        {
            entity.ToTable("Roles_tbl");
        });
    }

}
