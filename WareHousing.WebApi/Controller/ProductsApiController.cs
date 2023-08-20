using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public ProductsApiController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var product = await _context.productsUw.Get();
            if (product.Any(c => c.ProductName == model.ProductName))
                return StatusCode(550);

            try
            {

                var mProduct = _mapper.Map(model,new Products());
                await _context.productsUw.Create(mProduct);
                await _context.SaveAsync();
                return Ok(mProduct);
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
            var product = await _context.productsUw.GetEn.Where(c => c.ProductName == model.ProductName || c.ProductCode == model.ProductCode).Where(c=> c.ProductId != model.ProductId).ToListAsync();


            if (product.Count() > 0)
                return StatusCode(550);

            try
            {
                var _product = await _context.productsUw.GetById(model.ProductId);
                if (_product == null) return NotFound();


                var mProduct = _mapper.Map(model,_product);
                _context.productsUw.Update(mProduct);
                await _context.SaveAsync();
                return Ok(mProduct);
            }
            catch (Exception)
            {
                return StatusCode(500);

            }

        }

    }
    //kk

}

