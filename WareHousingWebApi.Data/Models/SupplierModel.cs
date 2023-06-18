using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHousingWebApi.Data.Contract;
using WareHousingWebApi.Data.Entities;

namespace WareHousingWebApi.Data.Models
{
    public class SupplierModel
    {
       
        [Required(AllowEmptyStrings =false,ErrorMessage = "نام را وارد کنید")]
        public string SupplierName { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage = "تلفن را وارد کنید")]
        public string SupplierTel { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierSite { get; set; }
    }


    public class SupplierEditModel : SupplierModel 
    {
        public int SupplierId { get; set; }

    }
}

