using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 主要处理行为相关的操作，状态、战斗等
     */
    public class BeingBehaviorControl : BeingControlBase
    {
        protected Vector3 m_srcPos;                 // 保存最初的位置

        public BeingBehaviorControl(BeingEntity rhv) : 
            base(rhv)
        {

        }

        public Vector3 srcPos
        {
            get
            {
                return m_srcPos;
            }
            set
            {
                m_srcPos = value;
            }
        }
    }
}