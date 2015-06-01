using SDK.Common;

namespace Game.UI
{
    /**
     * @brief 受伤数据
     */
    public class HurtData
    {
        protected MList<HurtItemBase> m_hurtList;
        protected HurtItemBase m_curHurtItem;           // 当前被击项

        public HurtData()
        {
            m_hurtList = new MList<HurtItemBase>();
        }

        public MList<HurtItemBase> hurtList
        {
            get
            {
                return m_hurtList;
            }
        }

        public HurtItemBase curHurtItem
        {
            get
            {
                return m_curHurtItem;
            }
            set
            {
                m_curHurtItem = value;
            }
        }

        public void addItem(HurtItemBase item)
        {
            m_hurtList.Add(item);
        }

        public void removeItem(HurtItemBase item)
        {
            m_hurtList.Remove(item);
        }

        public void onTime(float delta)
        {
            foreach(var item in m_hurtList.list)
            {
                item.onTime(delta);
            }
        }
    }
}