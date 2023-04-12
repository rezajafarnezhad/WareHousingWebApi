using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public SupplierApiController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Supplier>> Get()
        {
            return await _context.SupplierUw.Get();
        }

        [HttpPost]
        public async Task<IActionResult> Craate([FromForm] SupplierModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            //کنترل تکراری نبودن
            var supplier = await _context.SupplierUw.Get(c => c.SupplierName == model.SupplierName || c.SupplierTel == model.SupplierTel);
            if (supplier.Count() > 0)
                return StatusCode(550);

            try
            {
                var _supplier = new Supplier()
                {
                   SupplierName = model.SupplierName,
                   SupplierTel = model.SupplierTel,
                   SupplierDescription = model.SupplierDescription,
                   SupplierSite = model.SupplierSite,
                };
                await _context.SupplierUw.Create(_supplier);
                await _context.SaveAsync();
                return Ok(_supplier);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }


        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int Id)
        {
            var _supplier = await _context.SupplierUw.GetById(Id);
            return _supplier == null ? NotFound() : Ok(_supplier);

        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] SupplierEditModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            //کنترل تکراری نبودن
            var supplier = await _context.SupplierUw.Get(c => c.SupplierName == model.SupplierName  && c.SupplierId != model.SupplierId);
          

            if (supplier.Count() > 0)
                return StatusCode(550);

            try
            {
                var _supplier = await _context.SupplierUw.GetById(model.SupplierId);
                if (_supplier == null) return NotFound();

                _supplier.SupplierName = model.SupplierName.Trim();
                _supplier.SupplierTel = model.SupplierTel.Trim();
                _supplier.SupplierDescription = model.SupplierDescription.Trim();
                _supplier.SupplierSite = model.SupplierSite.Trim();
                
                _context.SupplierUw.Update(_supplier);
                await _context.SaveAsync();
                return Ok(_supplier);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }


        }

        [HttpGet("GetSupplierForDropDown")]
        public async Task<IActionResult> GetSupplierForDropDown() 
        {

            var data = await _context.SupplierUw.GetEn.ToDictionaryAsync(c => c.SupplierId , c=>c.SupplierName);
            return Ok(JsonConvert.SerializeObject(data));

        }

    }

}
