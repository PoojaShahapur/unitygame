using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 敌人的背面牌
     */
    public class BlackCard : SceneCardBase
    {
        public BlackCard(SceneDZData data) : 
            base(data)
        {
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_trackAniControl = new TrackAniControl(this);
            m_render = new BlackCardRender(this);
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as BlackCardRender).setIdAndPnt(objId, pntGo_);
        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {

        }
    }
}