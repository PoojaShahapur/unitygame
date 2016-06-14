using SDK.Lib;

namespace Game.AutoUpdate
{
    public class AutoUpdateSys : IAutoUpdate
    {
        public void Start()
        {
            initGVar();
            startAutoUpdate();
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.m_instance.m_cbUIEvent = new AutoUpdateUIEventCB();
        }

        protected void startAutoUpdate()
        {
            Ctx.m_instance.mAutoUpdateSys.m_onUpdateEndDisp = onAutoUpdateEnd;
            Ctx.m_instance.mAutoUpdateSys.startUpdate();
            //onAutoUpdateEnd();
        }

        // 调用这个函数，说明文件已经更新到本地，版本文件也加载完成
        public void onAutoUpdateEnd()
        {
            loadPakCfg();
        }

        protected void loadPakCfg()
        {
            Ctx.m_instance.m_pPakSys.m_pakCfgLoadDisp = onPakSysCfgEnd;
            Ctx.m_instance.m_pPakSys.loadFile();
        }

        // 调用这个函数，说明打包信息加载完成
        protected void onPakSysCfgEnd()
        {
            Ctx.m_instance.m_moduleSys.loadModule(ModuleID.LOGINMN);
        }

        // 卸载模块
        public void unload()
        {
            
        }
    }
}