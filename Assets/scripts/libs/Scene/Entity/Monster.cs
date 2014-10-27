namespace SDK.Lib
{
    class Monster : BeingEntity
    {
        public Monster()
            : base()
        {
            m_skinAniModel = new SkinAniModel[(int)MonstersModelDef.eModelTotal];
        }
    }
}
