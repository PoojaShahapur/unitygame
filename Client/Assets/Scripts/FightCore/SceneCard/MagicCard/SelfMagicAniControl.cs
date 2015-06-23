using SDK.Lib;
using UnityEngine;

namespace FightCore
{
    public class SelfMagicAniControl : MagicAniControl
    {
        protected ScaleGridElement m_scaleGridElement;

        public SelfMagicAniControl(SceneCardBase rhv) : 
            base(rhv)
        {
            m_scaleGridElement = new ScaleGridElement();
            m_scaleGridElement.setMovedEntity(this);
        }

        public override void dispose()
        {
            // 移除格子
            removeGridElem();
            base.dispose();
        }

        override public void createAndAddGridElem()
        {
            m_scaleGridElement = m_card.m_sceneDZData.m_sceneDZAreaArr[(int)m_card.sceneCardItem.m_playerSide].inSceneCardList.getDynSceneGrid().createAndAddElem(GridElementType.eScale) as ScaleGridElement;
            m_scaleGridElement.setMovedEntity(this);
        }

        override public void removeGridElem()
        {
            // 从列表中移除，不置空，因为战吼可能退回来
            m_card.m_sceneDZData.m_sceneDZAreaArr[(int)m_card.sceneCardItem.m_playerSide].inSceneCardList.getDynSceneGrid().removeElem(m_scaleGridElement);
        }

        override public GridElementBase getGridElement()
        {
            return m_scaleGridElement;
        }

        override public void setNormalPos(Vector3 pos)
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eHandDown);
            m_curPt.pos = pos;
            moveToDestPos(PosType.eHandDown);
        }

        override public void setExpandPos(Vector3 pos)
        {
            m_curPt = m_wayPtList.getPosInfo(PosType.eScaleUp);

            m_curPt.pos = pos;
            if (m_scaleGridElement.isNormalState())
            {
                m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
                m_curPt.scale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
            }
            else
            {
                m_curPt.rot = new UnityEngine.Vector3(0, 0, 0);
                m_curPt.scale = new UnityEngine.Vector3(1, 1, 1);
            }

            moveToDestPos(PosType.eScaleUp);
        }

        override public void normalState()
        {
            // 只有没有拖动卡牌的时候才更新
            if (!m_card.m_sceneDZData.bInDargCarding())
            {
                m_scaleGridElement.normal();
            }
        }

        override public void expandState()
        {
            if (!m_card.m_sceneDZData.bInDargCarding())
            {
                m_scaleGridElement.expand();
            }
        }
    }
}