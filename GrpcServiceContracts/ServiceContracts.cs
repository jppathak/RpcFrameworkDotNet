using Grpc.Core;
using DataTransferApi;

namespace GrpcServiceContracts
{
    public static class ServiceContracts
    {
        public static readonly Method<string, string> ProcessRequestString = new Method<string, string>(
               type: MethodType.Unary,
               serviceName: "SimpleGrpcService",
               name: "ProcessStringRequest",
               requestMarshaller: Marshallers.Create(
               serializer: Serializer.Serilize,
               deserializer: Serializer.Deserilize),
               responseMarshaller: Marshallers.Create(
               serializer: Serializer.Serilize,
               deserializer: Serializer.Deserilize));

        public static readonly Method<Request, Response> ProcessRequest = new Method<Request, Response>(
               type: MethodType.Unary,
               serviceName: "SimpleGrpcService",
               name: "ProcessRequest",
               requestMarshaller: Marshallers.Create(
               serializer: Serializer<Request>.Serilize,
               deserializer: Serializer<Request>.Deserilize),
               responseMarshaller: Marshallers.Create(
               serializer: Serializer<Response>.Serilize,
               deserializer: Serializer<Response>.Deserilize));

        public static readonly Method<Request, Response> ProcessRequestViaServerUpdateReported =
                        new Method<Request, Response>(
                            type: MethodType.ServerStreaming,
                            serviceName: "SimpleGrpcService",
                            name: "ProcessRequestViaServerUpdateReported",
                            requestMarshaller: Marshallers.Create(
                                serializer: Serializer<Request>.Serilize,
                                deserializer: Serializer<Request>.Deserilize),
                            responseMarshaller: Marshallers.Create(
                                serializer: Serializer<Response>.Serilize,
                                deserializer: Serializer<Response>.Deserilize));
    }
}
