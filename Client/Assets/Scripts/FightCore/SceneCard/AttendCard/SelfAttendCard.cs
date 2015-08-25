using SDK.Lib;

namespace FightCore
{
    /**
     * @brief 随从卡，必然是自己的， Enemy 随从卡是用的是 BlackCard
     */
    public class SelfAttendCard : AttendCard
    {
        public SelfAttendCard(SceneDZData sceneDZData) :
            base(sceneDZData)
        {
            m_sceneCardBaseData.m_trackAniControl = new SelfAttendAniControl(this);
            m_sceneCardBaseData.m_behaviorControl = new SelfAttendBehaviorControl(this);
        }

        override public int getZhanHouCommonClientIdx()
        {
            return preIndex;
        }

        // 随从战后可以移动回来
        override public void addGridElem2DynGrid()
        {
            // 从列表中移除，不置空，因为战吼可能退回来
            m_sceneDZData.m_sceneDZAreaArr[(int)this.sceneCardItem.playerSide].inSceneCardList.getDynSceneGrid().addElem(this.trackAniControl.getGridElement(), this.curIndex);
        }
    }
}