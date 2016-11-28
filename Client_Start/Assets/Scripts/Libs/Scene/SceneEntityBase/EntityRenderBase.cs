namespace SDK.Lib
{
    /**
     * @brief 基本的渲染器，所有与显示有关的接口都在这里，这里基本只提供接口，最基本的实现在 BeingEntityRender 里面
     */
    public class EntityRenderBase : AuxComponent
    {
        protected SceneEntityBase mEntity;  // Entity 数据

        public EntityRenderBase(SceneEntityBase entity_)
        {
            mEntity = entity_;
        }

        virtual public void setClientDispose()
        {

        }

        virtual public bool getClientDispose()
        {
            return mEntity.getClientDispose();
        }

        virtual public void onTick(float delta)
        {
            
        }

        // 初始化流程
        override public void init()
        {
            this.onInit();
        }

        // 初始化事件，仅仅是变量初始化，初始化流程不修改
        virtual public void onInit()
        {

        }

        // 销毁流程
        override public void dispose()
        {
            this.onDestroy();
        }

        // 资源释放事件，仅仅是释放基本的资源，不修改销毁流程
        virtual public void onDestroy()
        {

        }

        virtual public bool checkRender()
        {
            return false;
        }

        virtual public void load()
        {

        }
    }
}