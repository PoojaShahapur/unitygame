using Game.Msg;
using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class FightResultPanel : NpcEntityBase
    {
        protected AuxStaticModel m_model;           // 点击的背景面板
        protected LinkEffect m_firstEffect;         // 第一个特效，第一个特效非循环
        protected LinkEffect m_sndEffect;           // 第二个特效，第二个特效循环，直到点击退出
        protected int m_firstEffectId;              //  第一个特效 Id
        protected int m_sndEffectId;              //  第一个特效 Id

        public FightResultPanel()
        {
            m_model = new AuxStaticModel();
        }

        public override void init()
        {
            // 添加事件
            //UtilApi.addEventHandle(m_model.selfGo, OnBtnClk);
        }
        
        override public GameObject gameObject()
        {
            return m_model.selfGo;
        }

        override public void setGameObject(GameObject rhv)
        {
            m_model.selfGo = rhv;
            init();
        }

        override public void dispose()
        {
            if (m_firstEffect != null)
            {
                m_firstEffect.dispose();
                m_firstEffect = null;
            }

            if (m_sndEffect != null)
            {
                m_sndEffect.dispose();
                m_sndEffect = null;
            }
        }

        override public void show()
        {
            if (m_model != null)
            {
                m_model.show();
            }

            showResult();
        }

        override public void hide()
        {
            if (m_model != null)
            {
                m_model.hide();
            }
        }

        protected void OnBtnClk(GameObject go)
        {
            Ctx.m_instance.m_gameSys.loadGameScene();        // 加载游戏场景
        }

        public void showResult()
        {
            if (m_firstEffect == null)
            {
                m_firstEffect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(m_firstEffectId, gameObject(), true, false) as LinkEffect;
            }
            m_firstEffect.addEffectPlayEndHandle(onFirstEffectPlayEndHandle);
        }

        protected void onFirstEffectPlayEndHandle(IDispatchObject dispObj)
        {
            UtilApi.addEventHandle(m_model.selfGo, OnBtnClk);

            m_firstEffect = null;
            if (m_sndEffect == null)
            {
                m_sndEffect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(m_sndEffectId, gameObject(), false, true) as LinkEffect;
            }
        }

        public void psstRetBattleGameResultUserCmd(stRetBattleGameResultUserCmd cmd)
        {
            if(1 == cmd.win)
            {
                m_firstEffectId = 26;
                m_sndEffectId = 27;
            }
            else
            {
                m_firstEffectId = 28;
                m_sndEffectId = 29;
            }

            show();
        }
    }
}