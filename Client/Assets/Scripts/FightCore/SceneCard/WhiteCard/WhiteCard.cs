using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中的白色卡牌，就是用来占位使用的
     */
    public class WhiteCard : ExceptBlackSceneCard
    {
        public WhiteCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_render = new WhiteCardRender(this);
            m_sceneCardBaseData.m_trackAniControl = new TrackAniControl(this);
        }

        override public void dispose()
        {
            Ctx.m_instance.m_logSys.log("客户端彻底删除 White 卡牌");
            base.dispose();
        }

        override protected void removeRef()
        {
            
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as WhiteCardRender).setIdAndPnt(objId, pntGo_);
        }

        // 这个其实没什么初始化
        override public void init()
        {
            if (m_sceneCardBaseData.m_trackAniControl != null)
            {
                m_sceneCardBaseData.m_trackAniControl.init();
            }
        }

        override public void onTick(float delta)
        {

        }

        public override string getDesc()
        {
            return string.Format("WhiteCard {0}", m_ClientId);
        }

        // 白色卡牌不用更新状态特效
        override public void updateStateEffect()
        {

        }
    }
}