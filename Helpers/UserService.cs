using Sistema_Veterinaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Sistema_Veterinaria.Encryption;

namespace Sistema_Veterinaria.Helpers
{
    public class UserService : IUserService
    {

        private List<Usuarios> _users = new List<Usuarios>();

        private readonly JWTSettings _jwtSettings;
        
        public UserService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _users = GetUsuarios();
        }

        private List<Usuarios> GetUsuarios()
        {
            var context = new veterinariaContext();
            var user_list = (from us in context.Usuarios
                             select us).ToList<Usuarios>();
            return user_list;
        }

        public Usuarios Authenticate(string username, string password)
        {

            var user = _users.SingleOrDefault(x => x.UserName == username && x.Password == Encrypt.getSHA256(password));
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim (ClaimTypes.Name, user.IdUser.ToString ())
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;
            return user;
        }

        public IEnumerable<Usuarios> GetAll()
        {
            return _users.Select(x => {
                x.Password = null;
                return x;
            });

        }
    }

}
