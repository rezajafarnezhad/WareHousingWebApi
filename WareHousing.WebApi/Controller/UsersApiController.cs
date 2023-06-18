using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousing.WebApi.Tools;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.PublicTools;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<Users> _userManager;
        private readonly IMapper _mapper;
        public UsersApiController(IUnitOfWork unitOfWork, UserManager<Users> userManager, IMapper mapper)
        {
            _context = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _context.usersUw.Get();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId)
        {
            var user = await _context.usersUw.GetById(userId);
            return user == null ? NotFound() : Ok(user);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CrateUser model)
        {
            if (!ModelState.IsValid) { return BadRequest(model); }

            var user = await _context.usersUw.Get();
            if (user.Any(c => c.PhoneNumber == model.PhoneNumber || c.MelliCode == model.MelliCode))
                return StatusCode(550);

            try
            {

                model.BirthDayDate = model.BirthDayDate.ConvertShamsiToMiladi().ToString();
                var mUser = _mapper.Map(model,new Users());

                var result = await _userManager.CreateAsync(mUser, "123456");

                //دادن نقش
                if (result.Succeeded)
                {
                    if (mUser.UserType == 1)
                        await _userManager.AddToRoleAsync(mUser, "admin");
                    else
                        await _userManager.AddToRoleAsync(mUser, "user");

                    await _context.SaveAsync();
                    return Ok(mUser);
                }
                else
                {
                    return BadRequest(model);
                }

            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditUser model)
        {
            if (string.IsNullOrWhiteSpace(model.Id)) return BadRequest(ModelState);

            if (!ModelState.IsValid) { return BadRequest(); }

            try
            {
                var getUser = await _userManager.FindByIdAsync(model.Id);
                if (getUser != null)
                {
                    model.BirthDayDate = model.BirthDayDate.ConvertShamsiToMiladi().ToString();
                    var mUser = _mapper.Map(model, getUser);
                    var result = await _userManager.UpdateAsync(mUser);
                    if (result.Succeeded)
                        return Ok("200");
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return StatusCode(500);

        }



    }
}






