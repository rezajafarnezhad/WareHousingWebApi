namespace WareHousingWebApi.Data.Services.Interface
{
    public interface IEntityTransaction : IDisposable
    {
        void Commit();
        void RollBack();
    }
}
