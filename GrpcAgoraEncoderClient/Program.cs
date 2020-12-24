using System;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using NLog;

namespace GrpcAgoraEncoderClient
{
    //usage：./GrpcAgoraEncoderClient --Grpc_Port 54321 --Client_Id R-Gallery-941135995144
    class Program
    {
        #region Log
        private static Logger logger { get; } = LogManager.GetCurrentClassLogger();
        #endregion
        static async Task Main(string[] args)
        {
            RecevierCmdOptions option = new RecevierCmdOptions();
            ParserResult<RecevierCmdOptions> result = Parser.Default.ParseArguments<RecevierCmdOptions>(args).WithParsed(options =>
            {
                option.GrpcPort = options.GrpcPort;
                option.ClientId = options.ClientId;
            }).WithNotParsed(options =>
            {
                logger.Fatal("Application_Startup() parser failed.");
                Environment.Exit(-2);
            });
            logger.Info($"Application_Startup() with para: {JsonConvert.SerializeObject(option)}");
            AgoraEncoderClient agoraEncoderClient = new AgoraEncoderClient(option.GrpcPort, option.ClientId);
            agoraEncoderClient.Send();
            Console.ReadLine();
        }
    }
}
