namespace SDK.Lib
{
    /**
     * @brief 卡牌子模型类型
     */
    public enum CardSubPartType
    {
        eHeader,        // 头像资源
        eFrame,         // 边框资源
        eBelt,          // 腰带资源
        EQuality,       // 品质资源
        eTotal,         
    }

    public class CardSubPart
    {
        protected AuxStaticModelDynTex m_tex;

        public CardSubPart()
        {
            m_tex = new AuxStaticModelDynTex();
        }

        public AuxStaticModelDynTex tex
        {
            get
            {
                return m_tex;
            }
        }

        public void dispose()
        {
            if (m_tex != null)
            {
                m_tex.dispose();
                m_tex = null;
            }
        }
    }
}