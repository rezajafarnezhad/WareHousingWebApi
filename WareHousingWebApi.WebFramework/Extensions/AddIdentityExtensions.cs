using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Entities.Entities;

namespace WareHousingWebApi.WebFramework.Extensions;

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

        services.AddAuthentication(op =>
        {
            op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                TokenDecryptionKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["secKey"]))
            };
            op.SaveToken = true;
            op.RequireHttpsMetadata = false;

        });


        return services;
    }
}


