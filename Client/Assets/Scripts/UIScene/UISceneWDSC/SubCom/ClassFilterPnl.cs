using SDK.Common;

namespace Game.UI
{
    public class ClassFilterPnl
    {
        public SceneWDSCData m_sceneWDSCData;

        public ClassFilterBtn[] m_tabBtnList = new ClassFilterBtn[10];
        public int m_tabBtnIdx = -1;         // 当前点击的 Tab Btn 索引

        public void findWidget()
        {
            int idx = 0;

            idx = 0;
            while (idx < 10)
            {
                m_tabBtnList[idx] = new ClassFilterBtn();
                m_tabBtnList[idx].sceneWDSCData = m_sceneWDSCData;
                ++idx;
            }

            m_tabBtnList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/fs"));
            m_tabBtnList[0].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[0].tag = 1;

            m_tabBtnList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sq"));
            m_tabBtnList[1].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[1].tag = 2;

            m_tabBtnList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ms"));
            m_tabBtnList[2].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[2].tag = 3;

            m_tabBtnList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dz"));
            m_tabBtnList[3].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[3].tag = 4;

            m_tabBtnList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/sm"));
            m_tabBtnList[4].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[4].tag = 5;

            m_tabBtnList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/ss"));
            m_tabBtnList[5].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[5].tag = 6;

            m_tabBtnList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zs"));
            m_tabBtnList[6].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[6].tag = 7;

            m_tabBtnList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/lr"));
            m_tabBtnList[7].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[7].tag = 8;

            m_tabBtnList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/dly"));
            m_tabBtnList[8].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[8].tag = 9;

            // 最后一个
            m_tabBtnList[9].setGameObject(UtilApi.GoFindChildByPObjAndName("wdscjm/classfilter/zl"));
            m_tabBtnList[9].sceneWDSCData = m_sceneWDSCData;
            m_tabBtnList[9].tag = 0;
        }

        // 点击分类按钮
        public void updateByCareer(EnPlayerCareer myclass, bool bUpdate)
        {
            if ((int)myclass != m_tabBtnIdx)
            {
                if (bUpdate)
                {
                    foreach (ClassFilterBtn item in m_tabBtnList)
                    {
                        if (item.tag == (int)myclass)    // 当期 Down 按钮 Up
                        {
                            item.btnUpAni();
                        }
                        else if (item.tag == m_tabBtnIdx)       // 之前 Up 状态按钮 Down
                        {
                            item.btnDownAni();
                        }
                    }
                }

                m_tabBtnIdx = (int)myclass;
            }

            m_sceneWDSCData.m_wdscCardPnl.destroyAndUpdateCardList();
        }

        // 返回是否是当前的 Up 按钮索引
        public bool bCurUpBtn(int btnIdx)
        {
            return m_tabBtnIdx == btnIdx;
        }

        // 隐藏掉
        public void hideClassFilterBtnExceptThis(EnPlayerCareer c)
        {
            foreach (ClassFilterBtn item in m_sceneWDSCData.m_pClassFilterPnl.m_tabBtnList)
            {
                item.hideClassFilterBtnExceptThis(c);
            }
        }

        // 过滤面板按钮恢复原始位置
        public void gotoBack()
        {
            foreach (ClassFilterBtn item in m_tabBtnList)
            {
                item.gotoBack();
            }
        }
    }
}