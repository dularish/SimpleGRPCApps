using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization.JwtAuthorization
{
    public class JwtAuthToken
    {
        public bool IsTokenGenerationSuccess { get; init; }
        public DateTime Expiration { get; init; }

        public string TokenString { get; init; }
    }
}
