using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousing.WebApi.Tools;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.Services.Interface;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<Users> _userManager;
        public UsersApiController(IUnitOfWork unitOfWork, UserManager<Users> userManager)
        {
            _context = unitOfWork;
            _userManager = userManager;
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
                var _user = new Users()
                {
                    Family = model.Family,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    UserType = 1,
                    FirstName = model.FirstName,
                    BirthDayDate = model.BirthDayDate.ConvertShamsiToMiladi(),
                    MelliCode = model.MelliCode,
                    PersonalCode = model.PersonalCode,
                    UserImage = model.UserImage,
                };

                var result = await _userManager.CreateAsync(_user, "123456");

                //دادن نقش
                if (result.Succeeded)
                {
                    if (_user.UserType == 1)
                        await _userManager.AddToRoleAsync(_user, "admin");
                    else
                        await _userManager.AddToRoleAsync(_user, "user");

                    await _context.SaveAsync();
                    return Ok(_user);
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
            if (!ModelState.IsValid) { return BadRequest(); }

            return default;

        }



    }
}
