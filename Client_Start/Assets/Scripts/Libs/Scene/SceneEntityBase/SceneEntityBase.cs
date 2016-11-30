using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体，定义接口，默认的一些实现放在 BeingEntity 里面
     */
    public class SceneEntityBase : GObject, IDelayHandleItem, IDispatchObject
    {
        protected EntityRenderBase m_render;
        protected bool mIsClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的
        protected MVector3 mWorldPos;   // 世界空间
        protected uint mId;             // 唯一 Id
        protected Area mArea;           // 服务器区域
        protected MDistrict mDistrict;  // 裁剪区域
        protected bool mIsInSceneGraph; // 是否在场景图中，如果不在场景图中，肯定不可见，不管是否在可视范围内
        protected EntityType mEntityType;   // Entity 类型

        public SceneEntityBase()
        {
            mIsClientDispose = false;
            mIsInSceneGraph = true;
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

        virtual public void onInit()
        {

        }

        // 释放接口
        virtual public void dispose()
        {
            
        }

        // 释放的时候回调的接口
        virtual public void onDestroy()
        {
            if (m_render != null)
            {
                m_render.dispose();
                m_render = null;
            }
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

        virtual public void setClientDispose()
        {
            mIsClientDispose = true;
            if(m_render != null)
            {
                m_render.setClientDispose();
            }
        }

        virtual public bool isClientDispose()
        {
            return mIsClientDispose;
        }

        virtual public void onTick(float delta)
        {

        }

        virtual public GameObject gameObject()
        {
            return m_render.selfGo;
        }

        virtual public void setGameObject(GameObject rhv)
        {
            m_render.selfGo = rhv;
        }

        virtual public Transform transform()
        {
            return m_render.transform();
        }

        virtual public void setPnt(GameObject pntGO_)
        {
            m_render.setPntGo(pntGO_);
        }

        virtual public GameObject getPnt()
        {
            return m_render.getPntGo();
        }

        virtual public bool checkRender()
        {
            return m_render.checkRender();
        }

        virtual public float getWorldPosX()
        {
            return mWorldPos.x;
        }

        virtual public float getWorldPosY()
        {
            return mWorldPos.z;
        }

        public MVector3 getWorldPos()
        {
            return mWorldPos;
        }

        public void setArea(Area area)
        {
            mArea = area;
        }

        public void setDistrict(MDistrict district)
        {
            mDistrict = district;
        }

        public void setInSceneGraph(bool value)
        {
            mIsInSceneGraph = value;
        }

        public bool getInSceneGraph()
        {
            return mIsInSceneGraph;
        }

        public void setOriginal(Vector3 original)
        {
            if (null != m_render)
            {
                m_render.setOriginal(original);
            }
        }

        public void setRotation(Quaternion rotation)
        {
            if (null != m_render)
            {
                m_render.setRotation(rotation);
            }
        }

        public void setSelfName(string name)
        {
            if (null != m_render)
            {
                m_render.setSelfName(name);
            }
        }

        public Bounds getBounds()
        {
            Bounds retBounds = new Bounds(Vector3.zero, Vector3.zero);

            if (null != m_render)
            {
                retBounds = m_render.getBounds();
            }

            return retBounds;
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode)
        {
            if (null != m_render)
            {
                m_render.AddRelativeForce(force, mode);
            }
        }

        // 自动管理
        virtual public void autoHandle()
        {

        }

        // 初始化渲染器
        virtual public void initRender()
        {

        }

        virtual public void loadRenderRes()
        {

        }

        public EntityType getEntityType()
        {
            return this.mEntityType;
        }
    }
}