using SDK.Common;
namespace SDK.Lib
{
    /**
     * @brief 打包系统
     */
    public class PakSys
    {
        // 获取当前资源所在的包文件名字
        public string getCurResPakPathByResPath(string resPath)
        {
            Ctx.m_instance.m_shareData.m_resInPakPath = resPath;
            return Ctx.m_instance.m_shareData.m_resInPakPath;
        }
    }
}