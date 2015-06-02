using BehaviorLibrary;
using FSM;

namespace Game.UI
{
    public class SceneCard : SceneCardBase
    {
        public SceneCard(SceneDZData data) : 
            base(data)
        {
            m_sceneCardBaseData = new SceneCardBaseData();
            m_sceneCardBaseData.m_fightData = new FightData();
            m_sceneCardBaseData.m_animFSM = new AnimFSM();
            m_sceneCardBaseData.m_animFSM.card = this;
            m_sceneCardBaseData.m_animFSM.Start();

            m_sceneCardBaseData.m_aiController = new AIController();
            m_sceneCardBaseData.m_aiController.possess(this);
        }
    }
}