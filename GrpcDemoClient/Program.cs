using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcDemoServer;

namespace GrpcDemoClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpClientHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress("http://10.12.23.123:5002", new GrpcChannelOptions { HttpClient = httpClient });
            var helloClient = new Hello.HelloClient(channel);
            while (true)
            {
                //一元调用(同步方法）
                var reply = helloClient.SayHello(new HelloRequest { Name = "一元同步调用" });
                Console.WriteLine($"{reply.Message}");

                //一元调用(异步方法）
                var reply2 = helloClient.SayHelloAsync(new HelloRequest { Name = "一元异步调用" }).GetAwaiter().GetResult();
                Console.WriteLine($"{reply2.Message}");

                //服务端流
                var reply3 = helloClient.SayHelloServerStream(new HelloRequest { Name = "服务端流" });
                while (await reply3.ResponseStream.MoveNext())
                {
                    Console.WriteLine(reply3.ResponseStream.Current.Message);
                }

                //客户端流
                using (var call = helloClient.SayHelloClientStream())
                {
                    await call.RequestStream.WriteAsync(new HelloRequest { Name = "客户端流" + "客户端流".ToString() });
                    await call.RequestStream.CompleteAsync();
                    var reply4 = await call;
                    Console.WriteLine($"{reply4.Message}");
                }

                //双向流
                using (var call = helloClient.SayHelloStream())
                {
                    Console.WriteLine("Starting background task to receive messages");
                    var readTask = Task.Run(async () =>
                    {
                        await foreach (var response in call.ResponseStream.ReadAllAsync())
                        {
                            Console.WriteLine(response.Message);
                        }
                    });

                    for (var i = 0; i < 3; i++)
                    {
                        await call.RequestStream.WriteAsync(new HelloRequest { Name = "双向流" + i.ToString() });
                    }

                    await call.RequestStream.CompleteAsync();
                    await readTask;
                }
            }
            Console.ReadKey();
        }
    }
}
