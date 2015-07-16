namespace SDK.Lib
{
    public class ControlBase
    {
        protected SceneEntity m_entity;

        public ControlBase(SceneEntity entity)
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