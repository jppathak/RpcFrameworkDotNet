using System;
using Grpc.Core;
using System.Threading.Tasks;
using DataTransferApi;
using GrpcServiceContracts;

namespace GrpcConsoleApp
{
    class DemoServiceImpl
    {
        public static ServerServiceDefinition BindService(DemoServiceImpl serviceImpl)
        {
            return ServerServiceDefinition.CreateBuilder()
                .AddMethod(ServiceContracts.ProcessRequestString, serviceImpl.ProcessRequestString)
                .AddMethod(ServiceContracts.ProcessRequest, serviceImpl.ProcessRequest)
                .AddMethod(ServiceContracts.ProcessRequestViaServerUpdateReported, serviceImpl.ProcessRequestServerStreamingAsync)
                .Build();
        }

        private Task<string> ProcessRequestString(string request, ServerCallContext context)
        {
            return Task.FromResult( String.Format("Server response for: " + request + "\nRequest Processed") );
        }

        public Task<Response> ProcessRequest(Request request, ServerCallContext context)
        {
            return Task.FromResult(new Response { Data = String.Format("Server response for: " + request.Data + "\nRequest Processed") });
        }

        public async Task ProcessRequestServerStreamingAsync(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            for (int i = 0; i <= 100; i += 5)
            {
                await Task.Delay(200);
                await responseStream.WriteAsync(new Response { Data = string.Format(" {0}% of {1}%", i, 100) });
            }
            await responseStream.WriteAsync(new Response { Data = "Server response completed" });
        }
    }


    class Program
    {
        const int Port = 50051;
        static void Main(string[] args)
        {
            var server = new Server
            {
                Services = { DemoServiceImpl.BindService(new DemoServiceImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Grpc server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
