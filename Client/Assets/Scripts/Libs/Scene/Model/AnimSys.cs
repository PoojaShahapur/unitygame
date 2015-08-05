using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief Unity 最新的动画系统
     */
    public class AnimSys
    {
        private Animator m_Animator = null;

        private int m_SpeedId = 0;
        private int m_AgularSpeedId = 0;
        private int m_DirectionId = 0;

        public float m_SpeedDampTime = 0.1f;
        public float m_AnguarSpeedDampTime = 0.25f;
        public float m_DirectionResponseTime = 0.2f;

        public AnimSys()
        {
            m_SpeedId = Animator.StringToHash("Speed");
            m_AgularSpeedId = Animator.StringToHash("AngularSpeed");
            m_DirectionId = Animator.StringToHash("Direction");
        }

        public Animator animator
        {
            get
            {
                return m_Animator;
            }
            set
            {
                m_Animator = value;
            }
        }

        public void Do(float speed, float direction)
        {
            AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0);

            bool inTransition = m_Animator.IsInTransition(0);
            bool inIdle = state.IsName("Locomotion.Idle");
            bool inTurn = state.IsName("Locomotion.TurnOnSpot") || state.IsName("Locomotion.PlantNTurnLeft") || state.IsName("Locomotion.PlantNTurnRight");
            bool inWalkRun = state.IsName("Locomotion.WalkRun");

            float speedDampTime = inIdle ? 0 : m_SpeedDampTime;
            float angularSpeedDampTime = inWalkRun || inTransition ? m_AnguarSpeedDampTime : 0;
            float directionDampTime = inTurn || inTransition ? 1000000 : 0;

            float angularSpeed = direction / m_DirectionResponseTime;

            m_Animator.SetFloat(m_SpeedId, speed, speedDampTime, Time.deltaTime);
            m_Animator.SetFloat(m_AgularSpeedId, angularSpeed, angularSpeedDampTime, Time.deltaTime);
            m_Animator.SetFloat(m_DirectionId, direction, directionDampTime, Time.deltaTime);
        }

        // 播放动画
        public void playAnim(eBeingActId actState)
        {
            // 首先重置

            // 然后设置具体的动作
        }
    }
}