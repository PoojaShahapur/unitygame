using ProtoBuf;
using System.IO;
using System.Text;

namespace SDK.Lib
{
    /**
     * @brief Protobuf 辅助函数
     */
    public class ProtobufUtil
    {
        /**
         * @brief 序列化成字符串
         */
        public static string SerializeTString<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /**
         * @brief 序列化成字节数组
         */
        public static byte[] SerializeTBytes<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray();
            }
        }

        /**
         * @brief 从字符串反序列化
         */
        public static T DeSerializeFString<T>(string content)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        /**
         * @brief 从字节数组反序列化
         */
        public static T DeSerializeFBytes<T>(byte[] content)
        {
            using (MemoryStream ms = new MemoryStream(content))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        /**
         * @brief 获取序列化后占用的字节大小
         */
        public static int getSerializeBytesLength<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return ms.ToArray().Length;
            }
        }
    }
}