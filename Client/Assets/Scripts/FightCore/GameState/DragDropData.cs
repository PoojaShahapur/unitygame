using SDK.Lib;

namespace FightCore
{
    public class DragDropData
    {
        protected SceneCardBase m_curDragItem;             // 当前正在拖放的 item
        protected bool m_bDownInCard;

        public DragDropData()
        {
            m_bDownInCard = false;
        }

        public SceneCardBase getCurDragItem()
        {
            return m_curDragItem;
        }

        // 设置、清除当前卡牌
        public void setCurDragItem(SceneCardBase curDragItem_)
        {
            m_curDragItem = curDragItem_;
        }

        // 尝试清理当前拖放卡牌
        public void tryClearDragItem(SceneCardBase curDragItem_)
        {
            if(UtilApi.isAddressEqual(m_curDragItem, curDragItem_))
            {
                setCurDragItem(null);
            }
        }

        // 是否在拖动卡牌
        public bool bInDargCarding()
        {
            return m_curDragItem != null;
        }

        // 是否在手牌卡牌中按下
        public void setDownInCard(bool bDownInCard_)
        {
            m_bDownInCard = bDownInCard_;
        }

        public bool getDownInCard()
        {
            return m_bDownInCard;
        }

        // 如果进入对方回合，需要将当前卡牌退回到手牌区域
        public void backCard2Orig()
        {
            if(m_curDragItem != null)
            {
                m_curDragItem.ioControl.backCard2Orig();
            }
        }
    }
}