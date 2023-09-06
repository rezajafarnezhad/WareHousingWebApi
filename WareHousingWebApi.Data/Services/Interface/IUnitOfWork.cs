
using WareHousingWebApi.Data.Services.Repository;
using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.Data.Services.Interface
{
    public interface IUnitOfWork
    {
        GenericClass<Country> CountryUw { get; }
        GenericClass<Products> productsUw { get; }
        GenericClass<Supplier> SupplierUw { get; }
        GenericClass<Roles> rolesUw { get; }
        GenericClass<Users> usersUw { get; }
        GenericClass<FiscalYear> fiscalYearUw { get; }
        GenericClass<WareHouse> wareHouseUw { get; } 
        GenericClass<Inventory> inventoryUw { get; }
        GenericClass<ProductPrice> productPriceUW  { get; }
        GenericClass<ProductLocation> productLocationUW  { get; }
        GenericClass<UserInWareHouse> userInWareHouseUW { get; }

        void Save();
        Task SaveAsync();
    }
}
