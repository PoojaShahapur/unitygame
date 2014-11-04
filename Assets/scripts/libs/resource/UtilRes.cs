using SDK.Common;
namespace SDK.Lib
{
    // 资源工具类
    public class UtilRes
    {
        static public ResPathType ConvPath3Type(string path)
        {
            if(path.IndexOf("UI/Code") != -1)      // 如果含有 UI/Code 
            {
                return ResPathType.ePathCodePath;
            }
            else if (path.IndexOf("UI/Component") != -1) // 如果含有 UI/Component
            {
                return ResPathType.ePathComUI;
            }

            return ResPathType.ePathComUI;
        }
    }
}