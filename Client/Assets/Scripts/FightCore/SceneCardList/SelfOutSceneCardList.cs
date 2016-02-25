using SDK.Lib;

namespace FightCore
{
    public class SelfOutSceneCardList : OutSceneCardList
    {
        public SelfOutSceneCardList(SceneDZData data, EnDZPlayer playerSide)
            : base(data, playerSide)
        {

        }

        override public void clearAttTimes()
        {
            base.clearAttTimes();

            foreach(var card in m_sceneCardList.list())
            {
                card.clearAttTimes();
            }
        }

        override public void updateCanLaunchAttState(bool bEnable)
        {
            base.updateCanLaunchAttState(bEnable);

            foreach (var card in m_sceneCardList.list())
            {
                card.effectControl.updateCanLaunchAttState(bEnable);
            }
        }
    }
}