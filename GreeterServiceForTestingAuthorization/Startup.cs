using GreeterServiceForTestingAuthorization.JwtAuthorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreeterServiceForTestingAuthorization
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IJwtAuthValidator, MinimalTestJwtAuthValidator>();
            services.AddGrpc();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Authentication scheme must needed either here or in authorize attribute provided in service
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidIssuer = "localhost",//To make sure the tokens are generated from valid server
                        ValidateAudience = false,//Audience information avoided for this testing
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthTestProj:SecretKey"])),
                        ClockSkew = TimeSpan.Zero,//Needed this to test for short token lifetimes, as by default a clockskew of about ~5 minutes allowed between different clocks
                    };
                });//Must need for jwt authorization
            services.AddAuthorization();//Must need for jwt authorization
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();//Don't know what is exactly it's purpose
            app.UseRouting();
            app.UseAuthentication();//This should actually enable reading claims from the token to HttpContext.User.Identity, 
                                            //but able to do it without writing this statement
                                            //Role based authentication does not work without this line,
                                            //This line should be used before app.UseAuthorization
            app.UseAuthorization();//Must need for jwt authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
