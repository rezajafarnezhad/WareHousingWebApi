using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Services.Repository;

namespace WareHousingWebApi.Data.Services.Interface
{
    public interface IUnitOfWork
    {
        GenericClass<Country> CountryUw { get; }
        GenericClass<Products> productsUw { get; }
        GenericClass<Supplier> SupplierUw { get; }
        GenericClass<Roles> rolesUw { get; }
        GenericClass<Users> usersUw { get; }

        void Save();
        Task SaveAsync();
    }
}
