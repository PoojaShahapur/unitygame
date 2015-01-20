namespace SDK.Common
{
    public enum EnDZPlayer
    {
        ePlayerSelf,            // 自己
        ePlayerEnemy,           // 敌人
        ePlayerTotal            // 总共数量
    }

    /**
     * @brief 对战基本数据
     */
    public class DZData
    {
        public bool m_canReqDZ = true;     // 是否可以请求对战，如果已经请求了需要等待服务器返回
        public DZPlayer[] m_playerArr = new DZPlayer[(int)EnDZPlayer.ePlayerTotal];
        public byte m_priv;                 // 当前拥有权力的一方
        public byte m_state;                // 当前游戏所处的阶段，每一个阶段允许的操作是不同的

        public DZData()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf] = new DZPlayer();
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy] = new DZPlayer();
        }

        public void setSelfHeroInfo(CardGroupItem cardGroup)
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroName = cardGroup.m_cardGroup.name;
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation = cardGroup.m_cardGroup.occupation;
        }

        public void clear()
        {
            m_playerArr[(int)EnDZPlayer.ePlayerSelf].clear();
            m_playerArr[(int)EnDZPlayer.ePlayerEnemy].clear();
        }
    }
}