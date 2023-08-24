
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHousingWebApi.WebFramework.ApiResult
{

    public class ApiResponse
    {
        public bool flag { get; set; }
        public string Message { get; set; }
        public ApiStatusCode StatusCode { get; set; }

    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }


    public enum ApiStatusCode
    {
        [Display(Name = "عملیات موفق بود")]
        Success = 200,
        [Display(Name = "خطا در سرور")]
        ServerError = 500,
        [Display(Name = "پارامترهای ارسالی معتبر نیست")]
        BadRequest = 400,
        [Display(Name = "یافت نشد")]
        NotFound = 404,
        [Display(Name = "اطلاعات تکراری است")]
        DuplicateInformation = 550,
        [Display(Name = "خطای تاریخ")]
        DateTimeError = 515,
        [Display(Name = "لیست خالی می باشد")]
        ListEmpty = 590,
        [Display(Name = "خطای کاربر")]
        UserMistake = 290
    }

}

