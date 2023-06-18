using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WareHousing.WebApi.Tools.Interface;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;

namespace WareHousing.WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class AccountApiController : ControllerBase
{

    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AccountApiController(UserManager<Users> userManager, SignInManager<Users> signInManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
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

            var UserTokenResult = new UserJwtToken()
            {
                UserId = user.Id,
                UserName = model.UserName,
                Roles = userRoles,
                Token = await _jwtTokenGenerator.CreateToken(user, userRoles),
            };

            return Ok(UserTokenResult);

        }
        catch (Exception)
        {

            return StatusCode(500);
        }
    }

}