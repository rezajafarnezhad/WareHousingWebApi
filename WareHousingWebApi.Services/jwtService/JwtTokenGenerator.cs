
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Services.jwtService.Interface;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WareHousingWebApi.Services.jwtService;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly SymmetricSecurityKey _key;
    private readonly string _Seckey;
    public JwtTokenGenerator(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        _Seckey = config["secKey"];

    }


    public async Task<string> CreateToken(Users user, IList<string> Roles)
    {
        var Claims = new List<Claim>();

        Claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        Claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
        Claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.BirthDayDate + ""));


        //add roles User to claim
        Claims.AddRange(Roles.Select(c => new Claim(ClaimTypes.Role, c)));


        //Hash SecurityKey
        var encryptorKey = Encoding.UTF8.GetBytes(_Seckey);

        var encryptorCred = new EncryptingCredentials(new SymmetricSecurityKey(encryptorKey),
            SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriber = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),

            // امضا
            SigningCredentials = creds,

            //تاریخ انقضا
            Expires = DateTime.Now.AddHours(8),
            // کننده توکن
            Issuer = "AnbarTo",

            //شنونده
            Audience = "AnbarTo",
            EncryptingCredentials = encryptorCred,
        };


        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriber);

        return tokenHandler.WriteToken(token);
    }


}


//public class mapappjsoninclass
//{
//    public void d(IServiceCollection services , IConfiguration config)
//    {
//        var option = new optionclass();

//        var section = config.GetSection("jwt");
//        section.Bind(option);
//        services.Configure<optionclass> config.GetSection("jwt"));
//    }
//}
