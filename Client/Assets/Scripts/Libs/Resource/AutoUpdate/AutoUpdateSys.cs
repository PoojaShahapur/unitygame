using SDK.Common;
namespace SDK.Lib
{
    /**
     * @brief 自动更新系统
     */
    public class AutoUpdateSys
    {
        public void loadMiniVersion()
        {
            Ctx.m_instance.m_versionSys.m_miniLoadResultDisp = miniVerLoadResult;
            Ctx.m_instance.m_versionSys.m_LoadResultDisp = verLoadResult;
            Ctx.m_instance.m_versionSys.loadMiniVerFile();
        }

        public void miniVerLoadResult(bool needUpdate)
        {
            Ctx.m_instance.m_versionSys.loadVerFile();
        }

        public void verLoadResult()
        {
            if(Ctx.m_instance.m_versionSys.m_needUpdateVer) // 如果需要更新
            {
                // 开始正式加载文件
                loadUpdateFile();
            }
        }

        public void loadUpdateFile()
        {

        }
    }
}