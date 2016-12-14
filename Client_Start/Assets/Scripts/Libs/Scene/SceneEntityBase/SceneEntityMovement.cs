namespace SDK.Lib
{
    /**
     * @brief 移动类型
     */
    public class SceneEntityMovement : GObject, IDispatchObject
    {
        protected SceneEntityBase mEntity;          // 关联的实体

        public SceneEntityMovement(SceneEntityBase entity)
        {
            mTypeId = "SceneEntityMovement";
            mEntity = entity;
        }

        virtual public void init()
        {

        }

        virtual public void dispose()
        {

        }

        virtual public void onTick(float delta)
        {

        }
    }
}