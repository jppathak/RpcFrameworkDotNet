using System;
using Grpc.Core;
using DataTransferApi;
using GrpcServiceContracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GrpcClient
{
    public class GrpcDemoClient : ClientBase<GrpcDemoClient>
    {

        public GrpcDemoClient(Channel channel) : base(channel)
        {
        }
        /// <summary>Protected constructor to allow creation of configured clients.</summary>
        /// <param name="configuration">The client configuration.</param>
        protected GrpcDemoClient(ClientBaseConfiguration configuration) : base(configuration)
        {
        }
        protected override GrpcDemoClient NewInstance(ClientBaseConfiguration configuration)
        {
            return new GrpcDemoClient(configuration);
        }

        public string SendRequestMessageString(string request)
        {
            var options = new CallOptions { };
            return CallInvoker.BlockingUnaryCall(ServiceContracts.ProcessRequestString, null, options, request);
        }
        public Response SendRequestMessage(Request request)
        {
            var options = new CallOptions {};
            return CallInvoker.BlockingUnaryCall(ServiceContracts.ProcessRequest, null, options, request);
        }

        public async Task SendRequestAndGetIncrementalMessageAsync(Request request)
        {
            using (var call = CallInvoker.AsyncServerStreamingCall(ServiceContracts.ProcessRequestViaServerUpdateReported, null, new CallOptions {}, request))
            {
                var responseStream = call.ResponseStream;
                while (await responseStream.MoveNext())
                {
                    var message = responseStream.Current;
                    Console.WriteLine("Server Reply with incremental update: " + message.Data);
                }
            }          
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Channel channel = new Channel("chqjpathak:9000", ChannelCredentials.Insecure);
            var client = new GrpcDemoClient(channel);

            var reply = client.SendRequestMessageString("Please listen to me as string");
            Console.WriteLine("Server Reply: " + reply);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            var reply2 = client.SendRequestMessage(new Request { Data = "Please listen to me" });
            Console.WriteLine("Server Reply: " + reply2.Data);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            client.SendRequestAndGetIncrementalMessageAsync(new Request { Data = "Please send me data" }).Wait();
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
