using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Services.jwtService.Interface;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class AccountApiController : ControllerBase
{

    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _context;

    public AccountApiController(UserManager<Users> userManager, SignInManager<Users> signInManager, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginModel model)
    {
        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        try
        {

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) { return Unauthorized("Incorrect username or password."); }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) { return Unauthorized("Incorrect username or password."); }

            var userRoles = await _userManager.GetRolesAsync(user);

            //کنترل سال مالی
            var fiscalYear = await _context.fiscalYearUw.GetById(model.FiscalYearId);
            byte fiscalStatus = 10;
            if (fiscalYear.FiscalFlag == true && fiscalYear.EndDate.Date >= DateTime.Now.Date)
            {
                //
                fiscalStatus = 0;
            }
            else if (fiscalYear.FiscalFlag == true && fiscalYear.EndDate.Date < DateTime.Now.Date)
            {
                //سال مالی بازه ولی تاریخ روز از ناریخ پابان سال مالی عبور کزده
                fiscalStatus = 1;
            }
            else if (fiscalYear.FiscalFlag == false)
            {
                //سال مالی بسته است
                fiscalStatus = 2;
            }



            var UserTokenResult = new UserJwtToken()
            {
                UserId = user.Id,
                UserName = model.UserName,
                Roles = userRoles,
                Token = await _jwtTokenGenerator.CreateToken(user, userRoles),
                fiscalStatus = fiscalStatus,
            };

            return Ok(UserTokenResult);

        }
        catch (Exception)
        {

            return StatusCode(500);
        }
    }

}