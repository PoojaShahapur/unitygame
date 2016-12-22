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

        protected KBEngine.Entity mEntity_KBE;  // KBE 引擎的实体

        protected UnityEngine.Vector3 mPos;         // 当前位置信息
        protected UnityEngine.Quaternion mRotate;   // 当前方向信息
        protected UnityEngine.Vector3 mScale;         // 当前缩放信息
        public SceneEntityMovement mMovement;    // 移动组件

        protected bool mIsVisible;          // 是否可见

        public SceneEntityBase()
        {
            this.mIsClientDispose = false;
            this.mIsInSceneGraph = true;

            this.mPos = Vector3.zero;
            this.mRotate = new Quaternion(0, 0, 0, 1);
            this.mScale = Vector3.one;

            this.mIsVisible = true;
        }

        // 不允许直接访问 mRender ，必须通过接口访问
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
            this.preInit();
            this.postInit();
        }

        virtual public void preInit()
        {

        }

        virtual public void postInit()
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
            if (null != mRender)
            {
                mRender.dispose();
                mRender = null;
            }
        }

        public void setThisId(uint thisId)
        {
            mId = thisId;
        }

        public uint getThisId()
        {
            return mId;
        }

        virtual public void show()
        {
            this.mIsVisible = true;

            if (null != mRender)
            {
                mRender.show();
            }
        }

        virtual public void hide()
        {
            this.mIsVisible = false;

            if (null != mRender)
            {
                mRender.hide();
            }
        }

        virtual public bool IsVisible()
        {
            return this.mIsVisible;
        }

        virtual public void setClientDispose()
        {
            this.mIsClientDispose = true;

            if(null != mRender)
            {
                mRender.setClientDispose();
            }
        }

        virtual public bool isClientDispose()
        {
            return mIsClientDispose;
        }

        // 每一帧执行
        virtual public void onTick(float delta)
        {
            this.onPreTick(delta);
            this.onPostTick(delta);
        }

        // Tick 第一阶段执行
        virtual public void onPreTick(float delta)
        {

        }

        // Tick 第二阶段执行
        virtual public void onPostTick(float delta)
        {

        }

        virtual public GameObject gameObject()
        {
            if (null != mRender)
            {
                return mRender.selfGo;
            }

            return null;
        }

        virtual public void setGameObject(GameObject rhv)
        {
            if (null != mRender)
            {
                mRender.selfGo = rhv;
            }
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
            if (null != mRender)
            {
                mRender.setPntGo(pntGO_);
            }
        }

        virtual public GameObject getPnt()
        {
            if (null != mRender)
            {
                return mRender.getPntGo();
            }

            return null;
        }

        virtual public bool checkRender()
        {
            if (null != mRender)
            {
                return mRender.checkRender();
            }

            return false;
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

        virtual public void setPos(Vector3 pos)
        {
            if (!UtilMath.isEqualVec3(this.mPos, pos))
            {
                this.mPos = pos;
                this.mPos.y = 1.3f;     // TODO: 先固定

                if (null != mRender)
                {
                    mRender.setPos(pos);
                }
            }
        }

        public UnityEngine.Vector3 getPos()
        {
            return this.mPos;
        }

        public void setRotation(Quaternion rotation)
        {
            this.mRotate = rotation;

            if (null != mRender)
            {
                mRender.setRotate(rotation);
            }
        }

        public void setRotateEulerAngle(UnityEngine.Vector3 rotation)
        {
            this.mRotate = Quaternion.Euler(rotation);

            if (null != mRender)
            {
                mRender.setRotate(this.mRotate);
            }
        }

        // 获取前向向量
        public UnityEngine.Vector3 getForward()
        {
            UnityEngine.Vector3 forward = this.mRotate * UnityEngine.Vector3.forward;

            return forward;
        }

        public UnityEngine.Quaternion getRotate()
        {
            return this.mRotate;
        }

        public UnityEngine.Vector3 getRotateEulerAngle()
        {
            return this.mRotate.eulerAngles;
        }

        public Vector3 getScale()
        {
            return this.mScale;
        }

        public void setScale(UnityEngine.Vector3 value)
        {
            this.mScale = value;

            if (null != mRender)
            {
                mRender.setScale(this.mScale);
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

        public void setEntity_KBE(KBEngine.Entity entity)
        {
            mEntity_KBE = entity;
        }

        public void updateTransform()
        {
            if (null != this.mEntity_KBE)
            {
                this.setPos(new Vector3(mEntity_KBE.position.x, 1.3f, mEntity_KBE.position.z));
                this.setRotation(Quaternion.Euler(new Vector3(mEntity_KBE.direction.y, mEntity_KBE.direction.z, mEntity_KBE.direction.x)));
            }
        }

        public System.Int32 getId()
        {
            return mEntity_KBE.id;
        }

        public KBEngine.Entity getEntity()
        {
            return mEntity_KBE;
        }

        public void baseCall(string methodname, params object[] arguments)
        {
            if (null != mEntity_KBE)
            {
                mEntity_KBE.baseCall(methodname, arguments);
            }
        }

        public void cellCall(string methodname, params object[] arguments)
        {
            if (null != this.mEntity_KBE)
            {
                mEntity_KBE.cellCall(methodname, arguments);
            }
        }
    }
}