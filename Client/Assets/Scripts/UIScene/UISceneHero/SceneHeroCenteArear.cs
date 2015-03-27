using SDK.Common;
using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 中间区域显示和数据处理
     */
    public class SceneHeroCenteArear : SceneHeroAreaBase
    {
        public List<SceneHeroItem> m_heroList = new List<SceneHeroItem>();
        public UIGrid m_heroGrid = new UIGrid();

        public SceneHeroCenteArear(SceneHeroData data)
            : base(data)
        {
            m_heroGrid.setGameObject(UtilApi.TransFindChildByPObjAndPath(m_sceneHeroData.m_goRoot, "HeroGrid"));

            m_heroGrid.cellWidth = 0.388f;
            m_heroGrid.cellHeight = 0.651f;
        }

        public void updateAllHero()
        {
            releaseAll();

            int idx = 0;
            int idy = 0;

            int idTotal = 0;
            SceneHeroItem item = null;
            while(idTotal < 9 && idTotal < Ctx.m_instance.m_dataPlayer.m_dataHero.m_heroList.Count)
            {
                idy = idTotal / 3;
                idx = idTotal % 3;

                item = new SceneHeroItem();
                m_heroList.Add(item);
                //item.m_tran = m_sceneHeroData.m_goRoot.transform;
                item.m_grid = m_heroGrid;
                item.setDefaultRes();
                item.load();

                ++idTotal;
            }

            //m_heroGrid.Reposition();
        }

        public void releaseAll()
        {
            foreach(SceneHeroItem item in m_heroList)
            {
                if(item != null)
                {
                    item.unload();
                }
            }

            m_heroList.Clear();
        }
    }
}