using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 基本的渲染器，所有与显示有关的接口都在这里
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

        virtual public Transform transform()
        {
            return null;
        }

        virtual public void onTick(float delta)
        {
            
        }

        // 初始化
        override public void init()
        {

        }

        // 初始化事件
        //virtual public void onInit()
        //{

        //}

        // 销毁
        override public void dispose()
        {
            
        }

        // 资源释放事件
        //virtual public void onDestroy()
        //{

        //}

        override public void setPntGo(GameObject pntGO_)
        {

        }

        virtual public GameObject getPntGo()
        {
            return null;
        }

        virtual public bool checkRender()
        {
            return false;
        }
    }
}