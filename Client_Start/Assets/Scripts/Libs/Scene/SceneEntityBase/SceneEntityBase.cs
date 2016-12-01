using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体，定义接口，逻辑相关的一些实现放在 BeingEntity 里面，例如: 地形， Player， Npc
     */
    public class SceneEntityBase : GObject, IDelayHandleItem, IDispatchObject
    {
        protected EntityRenderBase mRender;
        protected bool mIsClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的
        protected MVector3 mWorldPos;   // 世界空间
        protected uint mId;             // 唯一 Id
        protected Area mArea;           // 服务器区域
        protected MDistrict mDistrict;  // 裁剪区域
        protected bool mIsInSceneGraph; // 是否在场景图中，如果不在场景图中，肯定不可见，不管是否在可视范围内
        protected EntityType mEntityType;   // Entity 类型
        protected string mEntityUniqueId;   // Entity 唯一 Id

        public SceneEntityBase()
        {
            mIsClientDispose = false;
            mIsInSceneGraph = true;
        }

        //public EntityRenderBase render
        //{
        //    get
        //    {
        //        return mRender;
        //    }
        //    set
        //    {
        //        mRender = value;
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
            if (mRender != null)
            {
                mRender.dispose();
                mRender = null;
            }
        }

        virtual public void show()
        {
            if (mRender != null)
            {
                mRender.show();
            }
        }

        virtual public void hide()
        {
            if (mRender != null)
            {
                mRender.hide();
            }
        }

        virtual public bool IsVisible()
        {
            if (mRender != null)
            {
                return mRender.IsVisible();
            }

            return true;
        }

        virtual public void setClientDispose()
        {
            mIsClientDispose = true;
            if(mRender != null)
            {
                mRender.setClientDispose();
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
            return mRender.selfGo;
        }

        virtual public void setGameObject(GameObject rhv)
        {
            mRender.selfGo = rhv;
        }

        virtual public Transform transform()
        {
            if (null != mRender)
            {
                return mRender.transform();
            }

            return null;
        }

        virtual public void setPnt(GameObject pntGO_)
        {
            mRender.setPntGo(pntGO_);
        }

        virtual public GameObject getPnt()
        {
            return mRender.getPntGo();
        }

        virtual public bool checkRender()
        {
            return mRender.checkRender();
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
            if (null != mRender)
            {
                mRender.setOriginal(original);
            }
        }

        public void setRotation(Quaternion rotation)
        {
            if (null != mRender)
            {
                mRender.setRotation(rotation);
            }
        }

        public void setSelfName(string name)
        {
            if (null != mRender)
            {
                mRender.setSelfName(name);
            }
        }

        public Bounds getBounds()
        {
            Bounds retBounds = new Bounds(Vector3.zero, Vector3.zero);

            if (null != mRender)
            {
                retBounds = mRender.getBounds();
            }

            return retBounds;
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (null != mRender)
            {
                mRender.AddRelativeForce(force, mode);
            }
        }

        virtual public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (null != this.mRender)
            {
                this.mRender.AddForce(force, mode);
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

        public UnityEngine.Rigidbody getRigidbody()
        {
            if (null != this.mRender)
            {
                return this.mRender.getRigidbody();
            }

            return null;
        }

        public string getEntityUniqueId()
        {
            return this.mEntityUniqueId;
        }
    }
}