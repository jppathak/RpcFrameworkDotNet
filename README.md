# RpcFrameworkDotNet
This repo compliments the other repo that I created earlier in C++ using grpc as an rpc framework referenced below:
https://github.com/jppathak/RpcFramework

This is extending the work to .Net. .NET API for grpc is found to be more clean than C++ version and hence defining a custom 
serilization/contracts is much easier(as below). This does not require any protobuf binaries. See the packages folder for dependencies.

In production code, we use "Protobuf.Net"(another great repo: https://github.com/mgravell/protobuf-net) as a way to serialize data, which is 
effectively using "Protobuf", however it does not require custom stub generation. 


    public static class Serializer<T> where T : IMessage, new()
    {
        static public byte[] Serilize(T obj) 
        {
            var data = (obj as IMessage).Data;
            return System.Text.Encoding.UTF8.GetBytes(data);
        }
        public static T Deserilize(byte[] input)
        {
            string temp = System.Text.Encoding.UTF8.GetString(input);
            var obj = new T();
            (obj as IMessage).Data = temp;
            return obj;
        }
    }
   
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
    
