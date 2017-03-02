namespace Game.Game
{
    /**
     * @brief KBE 引擎游戏消息处理
     */
    public class GameNetHandleCB_KBE
    {
        protected NineCmdHandle_KBE mNineCmdHandle_KBE;

        public GameNetHandleCB_KBE()
        {
            mNineCmdHandle_KBE = new NineCmdHandle_KBE();
        }

        public void init()
        {
            mNineCmdHandle_KBE.init();
        }

        public void dispose()
        {
            mNineCmdHandle_KBE.dispose();
        }
    }
}