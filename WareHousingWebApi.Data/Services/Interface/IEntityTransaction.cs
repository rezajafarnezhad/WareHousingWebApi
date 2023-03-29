namespace WareHousingWebApi.Data.Services.Interface
{
    public interface IEntityTransaction
    {
        void Commit();
        void Dispose();
        void RollBack();
    }
}
