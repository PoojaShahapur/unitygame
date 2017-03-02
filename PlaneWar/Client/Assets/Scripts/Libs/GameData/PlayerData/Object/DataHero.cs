using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 玩家所有的 hero 数据
     */
    public class DataHero
    {
        protected bool mCanReqAllHero;

        public MList<HeroItem> mHeroList;
        public MDictionary<uint, HeroItem> mId2HeroDic;

        public DataHero()
        {
            this.mCanReqAllHero = false;
            this.mHeroList = new MList<HeroItem>();
            this.mId2HeroDic = new MDictionary<uint, HeroItem>();
        }

        public void reqAllHero()
        {
            
        }

        // 所有 hero 的数据
        public void psstRetAllHeroInfoUserCmd(List<t_hero> infoList)
        {
            mHeroList.Clear();
            mId2HeroDic.Clear();

            HeroItem clientItem;
            foreach (t_hero item in infoList)
            {
                clientItem = new HeroItem();
                clientItem.m_svrHero = item;
                mHeroList.Add(clientItem);
                mId2HeroDic[clientItem.m_svrHero.occupation] = clientItem;
            }
        }

        public void psstRetOneHeroInfoUserCmd(t_hero info)
        {
            HeroItem clientItem;
            if (!mId2HeroDic.ContainsKey(info.occupation))
            {
                clientItem = new HeroItem();
                clientItem.m_svrHero = info;
                mHeroList.Add(clientItem);
                mId2HeroDic[clientItem.m_svrHero.occupation] = clientItem;
            }
            else
            {
                // 直接拷贝
                mId2HeroDic[info.occupation].m_svrHero.copyFrom(info);
            }
        }

        public HeroItem getJobInfo(int id)
        {
            foreach (var item in mHeroList.list())
            {
                if(item.m_svrHero.occupation == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}