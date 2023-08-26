using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.PublicApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadApiController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadApiController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ApiResponse> UploadFile(IEnumerable<IFormFile> imagearray)
        {
            try
            {
                var upload = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\productimage\\");
                var filename = "";

                foreach (var file in imagearray)
                {
                    filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                }

                return new ApiResponse()
                {
                    flag = true,
                    StatusCode = ApiStatusCode.Success,
                    Message = "https://" + HttpContext.Request.Headers.Host + "//upload//productimage/" + filename,
                };
            }
            catch (Exception)
            {

                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
                };

            }
        }
        [HttpPost("UserUpload")]
        public async Task<ApiResponse> UserUploadFile(IEnumerable<IFormFile> imagearray)
        {
            try
            {
                var upload = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\userimage\\");
                var filename = "";

                foreach (var file in imagearray)
                {
                    filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                }

                return new ApiResponse()
                {
                    flag = true,
                    StatusCode = ApiStatusCode.Success,
                    Message = "https://" + HttpContext.Request.Headers.Host + "//upload//userimage/" + filename
                };
            }
            catch (Exception)
            {

                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
                };


            }
        }

    }
}
