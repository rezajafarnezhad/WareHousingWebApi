
using Microsoft.AspNetCore.Identity;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Roles : IdentityRole<string> , IEntityObject
{

}