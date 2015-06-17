using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 自己回合提示界面
     */
    public class SelfRoundTip : AuxSceneComponent
    {
        protected LinkEffect m_effect;
        // 显示自己回合提示
        //public void turnBegin()
        //{
        //    //出现你的回合
        //    iTween.ScaleTo(gameObject, Vector3.one, 0.5f);
        //    iTween.ScaleTo(gameObject, iTween.Hash(
        //        "scale", Vector3.one * 0.00001f,
        //        "time", 0.5f,
        //        "delay", 1f
        //        ));
        //}

        public override void dispose()
        {
            base.dispose();

            if(m_effect != null)
            {
                m_effect.dispose();
                m_effect = null;
            }
        }

        public void playEffect()
        {
            if(m_effect == null)
            {
                m_effect = Ctx.m_instance.m_sceneEffectMgr.addLinkEffect(4, this.gameObject, false);
                m_effect.linkObject = this;
            }
            else
            {
                m_effect.play();
            }
        }
    }
}