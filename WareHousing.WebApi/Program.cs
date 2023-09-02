﻿
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Data.Services.Repository;
using WareHousingWebApi.Services.jwtService;
using WareHousingWebApi.Services.jwtService.Interface;
using WareHousingWebApi.WebFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();

var Configuration = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(FileNamesExtensions.AppSettingName).Build();
//اضافه کردن دیتابیس
builder.Services.AddDbContextService(Configuration);
//AutoMapper

builder.Services.AddAutoMapper(typeof(Program));
//اضافه کردن Identity
builder.Services.AddIdentityService(Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFiscalYearRepo,FiscalYearRepo>();
builder.Services.AddScoped<IInventoryRepo,InventoryRepo>();
builder.Services.AddScoped<IProductPriceRepo,ProductPriceRepo>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "WareHousingCors", builder =>
    {

        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host) => true);

    });

});

builder.Services.AddControllers().ConfigureApiBehaviorOptions(op =>
{
    op.SuppressModelStateInvalidFilter=true;

});
var app = builder.Build();


app.UseStaticFiles();
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