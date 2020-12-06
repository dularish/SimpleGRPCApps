using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization.JwtAuthorization
{
    public class UserAccount
    {
        public string UserId { get; init; }
        public string Password { get; init; }
        public string Name { get; init; }
        public string Role { get; init; }

    }
}
