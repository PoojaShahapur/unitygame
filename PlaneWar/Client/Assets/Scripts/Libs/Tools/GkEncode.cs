using System.Text;

namespace SDK.Lib
{
    public enum GkEncode
    {
        /**
        static public Encoding UTF8 = Encoding.UTF8;
        //static public Encoding GB2312 = Encoding.GetEncoding(936);         // GB2312 这个解码器在 mono 中是没有的，不能使用
        static public Encoding GB2312 = Encoding.UTF8;         // GB2312
        static public Encoding Unicode = Encoding.Unicode;
        static public Encoding Default = Encoding.Default;
        */
        eUTF8 = 0,
        eGB2312 = 1,
        eUnicode = 2,
        eDefault = 3,
    }
}