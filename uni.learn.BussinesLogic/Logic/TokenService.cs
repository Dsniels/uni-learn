using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using uni.learn.core.Entity;
using uni.learn.core.Interfaces;

namespace uni.learn.BussinesLogic.Logic;

public class TokenService : ITokenService
{

    private readonly SymmetricSecurityKey _key;
    private readonly IConfiguration _config;

    public TokenService(SymmetricSecurityKey key,IConfiguration config){
        _config = config;
        _key = key;
    }


    public string CreateToken(Usuario usuario, IList<string> roles)
    {
        var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
            new Claim(JwtRegisteredClaimNames.FamilyName, usuario.ApellidoPaterno),
            new Claim("Matricula", usuario.Matricula.ToString())
        };

        if(roles != null && roles.Count > 0){
            foreach(var role in roles){
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }


        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

        var TokenConfiguration = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(30),
            SigningCredentials = credentials,
            Issuer = _config["Token:Issuer"]
        };


        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(TokenConfiguration);
        return tokenHandler.WriteToken(token);


    }
}
