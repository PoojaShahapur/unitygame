namespace SDK.Lib
{
    public class ControlBase
    {
        protected SceneEntityBase m_entity;

        public ControlBase(SceneEntityBase entity)
        {
            m_entity = entity;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }
    }
}