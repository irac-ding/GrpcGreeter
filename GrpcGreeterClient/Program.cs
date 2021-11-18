using System;
using System.Net.Http;
using System.Threading.Tasks;
using GrpcGreeter;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Google.Protobuf.WellKnownTypes;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:9000");
            var greeterClient = new Greeter.GreeterClient(channel);
            var greeterReply = await greeterClient.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            var managerClient = new Manager.ManagerClient(channel);
            var managerAddReply = await managerClient.AddAsync(new NewCustomer { FirstName = "irac", LastName = "test", Email = "irac@test.com" });
            Console.WriteLine("manager: " + JsonConvert.SerializeObject(managerAddReply));
            var managerDeleteReply = await managerClient.DeleteAsync(new DeleteCustomer {Id="test"}) ;
            Console.WriteLine("manager: " + JsonConvert.SerializeObject(managerDeleteReply));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}