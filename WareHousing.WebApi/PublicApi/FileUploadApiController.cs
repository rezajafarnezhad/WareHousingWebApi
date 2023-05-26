using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> imagearray)
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

                return Ok("https://" + HttpContext.Request.Headers.Host + "//upload//productimage/" + filename);
            }
            catch (Exception)
            {

                return StatusCode(500);
                     
            }
        }
        [HttpPost("UserUpload")]
        public async Task<IActionResult> UserUploadFile(IEnumerable<IFormFile> imagearray)
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

                return Ok("https://" + HttpContext.Request.Headers.Host + "//upload//userimage/" + filename);
            }
            catch (Exception)
            {

                return StatusCode(500);

            }
        }

    }
}
