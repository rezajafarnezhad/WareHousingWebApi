using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Users : IdentityUser<string> , IEntityObject
{
    public string FirstName { get; set; }
    public string Family { get; set; }

    [NotMapped]
    public string FullName  => $"{FirstName} {Family}";
    public string UserImage { get; set; }
    public string PersonalCode { get; set; }
    public string MelliCode { get; set; }
    public DateTime BirthDayDate { get; set; }
    
    //1 ادمین
    //2 عاذی
    public byte UserType { get; set; }

    public bool Gender { get; set; }
}