using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GreeterServiceForTestingTLSCertificates
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(kestralOptions =>
                    {
                        IConfiguration config = kestralOptions.ApplicationServices.GetService<IConfiguration>();
                        var cert = new X509Certificate2(config["TLSCertificates:CertFileName"], config["TLSCertificates:CertPassword"]);
                        kestralOptions.ConfigureHttpsDefaults(configureOptions =>
                        {
                            configureOptions.ClientCertificateMode = 
                                Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
                                //Configures HTTP/2 handshake params. Client certificate is not required in all kinds of applications, think of a bank website-browser interaction.

                            configureOptions.CheckCertificateRevocation = false;
                            configureOptions.ServerCertificate = cert;
                        });
                    });
                });
    }
}
