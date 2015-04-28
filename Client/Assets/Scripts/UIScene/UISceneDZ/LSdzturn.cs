using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using Game.Msg;

namespace Game.UI
{
    /**
     * @brief 点击结束当前一局
     */
    public class dzturn : InterActiveEntity
    {
        //bool ismyturn = false;
        // Use this for initialization

        public SceneDZData m_sceneDZData;
        public bool m_bNeedTipsInfo = true;     // 是否需要弹出提示框
        public int m_clkTipsCnt = 0;               // 点击提示框次数

        public override void Start()
        {
            // 添加事件
            UtilApi.addEventHandle(gameObject, OnBtnClk);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnBtnClk(GameObject go)
        {
            //if (dzcam.ismyturn)
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
                //animation["dzturn"].speed = 1;
                //dzcam.ismyturn = false;
                //animation.Play("dzturn");
                ////endturn
                ////Camera.main.SendMessage("endturn");
                //Ctx.m_instance.m_camSys.m_dzcam.endturn();

                stReqEndMyRoundUserCmd cmd;
                if (!m_bNeedTipsInfo)
                {
                    cmd = new stReqEndMyRoundUserCmd();
                    UtilMsg.sendMsg(cmd);
                }
                else
                {
                    ++m_clkTipsCnt;
                    if (m_clkTipsCnt == 1)
                    {
                        if (!hasLeftMagicPtCanUse())
                        {
                            cmd = new stReqEndMyRoundUserCmd();
                            UtilMsg.sendMsg(cmd);
                        }
                        else    // 你还有可操作的随从
                        {
                            InfoBoxParam param = Ctx.m_instance.m_poolSys.newObject<InfoBoxParam>();
                            param.m_midDesc = Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem0);
                            param.m_btnClkDisp = onInfoBoxBtnClk;
                            param.m_btnOkCap = Ctx.m_instance.m_langMgr.getText(LangTypeId.eDZ4, LangItemID.eItem1);
                            param.m_formID = UIFormID.UIInfo;     // 这里提示使用这个 id
                            UIInfo.showMsg(param);
                        }
                    }
                    else
                    {
                        m_clkTipsCnt = 0;
                        cmd = new stReqEndMyRoundUserCmd();
                        UtilMsg.sendMsg(cmd);
                    }
                }
            }
        }

        public void onInfoBoxBtnClk(InfoBoxBtnType type)
        {
            if (type == InfoBoxBtnType.eBTN_OK)
            {
                m_bNeedTipsInfo = false;
            }
        }

        // 检查是否还有剩余的点数，如果还有，给出提示
        protected bool hasLeftMagicPtCanUse()
        {
            return m_sceneDZData.m_sceneDZAreaArr[(int)EnDZPlayer.ePlayerSelf].inSceneCardList.hasLeftMagicPtCanUse();
        }

        // 显示[结束回合]
        public void myturn()
        {
            animation["dzturn"].speed = -1;
            animation["dzturn"].time = 1;
            animation.Play("dzturn");
            dzcam.ismyturn = true;
        }

        // 显示[对方回合]
        public void enemyTurn()
        {
            animation["dzturn"].speed = 1;
            animation.Play("dzturn");
            //Ctx.m_instance.m_camSys.m_dzcam.endturn();
        }
    }
}