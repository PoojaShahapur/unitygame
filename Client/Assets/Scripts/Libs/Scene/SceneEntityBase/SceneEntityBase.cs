using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体，没有什么功能，就是基本循环
     */
    public class SceneEntityBase : IDelayHandleItem
    {
        protected EntityRenderBase m_render;

        public SceneEntityBase()
        {

        }

        //public EntityRenderBase render
        //{
        //    get
        //    {
        //        return m_render;
        //    }
        //    set
        //    {
        //        m_render = value;
        //    }
        //}

        virtual public void dispose()
        {
            if(m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }
        }

        virtual public void onTick(float delta)
        {

        }

        virtual public GameObject gameObject()
        {
            return m_render.gameObject();
        }

        virtual public void setGameObject(GameObject rhv)
        {
            m_render.setGameObject(rhv);
        }

        virtual public Transform transform()
        {
            return m_render.transform();
        }
    }
}