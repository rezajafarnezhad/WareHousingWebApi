using WareHousingWebApi.Data.Entities;

namespace WareHousing.WebApi.Tools.Interface
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(Users user,IList<string> Roles);
    }
}
