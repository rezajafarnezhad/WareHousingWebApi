using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Migrations;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;

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
        private GenericClass<FiscalYear> _fiscalYear;
        private GenericClass<WareHouse> _wareHouse;
        private GenericClass<Inventory> _inventory;
        private GenericClass<ProductPrice> _productPrice;
        private GenericClass<ProductLocation> _productLocation;
        private GenericClass<Entities.Entities.UserInWareHouse> _userInWareHouse;
        private GenericClass<Customer> _customer;
        private GenericClass<Invoice> _invoise;
        private GenericClass<Entities.Entities.InvoiceItems> _invoiceanditem;

        public GenericClass<Users> usersUw
        {
            get
            {
                if (_users == null)
                    _users = new GenericClass<Users>(_context);

                return _users;
            }
        }
        public GenericClass<Entities.Entities.UserInWareHouse> userInWareHouseUW
        {
            get
            {
                if (_userInWareHouse == null)
                    _userInWareHouse = new GenericClass<Entities.Entities.UserInWareHouse>(_context);

                return _userInWareHouse;
            }
        }

        public GenericClass<Invoice> invoiceUW
        {
            get
            {
                if (_invoise == null)
                    _invoise = new GenericClass<Invoice>(_context);

                return _invoise;
            }
        }

        public GenericClass<InvoiceItems> invoiceItemsUW
        {
            get
            {
                if (_invoiceanditem == null)
                    _invoiceanditem = new GenericClass<InvoiceItems>(_context);

                return _invoiceanditem;
            }
        }
        public GenericClass<Customer> customerUW
        {
            get
            {
                if (_customer == null)
                    _customer = new GenericClass<Customer>(_context);

                return _customer;
            }
        }

        public GenericClass<ProductPrice> productPriceUW
        {
            get
            {
                if (_productPrice == null)
                    _productPrice = new GenericClass<ProductPrice>(_context);

                return _productPrice;
            }
        }
        public GenericClass<ProductLocation> productLocationUW
        {
            get
            {
                if (_productLocation == null)
                    _productLocation = new GenericClass<ProductLocation>(_context);

                return _productLocation;
            }
        }
        public GenericClass<FiscalYear> fiscalYearUw
        {
            get
            {
                if (_fiscalYear == null)
                    _fiscalYear = new GenericClass<FiscalYear>(_context);

                return _fiscalYear;
            }
        } 
        public GenericClass<Inventory> inventoryUw
        {
            get
            {
                if (_inventory == null)
                    _inventory = new GenericClass<Inventory>(_context);

                return _inventory;
            }
        }

        public GenericClass<WareHouse> wareHouseUw
        {
            get
            {
                if (_wareHouse == null)
                    _wareHouse = new GenericClass<WareHouse>(_context);

                return _wareHouse;
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

        /// <summary>
        /// دریافت موجوذی هر سری ساخت
        /// </summary>
        /// <param name="inventoryId"></param>
        /// <returns></returns>
        public async Task<int> GetPhysicalStockForBranch(int inventoryId)
        {
            var _physicalStock = this.inventoryUw.GetEnNoTraking
                .Where(c => c.Id == inventoryId || c.ReferenceId == inventoryId)
                .Sum(x => x.OperationType == 1 ? x.ProductCountMain :
                    x.OperationType == 2 ? -x.ProductCountMain :
                    x.OperationType == 6 ? x.ProductCountMain :
                    x.OperationType == 5 ? -x.ProductCountMain : 0);
       
            return _physicalStock;
        }
    }

}
