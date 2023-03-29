using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public CountriesApiController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Country>> Get()
        {
            return await _context.CountryUw.Get();
        }

        [HttpPost]
        public async Task<IActionResult> Craate([FromForm] string CountryName) 
        {
            if (string.IsNullOrWhiteSpace(CountryName)) return BadRequest(ModelState);

            //کنترل تکراری نبودن
            var country = await _context.CountryUw.Get(c=>c.CountryName == CountryName);
            if (country.Count() > 0)
                return StatusCode(550);

            try
            {
                var _country = new Country()
                {
                    CountryName = CountryName,
                };
                await _context.CountryUw.Create(_country);
                await _context.SaveAsync();
                return Ok(_country);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        
        
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId(int Id)
        {
            var _country = await _context.CountryUw.GetById(Id);
            return _country == null? NotFound() : Ok(_country);

        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] Country datamodel)
        {
            if(string.IsNullOrWhiteSpace(datamodel.CountryName)) return BadRequest(ModelState);

            //تکراری نبودن
            var countries = await _context.CountryUw.Get(c => c.CountryName == datamodel.CountryName);
            if(countries.Count()>0)
                return StatusCode(550);
            try
            {
                var _country = await _context.CountryUw.GetById(datamodel.CountryId);
                if (_country == null) return NotFound();
                _country.CountryName = datamodel.CountryName;
                _context.CountryUw.Update(_country);
                await _context.SaveAsync();
                return Ok(_country);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }


        }

    }
}
