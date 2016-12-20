namespace SDK.Lib
{
    public class ControlBase
    {
        protected SceneEntityBase mEntity;

        public ControlBase(SceneEntityBase entity)
        {
            mEntity = entity;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }
    }
}