using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WareHousing.WebApi.Tools.Interface;
using WareHousingWebApi.Data.Entities;

namespace WareHousing.WebApi.Tools;

public class JwtTokenGenerator : IJwtTokenGenerator
{

    private readonly UserManager<Users> _userManager;

    private readonly SymmetricSecurityKey _key;
    public JwtTokenGenerator(UserManager<Users> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

    }


    public async Task<string> CreateToken(Users user,IList<string> Roles)
    {
        var Claims = new List<Claim>();

        Claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        Claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
        Claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.BirthDayDate + ""));


        //add roles User to claim
        Claims.AddRange(Roles.Select(c => new Claim(ClaimTypes.Role, c)));


        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriber = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            SigningCredentials = creds,
            Expires = DateTime.Now.AddDays(1),
        };


        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriber);

        return tokenHandler.WriteToken(token);
    }


}
