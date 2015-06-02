namespace Game.UI
{
    public class ComHurtItem : HurtItemBase
    {
        protected int m_hurtEffectId;   // 攻击受伤 Id

        public ComHurtItem()
        {

        }

        public int hurtEffectId
        {
            get
            {
                return m_hurtEffectId;
            }
            set
            {
                m_hurtEffectId = value;
            }
        }

        // 执行当前的受伤操作
        override public void execHurt(SceneCardBase card)
        {

        }
    }
}