using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="نام کاربری اجباری است")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "کلمه عبور اجباری است")]

        public string Password { get; set; } 
        
        [Required(ErrorMessage = "سال مالی اجباری است")]

        public int FiscalYearId { get; set; }
    }

    public class UserJwtToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public IList<string>? Roles { get; set; }
    }
}


public class DropDownDto
{
    public int DrId { get; set; }
    public string DrName { get; set; }
}