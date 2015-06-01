namespace Game.UI
{
    public class ControlBase
    {
        protected SceneCardBase m_card;

        public ControlBase(SceneCardBase rhv)
        {
            m_card = rhv;
        }

        public SceneCardBase card
        {
            get
            {
                return m_card;
            }
            set
            {
                m_card = value;
            }
        }

        virtual public void init()
        {

        }
    }
}