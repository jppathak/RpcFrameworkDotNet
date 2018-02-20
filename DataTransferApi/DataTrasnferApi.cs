using System.Threading.Tasks;

namespace DataTransferApi
{
    public interface IMessage
    {
        string Data { get; set; }
    }

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

    public static class Serializer
    {
        static public byte[] Serilize(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }

        public static string Deserilize(byte[] input)
        {
            string obj = System.Text.Encoding.UTF8.GetString(input);
            return obj;
        }
    }
    public class Response : IMessage
    {
        public string Data { get; set; }
    }
    public class Request: IMessage
    {
        public string Data { get; set; }
    }
}
