using SDK.Common;
using SDK.Lib;
using UnityEngine;

namespace Game.UI
{
    /**
     * @brief 场景中的白色卡牌，就是用来占位使用的
     */
    public class WhiteCard : SceneCardBase
    {
        public WhiteCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_render = new WhiteCardRender();
        }

        override public void setIdAndPnt(uint objId, GameObject pntGo_)
        {
            (m_render as WhiteCardRender).setIdAndPnt(objId, pntGo_);
        }

        // 这个其实没什么初始化
        override public void init()
        {

        }

        override public void setBaseInfo(EnDZPlayer m_playerFlag, CardArea area, CardType cardType)
        {

        }
    }
}