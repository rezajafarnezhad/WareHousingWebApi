using Microsoft.EntityFrameworkCore.Storage;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousingWebApi.Data.Services.Repository
{
    public class EntityTransaction : IEntityTransaction
    {
        private readonly IDbContextTransaction _transaction;
        public EntityTransaction(ApplicationDbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        // همه دستورات با موفقیت انحام شد
        public void Commit()
        {
            _transaction.Commit();
        }
        // برای خالی کردن حافظه
        public void Dispose()
        {
            _transaction.Dispose();
        }

        // وقتی خطایی رخ داد
        public void RollBack() { _transaction.Rollback(); } 

    }

}
