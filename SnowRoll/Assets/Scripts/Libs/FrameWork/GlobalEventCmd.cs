namespace SDK.Lib
{
    /**
     * @brief 全局性的事件
     */
    public class GlobalEventCmd
    {
        static public void onSample()
        {

        }

        static public void onEnterWorld()
        {
            //操作模式在UISettingsPanel.lua中设置
            if (Ctx.mInstance.mSystemSetting.hasKey("OptionModel"))
            {
                if(Ctx.mInstance.mSystemSetting.getInt("OptionModel") == 1)
                {
                    Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUIJoyStick);
                }
                else
                {
                    Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUIForwardForce);
                }
            }
            else
            {
                Ctx.mInstance.mUiMgr.loadAndShow(UIFormId.eUIJoyStick);
            }
            Ctx.mInstance.mLuaSystem.onPlayerMainLoaded();
        }
    }
}