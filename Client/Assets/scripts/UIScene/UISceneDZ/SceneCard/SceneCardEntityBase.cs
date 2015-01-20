using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 场景中卡牌基类
     */
    public class SceneCardEntityBase : LSBehaviour
    {
        public SceneCardItem m_sceneCardItem;

        public void setCardData(SceneCardItem dataitem)
        {
            m_sceneCardItem = dataitem;
        }

        // 更新显示
        public virtual void updateUI()
        {

        }
    }
}