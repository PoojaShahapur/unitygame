namespace SDK.Lib
{
    /**
     * @brief 链接到对象上去的特效，岁对象一起移动
     */
    public class LinkEffect : EffectBase
    {
        protected SceneEntityBase m_linkedEntity;       // 连接的对象，测试使用

        public LinkEffect(EffectRenderType renderType) :
            base(renderType)
        {
             
        }

        public SceneEntityBase linkedEntity
        {
            get
            {
                return m_linkedEntity;
            }
            set
            {
                m_linkedEntity = value;
            }
        }

        override public void setTableID(int tableId)
        {
            base.setTableID(tableId);
            //UtilApi.adjustEffectRST((m_render as EffectSpriteRender).spriteRender.selfGo.transform);
        }
    }
}