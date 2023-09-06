using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models
{
    public class CrateUser
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری را وارد کنید")]

        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نام را وارد کنید")]

        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "فامیلی را وارد کنید")]

        public string Family { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "تصویر را وارد کنید")]

        public string UserImage { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "کدپرسنلی را وارد کنید")]

        public string PersonalCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "کدملی را وارد کنید")]

        public string MelliCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ تولد را وارد کنید")]
        public string BirthDayDate { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره موبایل وارد کنید")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "انبار اجیاری است")]
        public List<int> WareHouseIds { get; set; }
        //1 ادمین
        //2 عاذی
        public byte UserType { get; set; }

        public bool Gender { get; set; }
       
        /// <summary>
        /// کاربر ثبت کننده
        /// </summary>
        public string UserId { get; set; }
    }

    public class EditUser : CrateUser
    {
        public string Id { get; set; }

    }
}