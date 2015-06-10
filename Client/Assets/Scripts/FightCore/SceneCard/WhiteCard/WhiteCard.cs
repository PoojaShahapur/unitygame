using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中的白色卡牌，就是用来占位使用的
     */
    public class WhiteCard : SceneCard
    {
        public WhiteCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_render = new WhiteCardRender(this);
            m_sceneCardBaseData.m_trackAniControl = new TrackAniControl(this);
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
    }
}