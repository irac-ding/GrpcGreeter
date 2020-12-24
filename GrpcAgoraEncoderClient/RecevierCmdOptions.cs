using CommandLine;

namespace GrpcAgoraEncoderClient
{
    public class RecevierCmdOptions
    {
        [Option('g', "Grpc_Port", Default = "", HelpText = "Grpc Port", Required = true)]
        public int GrpcPort { get; set; }

        [Option('c', "Client_Id", Default = "", HelpText = "Client Id", Required = true)]
        public string ClientId { get; set; }

        [Option('e', "throw_exception", Default = -1, HelpText = "throw exception", Required = false)]
        public int ExceptionType { get; set; }
    }
}
