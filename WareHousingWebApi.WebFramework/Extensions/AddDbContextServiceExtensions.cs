using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WareHousingWebApi.Data.DbContext;

namespace WareHousingWebApi.WebFramework.Extensions;

public static class AddDbContextServiceExtensions
{
    public static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("HareHousingConnectionString")));
        return services;
    }
}


