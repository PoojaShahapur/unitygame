using SDK.Lib;

namespace Game.AutoUpdate
{
    public class AutoUpdateSys : IAutoUpdate
    {
        public void Start()
        {
            this.initGVar();
            this.startAutoUpdate();
        }

        public void initGVar()
        {
            // 游戏逻辑处理
            Ctx.mInstance.mCbUIEvent = new AutoUpdateUIEventCB();
        }

        protected void startAutoUpdate()
        {
            Ctx.mInstance.mAutoUpdateSys.mOnUpdateEndDisp.addEventHandle(null, onAutoUpdateEnd);
            Ctx.mInstance.mAutoUpdateSys.startUpdate();
            //onAutoUpdateEnd();
        }

        // 调用这个函数，说明文件已经更新到本地，版本文件也加载完成
        public void onAutoUpdateEnd(IDispatchObject dispObj)
        {
            this.loadPakCfg();
        }

        protected void loadPakCfg()
        {
            Ctx.mInstance.mPakSys.m_pakCfgLoadDisp = onPakSysCfgEnd;
            Ctx.mInstance.mPakSys.loadFile();
        }

        // 调用这个函数，说明打包信息加载完成
        protected void onPakSysCfgEnd()
        {
            Ctx.mInstance.mModuleSys.loadModule(ModuleID.LOGINMN);
        }

        // 卸载模块
        public void unload()
        {
            
        }
    }
}