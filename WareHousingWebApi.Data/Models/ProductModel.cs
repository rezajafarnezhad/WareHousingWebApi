using System.ComponentModel.DataAnnotations;
using WareHousingWebApi.Data.Contract;

namespace WareHousingWebApi.Data.Models
{
    public class ProductCreatModel
    {


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام را وارد کنید")]
        public string ProductName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "نوع بسته بندی کالا انتخاب نشده")]
        public PackingType PackingType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد کالا را وارد کنید")]

        public int CountInPacking { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "وزن کالا را وارد کنید")]

        public int ProductWeight { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "تصویر کالا را وارد کنید")]

        public string ProductImage { get; set; }
        [MaxLength(500, ErrorMessage = "حداکثر 500 کاراکتر")]
        public string ProductDescription { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "کشور سازنده انتخاب نشده")]
        public int CountryId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "تامین کننده انتخاب نشده")]
        public int SupplierId { get; set; }

        /// <summary>
        /// 1 یخچالی
        /// 2 عاذی
        /// </summary>
        public byte IsRefregerator { get; set; }
    }


    public class ProductEditModel : ProductCreatModel
    {
        public int ProductId { get; set; }

    }

}
