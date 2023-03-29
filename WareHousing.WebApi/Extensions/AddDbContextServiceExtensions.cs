using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WareHousingWebApi.Data.DbContext;

namespace WareHousing.WebApi.Extensions;

public static class AddDbContextServiceExtensions
{
    public static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("HareHousingConnectionString")));
        return services;
    }
}


