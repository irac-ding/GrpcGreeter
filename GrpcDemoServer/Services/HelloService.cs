using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcDemoServer.Services
{
    public class HelloService : Hello.HelloBase
    {
        ////一元调用
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Response:" + request.Name
            });
        }
        //客户端流
        public override async Task<HelloReply> SayHelloClientStream(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {

            var current = new HelloRequest();
            while (await requestStream.MoveNext())
            {
                current = requestStream.Current;
            }

            var task = new Task<HelloReply>(() =>
            {
                var reply = new HelloReply()
                {
                    Message = "Response:" + current.Name
                };
                return reply;
            });

            task.Start();

            var result = await task;

            return result;
        }
        //服务端流
        public override async Task SayHelloServerStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await responseStream.WriteAsync(new HelloReply() { Message = "Response:" + request.Name });

        }

        //双向流
        public override async Task SayHelloStream(IAsyncStreamReader<HelloRequest> requestStream,
            IServerStreamWriter<HelloReply> responseStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                await responseStream.WriteAsync(new HelloReply() { Message = "Response:" + requestStream.Current.Name });
            }
        }
    }
}
