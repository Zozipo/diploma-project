using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Specification;

namespace Core.Services
{
    public class JwtService(IConfiguration configuration, 
                            UserManager<User> userManager,
                            IRepository<TablePermission> permissionsRepo) : IJwtService
    {
        public string CreateToken(IEnumerable<Claim> claims)
        {
            var jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpts.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOpts.Lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new (CustomClaimTypes.id, user.Id),
                new (CustomClaimTypes.userName, user.UserName),
                new (CustomClaimTypes.lastName, user.LastName),
                new (CustomClaimTypes.firstName, user.FirstName),
                new (CustomClaimTypes.sex, user.Sex),
                new (CustomClaimTypes.position, user.Position),
                new (CustomClaimTypes.email, user.Email),                
                new (CustomClaimTypes.dateCreated, user.DateCreated.ToString()),
                new (CustomClaimTypes.image, user.Image),
            };

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claims.Add(new Claim(CustomClaimTypes.phoneNumber, user.PhoneNumber));
            }

            var roles = userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(CustomClaimTypes.roles, role)));

            var permissions = permissionsRepo.GetAllBySpec(new Permissions.ByUserId(user.Id)).Result;
            claims.AddRange(permissions.Select(permission => new Claim(CustomClaimTypes.tablePermissions, permission.ModeratingTable.TableName.ToString())));

            return claims;
        }
    }

    public static class CustomClaimTypes
    {
        public const string id = "id";
        public const string userName = "userName";
        public const string sex = "sex";
        public const string position = "position";
        public const string lastName = "lastName";
        public const string firstName = "firstName";
        public const string email = "email";
        public const string roles = "roles";
        public const string tablePermissions = "tablePermissions";
        public const string phoneNumber = "phoneNumber";
        public const string dateCreated = "dateCreated";
        public const string image = "image";
    }
}
