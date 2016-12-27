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
            Ctx.mInstance.mLuaSystem.onPlayerMainLoaded();
        }
    }
}