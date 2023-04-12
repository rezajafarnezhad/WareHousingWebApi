using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WareHousing.WebApi.Extensions;
using WareHousingWebApi.Data.DbContext;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Data.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var Configuration = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(FileNamesExtensions.AppSettingName).Build();

//اضافه کردن دیتابیس
builder.Services.AddDbContextService(Configuration);

//اضافه کردن Identity
builder.Services.AddIdentityService(Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WareHousingCors", builder =>
    {
        
        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host)=>true);

    });

});


var app = builder.Build();



app.UseHttpsRedirection();
app.UseCors("WareHousingCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
