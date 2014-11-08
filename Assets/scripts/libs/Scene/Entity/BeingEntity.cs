using BehaviorLibrary;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
	/**
	 * @brief 生物 
	 */
	public class BeingEntity
	{
        protected SkinAniModel m_skinAniModel;            // 一个数组

        // AI 数据
        protected Biped m_vehicle;
        protected BehaviorTree m_behaviorTree;

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
        }

        public Biped vehicle
        {
            get
            {
                return m_vehicle;
            }
            set
            {
                m_vehicle = value;
            }
        }

        // 添加 AI
        virtual public void addAi(BehaviorTree behaviorTree)
        {
            m_vehicle = new Biped();
            m_behaviorTree = behaviorTree;
        }
	}
}