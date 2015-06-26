using UnityEngine;
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
    public class StartBtn : NpcEntityBase
    {
        
        protected AuxStaticModel m_model;
             
        protected LinkEffect m_effect;

        public StartBtn()
        {
            m_model = new AuxStaticModel();
        }

       
        override public GameObject gameObject()
        {
            return m_model.selfGo;
        }

        override public void setGameObject(GameObject rhv)
        {
            m_model.selfGo = rhv;
        }

        override public void dispose()
        {
            base.dispose();

            if (m_effect != null)
            {
                m_effect.dispose();
                m_effect = null;
            }
        }

        public void updateEffect()
        {
            if (m_effect == null)
            {
                addFrameEffect();
            }
            else
            {
                m_effect.play();
            }
        }

        // 添加边框特效
        protected void addFrameEffect()
        {
            if (m_effect == null)
            {
                m_effect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(30, gameObject(), false, true) as LinkEffect;
            }
        }
    }
}