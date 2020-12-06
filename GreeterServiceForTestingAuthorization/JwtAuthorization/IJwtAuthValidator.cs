using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization.JwtAuthorization
{
    public interface IJwtAuthValidator
    {
        JwtAuthToken GetAuthToken(Crendential crendential);

        bool ValidateAuthToken(JwtAuthToken jwtAuthToken);
    }
}
