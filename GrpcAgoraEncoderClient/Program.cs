using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using TVU.AgoraEncoder.Contract;
using static TVU.AgoraEncoder.Contract.AgoraEncoderMaster;

namespace GrpcAgoraEncoderClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/ HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://127.0.0.1:56790");
            AgoraEncoderMasterClient Client = new AgoraEncoderMaster.AgoraEncoderMasterClient(channel);

            Task.Run(async () =>
            {
                AsyncServerStreamingCall<ServerMessage> serverMessageReply = Client.Register(new ClientInfo { ClientId = "R-Gallery-941135995144" });
                while (await serverMessageReply.ResponseStream.MoveNext())
                {
                    Console.WriteLine(serverMessageReply.ResponseStream.Current.Data);
                }
            });


            for (int i = 0; i <= 10; i++)
            {
                GenericResult genericResult1 = Client.ReportEncoder(new EncoderMessage { ClientId = "R-Gallery-941135995144", Timestamp = 1234 });
                GenericResult genericResult2 = Client.ReportEncodingStatus(new EncodingStatusMessage { ClientId = "R-Gallery-941135995144", Data = i.ToString() + "test" });
            }
          
        }
    }
}
