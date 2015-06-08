﻿using UnityEngine;
using System.Collections;
using SDK.Common;
using SDK.Lib;
using Game.Msg;
using Game.UI;

namespace FightCore
{
    /**
     * @brief 点击结束当前一局
     */
    public class RoundBtn : SceneComponent
    {
        public SceneDZData m_sceneDZData;
        public bool m_bNeedTipsInfo = true;     // 是否需要弹出提示框
        public int m_clkTipsCnt = 0;            // 点击提示框次数
        protected Material m_mat;

        protected TextureRes m_selfTex;         // 自己纹理
        protected TextureRes m_enemyTex;        // 别人纹理

        public override void Start()
        {
            // 添加事件
            UtilApi.addEventHandle(gameObject, OnBtnClk);
            m_mat = gameObject.GetComponent<Renderer>().material;
        }

        override public void dispose()
        {
            base.dispose();

            if(m_selfTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_selfTex.GetPath(), null);
                m_selfTex = null;
            }

            if (m_enemyTex != null)
            {
                Ctx.m_instance.m_texMgr.unload(m_enemyTex.GetPath(), null);
                m_enemyTex = null;
            }
        }

        protected void OnBtnClk(GameObject go)
        {
            if (Ctx.m_instance.m_dataPlayer.m_dzData.bSelfSide())
            {
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
                            param.m_formID = UIFormID.eUIInfo;     // 这里提示使用这个 id
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
        public void myTurn()
        {
            if(m_selfTex == null)
            {
                m_selfTex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>("Image/Scene/jieshu_zhanchang.tga");
            }

            m_mat.mainTexture = m_selfTex.getTexture();
        }

        // 显示[对方回合]
        public void enemyTurn()
        {
            if (m_enemyTex == null)
            {
                m_enemyTex = Ctx.m_instance.m_texMgr.getAndSyncLoad<TextureRes>("Image/Scene/duishou_zhanchang.tga");
            }

            m_mat.mainTexture = m_enemyTex.getTexture();
        }
    }
}