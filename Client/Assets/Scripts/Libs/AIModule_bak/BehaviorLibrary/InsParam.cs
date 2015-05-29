using SDK.Lib;

namespace BehaviorLibrary
{
    /**
     * @brief 实例参数，行为树执行时传入的操作数据
     */
    public class InsParam
    {
        protected BeingEntity m_beingEntity;

        public BeingEntity beingEntity
        {
            get
            {
                return m_beingEntity;
            }
            set
            {
                m_beingEntity = value;
            }
        }
    }
}
