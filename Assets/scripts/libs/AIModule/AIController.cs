using UnityEngine;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
    /**
     * @brief 凡是 BehaviorTree 之外的 AI 逻辑都写在这里
     */
    public class AIController
    {
        protected Biped m_vehicle;
        protected Radar m_radar;                // 每一个人身上有一个雷达
        protected AILocalState m_aiLocalState;

        public AIController()
        {
            m_aiLocalState = new AILocalState();
        }

        public AILocalState aiLocalState
        {
            get
            {
                return m_aiLocalState;
            }
            set
            {
                m_aiLocalState = value;
            }
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

        public void OnTick(float delta)
        {
            if(m_radar != null)
            {
                m_radar.UpdateRadar();      // 更新雷达数据
            }
        }

        public void initControl(SkinAniModel skinAniModel)
        {
            m_radar = new Radar();
            m_vehicle = new Biped();
            m_vehicle.Radar = m_radar;
            m_vehicle.initOwner(skinAniModel.rootGo);
            m_vehicle.AllowedMovementAxes = new Vector3(1, 0, 1);
            m_vehicle.MaxSpeed = 10;
            m_vehicle.setSpeed(5);
        }
    }
}