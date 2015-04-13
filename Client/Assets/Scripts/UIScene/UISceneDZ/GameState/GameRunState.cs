namespace Game.UI
{
    /**
     * @brief 游戏运行状态
     */
    public class GameRunState
    {
        public const int INITCARD = 0;      // 获取初始卡牌阶段
        public const int STARTDZ = 1;       // 开始对战

        protected SceneDZData m_sceneDZData;
        protected int m_curState;

        public GameRunState(SceneDZData sceneDZData)
        {
            m_sceneDZData = sceneDZData;
            m_curState = INITCARD;
        }

        // 进入某种状态
        public void enterState(int state)
        {
            m_curState = state;
        }

        // 是否处于当前状态
        public bool isInState(int state)
        {
            return m_curState == state;
        }
    }
}