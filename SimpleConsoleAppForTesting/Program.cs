using Grpc.Net.Client;
using SimpleGRPCService;
using System;

namespace SimpleConsoleAppForTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            string port = "5001";
            var channelTarget = $"https://{host}:{port}";
            var channel = GrpcChannel.ForAddress(channelTarget);
            
            var client2 = new Greeter.GreeterClient(channel);
            var response = client2.SayHello(new HelloRequest() { Name = "Dularish" });
            Console.ReadKey();
        }
    }
}
