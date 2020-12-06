using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization.JwtAuthorization
{
    public class MinimalTestJwtAuthValidator : IJwtAuthValidator
    {
        private List<UserAccount> _allowedUsers = new List<UserAccount>()
        {
            new UserAccount()
            {
                UserId = "testUserId",
                Password = "testPassword",
                Name = "Test User",
                Role = "Employee"
            },
            new UserAccount()
            {
                UserId = "testAdminUserId",
                Password = "testAdminPassword",
                Name = "Test Admin User",
                Role = "Admin"
            }
        };
        private IConfiguration _configuration;

        public MinimalTestJwtAuthValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtAuthToken GetAuthToken(Crendential crendential)
        {
            if (!IsCredentialValidToGetAuthToken(crendential, out UserAccount userAccount))
            {
                return new JwtAuthToken() { IsTokenGenerationSuccess = false };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyInBytes = Encoding.UTF8.GetBytes(_configuration["AuthTestProj:SecretKey"]);
            DateTime expirationTime = DateTime.Now.AddSeconds(30);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "localhost",
                Expires = expirationTime,
                Claims = GetClaimsFor(userAccount),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyInBytes), SecurityAlgorithms.HmacSha256Signature),
                //Subject = new System.Security.Claims.ClaimsIdentity(new[] { new Claim("id", crendential.UserId)}),//Yet to understand about claims and why it is set to subject
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); //How is it different from token tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return new JwtAuthToken() { TokenString = tokenHandler.WriteToken(token), Expiration = expirationTime, IsTokenGenerationSuccess = true };
        }

        private IDictionary<string, object> GetClaimsFor(UserAccount userAccount)
        {
            var dict = new Dictionary<string, object>()
            {
                { ClaimTypes.Name, userAccount.Name },
                { ClaimTypes.NameIdentifier, userAccount.UserId},
                { ClaimTypes.Role, userAccount.Role }
            };
            return dict;
        }

        private bool IsCredentialValidToGetAuthToken(Crendential crendential, out UserAccount userAccount)
        {
            var matchedUser = _allowedUsers.FirstOrDefault(s => s.UserId == crendential.UserId);
            userAccount = matchedUser;
            return matchedUser != null && matchedUser.Password == crendential.Password;
        }

        public bool ValidateAuthToken(JwtAuthToken jwtAuthToken)
        {
            //validating only the expiration time
            return jwtAuthToken == null ? false : jwtAuthToken.Expiration > DateTime.Now;
        }
    }
}
