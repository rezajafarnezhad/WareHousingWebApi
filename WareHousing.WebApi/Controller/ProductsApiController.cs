using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        public ProductsApiController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Products>> Get()
        {
            return await _context.productsUw.Get(null, "Country,Supplier");
        }

        [HttpPost]
        public async Task<IActionResult> Craate([FromForm] ProductCreatModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            //کنترل تکراری نبودن
            var product = await _context.productsUw.Get(c => c.ProductName == model.ProductName);
            if (product.Count() > 0)
                return StatusCode(550);

            try
            {
                var _product = new Products()
                {
                    ProductName = model.ProductName,
                    ProductWeight = model.ProductWeight,
                    ProductDescription = model.ProductDescription,
                    ProductImage = model.ProductImage,
                    CountInPacking = model.CountInPacking,
                    IsRefregerator = model.IsRefregerator,
                    CountryId = model.CountryId,
                    SupplierId = model.SupplierId,
                    PackingType = model.PackingType,

                };
                await _context.productsUw.Create(_product);
                await _context.SaveAsync();
                return Ok(_product);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetbyId([FromRoute] int Id)
        {
            var _product = await _context.productsUw.GetById(Id);
            return _product == null ? NotFound() : Ok(_product);

        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductEditModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            //کنترل تکراری نبودن
            var product = await _context.productsUw.Get(c => c.ProductName == model.ProductName && c.ProductId != model.ProductId);


            if (product.Count() > 0)
                return StatusCode(550);

            try
            {
                var _product = await _context.productsUw.GetById(model.ProductId);
                if (_product == null) return NotFound();

                _product.ProductName = model.ProductName;
                _product.ProductDescription = model.ProductDescription;
                _product.ProductWeight = model.ProductWeight;
                _product.CountInPacking = model.CountInPacking;
                _product.CountryId = model.CountryId;
                _product.SupplierId = model.SupplierId;
                _product.PackingType = model.PackingType;
                _product.IsRefregerator = model.IsRefregerator;
                _product.ProductImage = model.ProductImage;
                _context.productsUw.Update(_product);
                await _context.SaveAsync();
                return Ok(_product);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }

        }

    }
    //kk

}

