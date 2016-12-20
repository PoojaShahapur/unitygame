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
    }
}