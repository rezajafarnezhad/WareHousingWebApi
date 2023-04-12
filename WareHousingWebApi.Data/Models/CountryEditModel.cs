using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Data.Models
{
    public class CountryEditModel
    {
        public int CountryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام را وارد کنید")]
        public string CountryName { get; set; }
    }
}
