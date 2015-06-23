namespace SDK.Lib
{
    public class ControlBase
    {
        protected SceneEntity m_entity;

        public ControlBase(SceneEntity entity)
        {
            m_entity = entity;
        }

        public void dispose()
        {

        }
    }
}