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

                stReqEndMyRoundUserCmd cmd = new stReqEndMyRoundUserCmd();
                UtilMsg.sendMsg(cmd);
            }
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