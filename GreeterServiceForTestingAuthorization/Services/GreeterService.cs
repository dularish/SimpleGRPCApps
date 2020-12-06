using GreeterServiceForTestingAuthorization.JwtAuthorization;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IJwtAuthValidator _authValidator;

        public GreeterService(ILogger<GreeterService> logger, IJwtAuthValidator jwtAuthValidator)
        {
            _logger = logger;
            _authValidator = jwtAuthValidator;
        }

        public override Task<HelloReply> SayEasyHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Easy hello " + request.Name
            });
        }

        [Authorize]
        public override Task<HelloReply> SayAuthorizedHello(HelloRequest request, ServerCallContext context)
        {
            Microsoft.AspNetCore.Http.HttpContext httpContext = context.GetHttpContext();
            return Task.FromResult(new HelloReply
            {
                Message = $"Authorized hello {request.Name} {httpContext.User.Identity.Name}"
            });
        }

        [Authorize(Roles ="Admin")]
        public override Task<HelloReply> SayAdminAuthorizedHello(HelloRequest request, ServerCallContext context)
        {
            Microsoft.AspNetCore.Http.HttpContext httpContext = context.GetHttpContext();
            return Task.FromResult(new HelloReply
            {
                Message = $"Admin Authorized hello {request.Name} {httpContext.User.Identity.Name}"
            });
        }

        public override Task<TokenResponse> GetAuthToken(TokenRequest request, ServerCallContext context)
        {
            TokenResponse tokenResponse = new TokenResponse()
            {
                Result = false
            };

            var authToken = _authValidator.GetAuthToken(new Crendential() { UserId = request.Username, Password = request.Password });
            if (authToken.IsTokenGenerationSuccess)
            {
                tokenResponse.Expiration = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(authToken.Expiration.ToUniversalTime());
                tokenResponse.Result = true;
                tokenResponse.TokenString = authToken.TokenString;
            }

            return Task.FromResult(tokenResponse);
        }
    }
}
