using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 对战界面
     */
    public class UISceneDZ : SceneForm
    {
        public SceneDZData m_sceneDZData = new SceneDZData();
        public SceneDZArea[] m_sceneDZAreaArr = new SceneDZArea[(int)EnDZPlayer.ePlayerTotal];
        public HistoryArea m_historyArea = new HistoryArea();

        public override void onReady()
        {
            base.onReady();
            //Ctx.m_instance.m_camSys.m_dzcam.setGameObject(UtilApi.GoFindChildByPObjAndName("Main Camera"));
            getWidget();
            addEventHandle();

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf] = new SelfDZArea();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_playerFlag = EnDZPlayer.ePlayerSelf;
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_sceneDZData = m_sceneDZData;

            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy] = new EnemyDZArea();
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_playerFlag = EnDZPlayer.ePlayerEnemy;
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_sceneDZData = m_sceneDZData;

            m_historyArea.m_sceneDZData = m_sceneDZData;
        }

        public override void onShow()
        {
            base.onShow();
        }

        // 获取控件
        public void getWidget()
        {
            //GameObject[] goList = UtilApi.FindGameObjectsWithTag("aaaa");
            m_sceneDZData.m_dzturn.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.TurnBtn));
            m_sceneDZData.m_luckycoin.setGameObject(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin));

            m_sceneDZData.m_centerGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.CenterGO);
            m_sceneDZData.m_startGO = UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.StartGO);
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(UtilApi.GoFindChildByPObjAndName(CVSceneDZPath.LuckyCoin), onBtnTurnClk);       // 结束本回合
            UtilApi.addEventHandle(m_sceneDZData.m_startGO, onStartBtnClk);       // 开始游戏
        }

        // 结束回合
        protected void onBtnTurnClk(GameObject go)
        {
            stReqEndMyRoundUserCmd cmd = new stReqEndMyRoundUserCmd();
            UtilMsg.sendMsg(cmd);
        }

        protected void onStartBtnClk(GameObject go)
        {
            stReqFightPrepareOverUserCmd cmd = new stReqFightPrepareOverUserCmd();
            UtilMsg.sendMsg(cmd);

            m_sceneDZData.m_startGO.SetActive(false);
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].startDZ();
        }

        public void psstRetLeftCardLibNumUserCmd(stRetLeftCardLibNumUserCmd msg)
        {

        }

        public void psstRetMagicPointInfoUserCmd(stRetMagicPointInfoUserCmd msg)
        {

        }

        public void psstRetRefreshBattleStateUserCmd(stRetRefreshBattleStateUserCmd msg)
        {

        }

        public void psstRetRefreshBattlePrivilegeUserCmd(stRetRefreshBattlePrivilegeUserCmd msg)
        {

        }

        public void psstAddBattleCardPropertyUserCmd(stAddBattleCardPropertyUserCmd msg, SceneCardItem sceneItem)
        {
            m_sceneDZAreaArr[msg.who - 1].psstAddBattleCardPropertyUserCmd(msg, sceneItem);
        }

        public void psstNotifyFightEnemyInfoUserCmd(stNotifyFightEnemyInfoUserCmd msg)
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].m_hero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerEnemy].m_hero.setclasss((EnPlayerCareer)Ctx.m_instance.m_dataPlayer.m_dzData.m_playerArr[(int)EnDZPlayer.ePlayerSelf].m_heroOccupation);   // 设置职业
        }

        public void psstRetFirstHandCardUserCmd(stRetFirstHandCardUserCmd cmd)
        {
            m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].addInitCard();
        }

        public void psstRetMoveGameCardUserCmd(stRetMoveGameCardUserCmd msg)
        {
            if(msg.success == 1)     // 如果成功
            {

            }
            else
            {

            }
        }
    }
}