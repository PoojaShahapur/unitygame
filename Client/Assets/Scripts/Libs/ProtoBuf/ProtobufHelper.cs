using ProtoBuf;
using System.IO;
using System.Text;

namespace SDK.Lib
{
    public class ProtobufHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T DeSerializeFString<T>(string content)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        /**
         * @brief 反序列化
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