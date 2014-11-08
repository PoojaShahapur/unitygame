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

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
        }
	}
}