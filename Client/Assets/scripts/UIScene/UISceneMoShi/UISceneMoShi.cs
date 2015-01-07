﻿using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 模式界面
     */
    public class UISceneMoShi : SceneForm, IUISceneMoShi
    {
        protected moshijm m_moshijm = new moshijm();
        protected yxclick[] m_heroList = new yxclick[9];

        public override void onReady()
        {
            base.onReady();

            getWidget();
            addEventHandle();
        }

        protected void getWidget()
        {
            m_moshijm.setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm"));
            int idx = 0;
            while(idx < 9)
            {
                m_heroList[idx] = new yxclick();

                ++idx;
            }

            m_heroList[0].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/战士"));
            m_heroList[1].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/萨满祭司"));
            m_heroList[2].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/潜行者"));

            m_heroList[3].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/圣骑士"));
            m_heroList[4].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/猎人"));
            m_heroList[5].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/德鲁伊"));

            m_heroList[6].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/术士"));
            m_heroList[7].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/法师"));
            m_heroList[8].setGameObject(UtilApi.GoFindChildByPObjAndName("moshijm/jbtp/牧师"));
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneMoshiPath.BtnReturn), onBtnClkReturn);   // 返回
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(SceneMoshiPath.BtnSelect), onBtnClkSelect);   // 返回
        }

        protected void onBtnClkReturn(GameObject go)
        {
            m_moshijm.gotoback();
        }

        protected void onBtnClkSelect(GameObject go)
        {
            m_moshijm.chooseok();

            stReqCreateOneCardGroupUserCmd cmd = new stReqCreateOneCardGroupUserCmd();
            cmd.occupation = (uint)m_moshijm.getClass();
            UtilMsg.sendMsg(cmd);
        }

        public void AddNewTaoPai()
        {
            m_moshijm.newset();
        }

        public void setclass(CardClass c)
        {
            m_moshijm.setclass(c);
        }

        public void setClassname(string n)
        {
            m_moshijm.setClassname(n);
        }

        public void setclasspic(Material pic)
        {
            m_moshijm.setclasspic(pic);
        }

        public CardClass getClass()
        {
            return m_moshijm.getClass();
        }
    }
}