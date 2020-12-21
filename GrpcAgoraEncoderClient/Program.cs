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
            while (true)
            {
                Thread.Sleep(2000)
                Client.ReportEncoder(new EncoderMessage { ClientId = "R-Gallery-941135995144", Timestamp = 0 });
                Client.ReportEncodingStatus(new EncodingStatusMessage { ClientId = "R-Gallery-941135995144", Data = "{\"LocalNetworkSetting\":{\"NetworkConfigList\":[{\"InterfaceName\":\"em1\",\"InterfaceOrder\":1,\"NickName\":\"LAN1\",\"Bootproto\":2,\"IsUp\":true,\"IsPluginCable\":true,\"MacAddress\":\"98:90:96:BB:8B:33\",\"IPv4IP\":\"10.12.23.123\",\"MaskMapToIPv4\":\"255.255.192.0\",\"IPv4Gateway\":\"10.12.0.254\",\"DnsList\":[\"10.12.0.7\",\"8.8.4.4\"]}]},\"DualRList\":[{\"IsCheckStatus\":false,\"RPath\":\"/opt/tvu/R\",\"InstanceNum\":0,\"IdHex\":\"7BDFB6392B24D763\",\"ProductVersion\":\"7.6.0.7604\",\"ConfigFilePath\":\"/opt/tvu/R/Config.xml\",\"RConfig\":{\"RLocalIP\":\"0.0.0.0\",\"RLocalPort\":8088,\"WebPort\":8288,\"AuthWebUrl\":\"gridservice.tvupack.com\",\"AuthWebPort\":80,\"RPSIP\":\"10.12.22.100\",\"RPSPort\":3970,\"RExternalIP\":\"0.0.0.0\",\"RExternalPort\":8088,\"RUrl\":\"url=0:9999/xyz\",\"PlaybackUrl\":\"http://127.0.0.1:9999/xyz\",\"SwitcherPort\":7001,\"SwitcherIndex\":-1,\"FilterGrpcPort\":50051,\"WSUrlCommandCenterWebSocket\":\"ws://10.12.22.93/tvucc/websocket\",\"NginxLocalPort\":80,\"NginxRootDir\":\"/usr/share/nginx/html\",\"NginxExternalPort\":0,\"OutputFormat\":\"720P50\",\"FecTransportRpcServerPort\":6530,\"ExEncoderStartPort\":30000,\"LiveEncoderStartPort\":32000,\"ProducerStartPort\":33000,\"CategoryName\":\"Core.Macro\",\"ShortName\":\"ReceiverConfig0\"},\"SystemdServiceTVUR\":{\"ServiceName\":\"tvu.r\",\"ActiveState\":1,\"LoadState\":1,\"SubState\":8,\"IsRunning\":true}}]}" });
            }
            Console.ReadLine();
        }
    }
}
