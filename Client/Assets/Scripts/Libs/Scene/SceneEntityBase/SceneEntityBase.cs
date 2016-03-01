using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体，没有什么功能，就是基本循环
     */
    public class SceneEntityBase : IDelayHandleItem, IDispatchObject
    {
        protected EntityRenderBase m_render;
        protected bool m_bClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的

        public SceneEntityBase()
        {
            m_bClientDispose = false;
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

        virtual public void init()
        {

        }

        virtual public void show()
        {
            if (m_render != null)
            {
                m_render.show();
            }
        }

        virtual public void hide()
        {
            if (m_render != null)
            {
                m_render.hide();
            }
        }

        virtual public bool IsVisible()
        {
            if (m_render != null)
            {
                return m_render.IsVisible();
            }

            return true;
        }

        virtual public void dispose()
        {
            if(m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }
        }

        virtual public void setClientDispose()
        {
            m_bClientDispose = true;
            if(m_render != null)
            {
                m_render.setClientDispose();
            }
        }

        virtual public bool getClientDispose()
        {
            return m_bClientDispose;
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

        virtual public void setPnt(GameObject pntGO_)
        {

        }

        virtual public GameObject getPnt()
        {
            return m_render.getPnt();
        }

        virtual public bool checkRender()
        {
            return m_render.checkRender();
        }

        virtual public float getWorldPosX()
        {
            return 0;
        }

        virtual public float getWorldPosY()
        {
            return 0;
        }
    }
}