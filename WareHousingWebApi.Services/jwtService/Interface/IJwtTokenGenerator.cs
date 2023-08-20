using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.Services.jwtService.Interface
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(Users user,IList<string> Roles);
    }
}
