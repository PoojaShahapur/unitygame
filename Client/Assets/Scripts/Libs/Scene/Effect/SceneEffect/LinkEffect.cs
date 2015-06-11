using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 链接到对象上去的特效，岁对象一起移动
     */
    public class LinkEffect : EffectBase
    {
        public LinkEffect()
        {
             
        }

        override public void setTableID(int tableId)
        {
            base.setTableID(tableId);
            //UtilApi.adjustEffectRST((m_render as EffectSpriteRender).spriteRender.selfGo.transform);
        }
    }
}