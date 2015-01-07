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

        public SceneHeroCenteArear(SceneHeroData data)
            : base(data)
        {

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
                item.m_tran = m_sceneHeroData.m_goRoot.transform;
                item.setDefaultRes();
                item.load();

                ++idTotal;
            }
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