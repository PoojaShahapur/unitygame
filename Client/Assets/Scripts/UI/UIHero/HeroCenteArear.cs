using SDK.Lib;
using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 中间区域显示和数据处理
     */
    public class HeroCenteArear : HeroAreaBase
    {
        public List<HeroItemCom> m_heroList = new List<HeroItemCom>();
        public UIGrid m_heroGrid = new UIGrid();

        public HeroCenteArear(HeroData data)
            : base(data)
        {
            m_heroGrid.setGameObject(UtilApi.TransFindChildByPObjAndPath(m_heroData.m_goRoot, "HeroGrid"));

            m_heroGrid.cellWidth = 0.388f;
            m_heroGrid.cellHeight = 0.651f;
        }

        public void updateAllHero()
        {
            releaseAll();

            int idx = 0;
            int idy = 0;

            int idTotal = 0;
            HeroItemCom item = null;
            while(idTotal < 9 && idTotal < Ctx.m_instance.m_dataPlayer.m_dataHero.m_heroList.Count)
            {
                idy = idTotal / 3;
                idx = idTotal % 3;

                item = new HeroItemCom();
                m_heroList.Add(item);

                ++idTotal;
            }
        }

        public void releaseAll()
        {
            foreach(HeroItemCom item in m_heroList)
            {
                if(item != null)
                {
                    
                }
            }

            m_heroList.Clear();
        }
    }
}