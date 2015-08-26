using SDK.Lib;

namespace FightCore
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

        override public void init()
        {

        }

        override public void dispose()
        {

        }
    }
}