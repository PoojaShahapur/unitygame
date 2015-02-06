using SDK.Common;

namespace SDK.Lib
{
    /**
     * @brief 除了卡牌外，场景中其它可交互的对象
     */
    public class InterActiveEntity : LSBehaviour
    {
        protected int m_tag;                 // 唯一 ID

        public int tag
        {
            get
            {
                return m_tag;
            }
            set
            {
                m_tag = value;
            }
        }
    }
}