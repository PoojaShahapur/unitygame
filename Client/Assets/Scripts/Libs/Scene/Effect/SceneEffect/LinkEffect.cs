using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 链接到对象上去的特效，岁对象一起移动
     */
    public class LinkEffect : EffectBase
    {
        protected System.Object m_linkObject;       // 连接的对象，测试使用

        public LinkEffect()
        {
             
        }

        public System.Object linkObject
        {
            get
            {
                return m_linkObject;
            }
            set
            {
                m_linkObject = value;
            }
        }

        override public void setTableID(int tableId)
        {
            base.setTableID(tableId);
            //UtilApi.adjustEffectRST((m_render as EffectSpriteRender).spriteRender.selfGo.transform);
        }
    }
}