
using Microsoft.AspNetCore.Identity;

namespace WareHousingWebApi.Data.Entities;

public class Users : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string Family { get; set; }
    public string UserImage { get; set; }
    public string PersonalCode { get; set; }
    public string MelliCode { get; set; }
    public DateTime BirthDayDate { get; set; }
    
    //1 ادمین
    //2 عاذی
    public byte UserType { get; set; }

    public bool Gender { get; set; }
}




