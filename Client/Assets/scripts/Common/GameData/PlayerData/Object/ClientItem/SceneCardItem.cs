namespace SDK.Common
{
    /**
     * @brief 场景中的一个卡牌
     */
    public class SceneCardItem
    {
        public byte m_preSlot;          // 移动之前插槽位置
        protected byte m_slot;	        // 哪个槽
        public t_Card m_svrCard;        // 服务器卡牌数据
        public TableCardItemBody m_cardTableItem;       // 卡牌表中的数据

        public CardArea m_cardArea;                     // 卡牌在什么位置
        public EnDZPlayer m_playerFlag;                 // 卡牌属性哪个玩家

        public byte curSlot
        {
            get
            {
                return m_preSlot;
            }
            set
            {
                m_preSlot = m_slot;
                m_slot = value;
            }
        }
    }
}