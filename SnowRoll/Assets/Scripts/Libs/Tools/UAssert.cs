using System;

namespace SDK.Lib
{
    public class UAssert
    {
        public static void DebugAssert(bool condation)
        {
            if(!condation)
            {
                throw new Exception("DebugAssert Error");
            }
        }
    }
}