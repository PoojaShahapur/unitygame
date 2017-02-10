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
            Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIJoyStick);
            Ctx.mInstance.mUiMgr.exitForm(UIFormId.eUIForwardForce);

            if (Ctx.mInstance.mSystemSetting.hasKey("OptionModel"))
            {
                if (Ctx.mInstance.mSystemSetting.getInt("OptionModel") == 1)
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

            if (Ctx.mInstance.mSystemSetting.hasKey("MusicModel"))
            {
                if (Ctx.mInstance.mSystemSetting.getInt("MusicModel") == 1)
                {
                    Ctx.mInstance.mSoundMgr.play("Sound/Music/StudioEIM-myseabed.mp3", true);
                }
            }
            else
            {
                Ctx.mInstance.mSoundMgr.play("Sound/Music/StudioEIM-myseabed.mp3", true);
            }
        }
    }
}