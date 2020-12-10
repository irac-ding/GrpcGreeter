﻿using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using TVU.AgoraEncoder.Contract;

namespace GrpcAgoraEncoderClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/ HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress("http://127.0.0.1:56790");
            var Client = new AgoraEncoderMaster.AgoraEncoderMasterClient(channel);

            for (int i = 0; i <= 10; i++)
            {
                GenericResult genericResult1 = Client.ReportEncoder(new EncoderMessage { ClientId = "irac", Timestamp = 1234 });
                GenericResult genericResult2 = Client.ReportEncodingStatus(new EncodingStatusMessage { ClientId = "irac", Data = i.ToString() + "test" });
            }
            AsyncServerStreamingCall<ServerMessage> serverMessageReply = Client.Register(new ClientInfo { ClientId = "irac" });
            while (await serverMessageReply.ResponseStream.MoveNext())
            {
                Console.WriteLine(serverMessageReply.ResponseStream.Current.Data);
            }
        }
    }
}
