using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class CrystalPtPanel
    {
        protected AuxLabel m_text;
        protected UIGrid m_mpGrid;
        protected EnDZPlayer m_playerSide;                 // 指示玩家的位置
        protected MList<CrystalPtItem> m_crystalList;

        public CrystalPtPanel(EnDZPlayer playerFlag_)
        {
            m_text = new AuxLabel();
            m_mpGrid = new UIGrid();
            m_playerSide = playerFlag_;
            m_crystalList = new MList<CrystalPtItem>();
        }

        public void findWidget()
        {
            if (EnDZPlayer.ePlayerSelf == m_playerSide)
            {
                m_text = new AuxLabel(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfMpText));
            }
            else
            {
                m_text = new AuxLabel(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyMpText));
            }

            if (EnDZPlayer.ePlayerSelf == m_playerSide)
            {
                m_mpGrid = new UIGrid();
                m_mpGrid.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.SelfMpList));
                m_mpGrid.maxPerLine = 1;
                m_mpGrid.cellWidth = 0.5f;
                m_mpGrid.cellHeight = 0.5f;
            }
            else
            {
                m_mpGrid = new UIGrid();
                m_mpGrid.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.EnemyMpList));
                m_mpGrid.maxPerLine = 10;
                m_mpGrid.cellWidth = 0.5f;
                m_mpGrid.cellHeight = 0.5f;
            }
        }

        public void updateMp()
        {
            // 更新 MP 数据显示
            m_text.text = string.Format("{0}/{1}", Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp, Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp);

            // 由于技能可能改变 mp 数量，但是不改变 maxmp ，导致 mp 比 maxmp 还大
            int maxMp = (int)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp;
            if(maxMp < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp)
            {
                maxMp = (int)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp;
            }

            int idx = 0;
            // 更新 MP 模型显示
            if (m_crystalList.Count() < maxMp)  // 如果 maxmp 多了
            {
                idx = m_crystalList.Count();
                while (idx < maxMp)
                {
                    m_crystalList.Add(new CrystalPtItem());
                    m_crystalList[idx].load(m_mpGrid.getGameObject());
                    ++idx;
                }

                m_mpGrid.Reposition();
            }
            else if(m_crystalList.Count() > maxMp)   // 隐藏多余的
            {
                idx = maxMp;
                while (idx < m_crystalList.Count())
                {
                    m_crystalList[idx].modelItem.hide();
                    ++idx;
                }

                m_mpGrid.Reposition();
            }

            // 设置可见的
            idx = 0;
            while (idx < maxMp)
            {
                if (!m_crystalList[idx].modelItem.IsVisible())
                {
                    m_crystalList[idx].modelItem.show();
                }
                m_crystalList[idx].updateTexture(true);         // 默认都变亮
                ++idx;
            }

            // 只有这种情况下才
            if(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp > Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp)
            {
                // 从后向前变暗
                idx = (int)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp;
                while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp)
                {
                    m_crystalList[idx].updateTexture(false);
                    ++idx;
                }

                // 将前面的变亮
                idx = 0;
                while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp)
                {
                    m_crystalList[idx].updateTexture(true);
                    ++idx;
                }
            }

            //GameObject go = null;

            // 更新哪些是可以使用的 mp
            //idx = 0;
            //while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp - Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.mp)
            //{
            //    m_crystalList[idx].modelItem.hide();
            //    ++idx;
            //}

            //while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp)
            //{
            //    m_crystalList[idx].modelItem.show();
            //    ++idx;
            //}

            //// 继续更新可用的 Mp 也可能是锁住的
            //if (Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.forbid > 0)
            //{
            //    // 显示一把锁
            //    idx = 0;
            //    int endidx = (int)(Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.maxmp - 1);
            //    while (idx < Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)m_playerSide].m_heroMagicPoint.forbid)
            //    {
            //        go = UtilApi.TransFindChildByPObjAndPath(m_mpGrid.GetChild(endidx - idx).gameObject, "light");

            //        // 改成一把锁

            //        ++idx;
            //    }
            //}
        }
    }
}