namespace SDK.Lib
{
    public class UtilStr
    {
        static public void removeLastCR(ref string srcStr)
        {
            if(!string.IsNullOrEmpty(srcStr))
            {
                if(srcStr[srcStr.Length - 1] == Symbolic.CR)
                {
                    srcStr = srcStr.Substring(0, srcStr.Length - 1);
                }
            }
        }

        //static public void split(ref string str, params string[] param)
        // 仅支持一个符号分割
        static public string[] split(ref string str, char splitSymbol)
        {
            char[] split = new char[1];
            split[0] = splitSymbol;
            string[] strArr = null;

            if (!string.IsNullOrEmpty(str))
            {
                strArr = str.Split(split);
            }

            return strArr;
        }
    }
}