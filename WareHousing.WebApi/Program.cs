
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WareHousingWebApi.Data.DbContext;
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
builder.Services.AddScoped<IRialStockRepo,RialStockRepo>();
builder.Services.AddScoped<IWastageRialStockRepo,WastageRialStockRepo>();
builder.Services.AddScoped<IInvoiceRepo, InvoiceRepo>();


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


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();
using var scope = app.Services.CreateScope();
ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.Migrate();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("WareHousingCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    app.UseDeveloperExceptionPage();
}

app.Run();