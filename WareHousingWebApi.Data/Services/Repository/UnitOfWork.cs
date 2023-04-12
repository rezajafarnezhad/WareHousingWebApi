﻿using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousingWebApi.Data.Services.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

        }

        private GenericClass<Users> _users;
        private GenericClass<Roles> _roles;
        private GenericClass<Products> _products;
        private GenericClass<Supplier> _Suppliers;
        private GenericClass<Country> _Country;

        public GenericClass<Users> usersUw
        {
            get
            {
                if (_users == null)
                    _users = new GenericClass<Users>(_context);

                return _users;
            }
        }

        public GenericClass<Roles> rolesUw
        {
            get
            {
                if (_roles == null)
                    _roles = new GenericClass<Roles>(_context);

                return _roles;
            }
        }

        public GenericClass<Supplier> SupplierUw
        {
            get
            {
                if (_Suppliers == null)
                    _Suppliers = new GenericClass<Supplier>(_context);

                return _Suppliers;
            }
        }

        public GenericClass<Products> productsUw
        {
            get
            {
                if (_products == null)
                    _products = new GenericClass<Products>(_context);

                return _products;
            }
        }

        public GenericClass<Country> CountryUw
        {
            get
            {
                if (_Country == null)
                    _Country = new GenericClass<Country>(_context);

                return  _Country;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public IEntityTransaction BeginTransaction() 
        {
            return new EntityTransaction(_context);
        }

    }

}