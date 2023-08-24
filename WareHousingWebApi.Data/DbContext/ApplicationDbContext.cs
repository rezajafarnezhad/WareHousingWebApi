using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Entities.Base;
using WareHousingWebApi.Entities.Entities;

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
    public DbSet<ProductLocation> ProductLocation_tbl{ get; set; }


    #endregion



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var assembly = typeof(IEntityObject).Assembly;
        builder.VerifyEntities<IEntityObject>(assembly);

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
    public override int SaveChanges()
    {
        cleanStr();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        cleanStr();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        cleanStr();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cleanStr();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void cleanStr()
    {
        var changedEntities = ChangeTracker.Entries()
            //پيدا کردن رکوردهایی که عمليات افزودن و ویرایش دارند
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);


        foreach (var item in changedEntities)
        {
            if (item.Entity == null)
                continue;
            //فيلدهايي که مقدارشون استرينگ باشه و قابليت خواندن و نوشتن داشته باشند
            var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                //اگر پراپرتي مقدار داشت
                var propName = property.Name;
                var val = (string)property.GetValue(item.Entity, null);

                if (val.HasValue())
                {
                    var newVal = val.Fa2En().FixPersianChars();
                    if (newVal == val)
                        continue;
                    property.SetValue(item.Entity, newVal, null);
                }
            }
        }
    }



}
