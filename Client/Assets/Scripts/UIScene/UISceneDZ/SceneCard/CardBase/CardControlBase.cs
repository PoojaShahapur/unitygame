using SDK.Lib;

namespace Game.UI
{
    public class CardControlBase : ControlBase
    {
        public CardControlBase(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public SceneCardBase m_card
        {
            get
            {
                return m_entity as SceneCardBase;
            }
            set
            {
                m_entity = value;
            }
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }
    }
}