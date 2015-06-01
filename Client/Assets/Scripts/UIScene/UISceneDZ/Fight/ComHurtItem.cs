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
    }
}