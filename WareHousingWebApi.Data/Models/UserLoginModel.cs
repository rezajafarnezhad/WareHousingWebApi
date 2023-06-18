using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHousingWebApi.Data.Models
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
