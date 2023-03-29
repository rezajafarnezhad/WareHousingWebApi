using Microsoft.AspNetCore.Identity;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Entities;

namespace WareHousing.WebApi.Extensions;

public static class AddIdentityExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<Users,Roles>(opt =>
        {
            opt.Password.RequireDigit= true;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequireNonAlphanumeric = false;

        })
            .AddRoles<Roles>()
            .AddRoleManager<RoleManager<Roles>>()
            .AddRoleValidator<RoleValidator<Roles>>()
            .AddSignInManager<SignInManager<Users>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()

            ;
           
        return services;
    }
}


