using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的实体，定义接口，逻辑相关的一些实现放在 BeingEntity 里面，例如: 地形， Player， Npc
     */
    public class SceneEntityBase : GObject, IDelayHandleItem, IDispatchObject, ITickedObject, IPriorityObject
    {
        protected EntityRenderBase mRender;
        protected bool mIsClientDispose;        // 客户端已经释放这个对象，但是由于在遍历中，等着遍历结束再删除，所有多这个对象的操作都是无效的
        protected MVector3 mWorldPos;   // 世界空间
        protected Area mArea;           // 服务器区域
        protected TDTile mTDTile;       // 屏区域
        protected MDistrict mDistrict;  // 裁剪区域
        protected bool mIsInSceneGraph; // 是否在场景图中，如果不在场景图中，肯定不可见，不管是否在可视范围内
        protected EntityType mEntityType;   // Entity 类型
        protected string mEntityUniqueId;   // Entity 唯一 Id

        protected KBEngine.Entity mEntity_KBE;  // KBE 引擎的实体

        protected UnityEngine.Vector3 mPos;         // 当前位置信息
        protected UnityEngine.Quaternion mRotate;   // 当前方向信息
        protected UnityEngine.Vector3 mScale;         // 当前缩放信息

        protected bool mIsVisible;          // 是否可见，逻辑数据是否可见
        protected bool mEnableVisible;      // 主要是客户端主动设置是否可见
        protected bool mIsInScreenRange;    // 是否在屏幕范围，主要是是否真正的显示可见
        protected uint mThisId;             // 唯一 Id    

        public SceneEntityBase()
        {
            this.mRender = null;
            this.mEntity_KBE = null;

            this.mIsClientDispose = false;
            this.mIsInSceneGraph = true;

            this.mPos = Vector3.zero;
            this.mRotate = new Quaternion(0, 0, 0, 1);
            this.mScale = Vector3.one;

            this.mIsVisible = true;         // 当前逻辑是否可见
            this.mEnableVisible = true;     // 是否允许可见
            this.mIsInScreenRange = false;  // 屏幕是否可见
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

        // 这个接口调用之前，一定要先设置 ThisId ，调用 setThisId，必须先设置这个
        virtual public void init()
        {
            this.onInit();
        }

        virtual protected void onPreInit()
        {

        }

        virtual protected void onExecInit()
        {

        }

        virtual protected void onPostInit()
        {

        }

        virtual public void onInit()
        {
            this.onPreInit();
            this.onExecInit();
            this.onPostInit();
        }

        // 释放接口
        virtual public void dispose()
        {
            
        }

        // 释放的时候回调的接口
        virtual public void onDestroy()
        {
            if (null != this.mRender)
            {
                this.mRender.dispose();
                this.mRender = null;
            }

            if(null != this.mEntity_KBE)
            {
                this.mEntity_KBE.setEntity_SDK(null);
            }

            this.setClientDispose(true);
        }

        public void setThisId(uint thisId)
        {
            this.mThisId = (uint)thisId;
        }

        public uint getThisId()
        {
            // this.mThisId 这个需要单独保存，不要从 mEntity_KBE 获取，因为只有 PlayerMain 才有 Avatar , 其它 PlayerOther 是没有 Avatar 的
            return this.mThisId;
        }

        virtual public void show()
        {
            if (!this.mIsVisible)
            {
                if (this.mEnableVisible)
                {
                    this.mIsVisible = true;
                    //this.mIsInScreenRange = true;   // 显示不一定在 Screen 可见

                    if (null != this.mRender)
                    {
                        this.mRender.show();
                    }
                }
            }
        }

        virtual public void hide()
        {
            if (this.mIsVisible)
            {
                this.mIsVisible = false;
                this.mIsInScreenRange = false;  // 逻辑隐藏，直接设定不在屏幕范围内

                if (null != this.mRender)
                {
                    this.mRender.hide();
                }
            }
        }

        virtual public void forceShow()
        {
            if (!this.mIsVisible)
            {
                if (this.mEnableVisible)
                {
                    this.mIsVisible = true;

                    if (null != this.mRender)
                    {
                        this.mRender.forceShow();
                    }
                }
            }
        }

        virtual public void forceHide()
        {
            if (this.mIsVisible)
            {
                this.mIsVisible = false;

                if (null != this.mRender)
                {
                    this.mRender.forceHide();
                }
            }
        }

        virtual public bool IsVisible()
        {
            return this.mIsVisible;
        }

        virtual public void setEnableVisible(bool visible)
        {
            this.mEnableVisible = visible;

            if (!this.mEnableVisible && this.mIsVisible)
            {
                this.hide();
            }
            else if(this.mEnableVisible && !this.mIsVisible)
            {
                this.show();
            }
        }

        virtual public bool isEnableVisible()
        {
            return this.mEnableVisible;
        }

        public void setInScreenRange(bool value)
        {
            this.mIsInScreenRange = value;
        }

        public bool isInScreenRange()
        {
            return this.mIsInScreenRange;
        }

        // 进入可见
        public void onEnterScreenRange()
        {
            if (!this.mIsInScreenRange)
            {
                this.mIsInScreenRange = true;

                if (null != this.mRender)
                {
                    this.mRender.onEnterScreenRange();
                }
            }
        }

        // 离开可见
        public void onLeaveScreenRange()
        {
            if (this.mIsInScreenRange)
            {
                this.mIsInScreenRange = false;

                if (null != this.mRender)
                {
                    this.mRender.onLeaveScreenRange();
                }
            }
        }

        virtual public void setClientDispose(bool isDispose)
        {
            this.mIsClientDispose = isDispose;

            if(null != this.mRender)
            {
                this.mRender.setClientDispose(isDispose);
            }
        }

        virtual public bool isClientDispose()
        {
            return this.mIsClientDispose;
        }

        public UnityEngine.GameObject getGameObject()
        {
            if(null != this.mRender)
            {
                return this.mRender.gameObject();
            }

            return null;
        }

        // 每一帧执行
        virtual public void onTick(float delta)
        {
            this.onPreTick(delta);
            this.onExecTick(delta);
            this.onPostTick(delta);
            if(null != this.mRender) this.mRender.onTick(delta);
        }

        // Tick 第一阶段执行
        virtual protected void onPreTick(float delta)
        {

        }

        virtual protected void onExecTick(float delta)
        {

        }

        // Tick 第二阶段执行
        virtual protected void onPostTick(float delta)
        {

        }

        virtual public GameObject gameObject()
        {
            if (null != this.mRender)
            {
                return this.mRender.selfGo;
            }

            return null;
        }

        virtual public void setGameObject(GameObject rhv)
        {
            if (null != this.mRender)
            {
                this.mRender.selfGo = rhv;
            }
        }

        virtual public Transform transform()
        {
            if (null != this.mRender)
            {
                return this.mRender.transform();
            }

            return null;
        }

        virtual public void setPnt(GameObject pntGO_)
        {
            if (null != this.mRender)
            {
                this.mRender.setPntGo(pntGO_);
            }
        }

        virtual public GameObject getPnt()
        {
            if (null != this.mRender)
            {
                return this.mRender.getPntGo();
            }

            return null;
        }

        virtual public bool checkRender()
        {
            if (null != this.mRender)
            {
                return this.mRender.checkRender();
            }

            return false;
        }

        virtual public float getWorldPosX()
        {
            return this.mWorldPos.x;
        }

        virtual public float getWorldPosY()
        {
            return this.mWorldPos.z;
        }

        public MVector3 getWorldPos()
        {
            return this.mWorldPos;
        }

        public void setArea(Area area)
        {
            this.mArea = area;
        }

        public void setTile(TDTile tile)
        {
            this.mTDTile = tile;
        }

        public TDTile getTile()
        {
            return this.mTDTile;
        }

        public void setDistrict(MDistrict district)
        {
            this.mDistrict = district;
        }

        public void setInSceneGraph(bool value)
        {
            this.mIsInSceneGraph = value;
        }

        public bool getInSceneGraph()
        {
            return this.mIsInSceneGraph;
        }

        // 从 KBE 设置位置，必须要从这个接口设置
        public void setPos_FromKBE(Vector3 pos)
        {
            pos = UtilApi.convPosByMode(pos);

            this.setPos(pos);
        }
        
        virtual public void setPos(Vector3 pos)
        {
            if (!UtilMath.isEqualVec3(this.mPos, pos))
            {
                pos = Ctx.mInstance.mSceneSys.adjustPosInRange(this, pos);

                this.mPos = pos;

                if (null != this.mRender)
                {
                    this.mRender.setPos(pos);
                }

                if (MacroDef.ENABLE_SCENE2D_CLIP)
                {
                    Ctx.mInstance.mTileMgr.updateEntity(this);
                }

                if(EntityType.ePlayerMain == this.mEntityType ||
                   EntityType.ePlayerMainChild == this.mEntityType)
                {
                    Ctx.mInstance.mPlayerMgr.setIsMainPosOrOrientChanged(true);
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setPos, BasicInfo is {0}, X = {1}, Y = {2}, Z = {3}", this.getBasicInfoStr(), this.mPos.x, this.mPos.y, this.mPos.z), LogTypeId.eLogBeingMove);
                }
            }
        }

        virtual public void setRenderPos(Vector3 pos)
        {
            if (!UtilApi.isInFakePos(pos) && !UtilMath.isEqualVec3(this.mPos, pos))
            {
                this.mPos = pos;

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setRenderPos, BasicInfo is {0}, mPosX = {1}, mPosY = {2}, mPosZ = {3}", this.getBasicInfoStr(), this.mPos.x, this.mPos.y, this.mPos.z), LogTypeId.eLogBeingMove);
                }
            }
        }

        public UnityEngine.Vector3 getPos()
        {
            return this.mPos;
        }

        public void setRotation(Quaternion rotation)
        {
            if (!UtilMath.isEqualQuat(this.mRotate, rotation))
            {
                this.mRotate = rotation;

                if (null != this.mRender)
                {
                    this.mRender.setRotate(rotation);
                }

                if (EntityType.ePlayerMain == this.mEntityType ||
                   EntityType.ePlayerMainChild == this.mEntityType)
                {
                    Ctx.mInstance.mPlayerMgr.setIsMainPosOrOrientChanged(true);
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setRotation, BasicInfo is {0}, X = {1}, Y = {2}, Z = {3}, W = {4}", this.getBasicInfoStr(), this.mRotate.x, this.mRotate.y, this.mRotate.z, this.mRotate.w), LogTypeId.eLogBeingMove);
                }
            }
        }

        public void setRotateEulerAngle_FromKBE(UnityEngine.Vector3 rotation)
        {
            rotation = UtilApi.convRotByMode(rotation);
            this.setRotateEulerAngle(rotation);
        }

        public void setRotateEulerAngle(UnityEngine.Vector3 rotation)
        {
            if (!UtilMath.isEqualVec3(this.mRotate.eulerAngles, rotation))
            {
                // 只能绕 Y 轴旋转
                if (MacroDef.XZ_MODE)
                {
                    rotation.x = 0;
                    rotation.z = 0;
                }
                else if (MacroDef.XY_MODE)
                {
                    // 只能绕 Z 轴旋转
                    rotation.x = 0;
                    rotation.y = 0;
                }

                this.mRotate = Quaternion.Euler(rotation);

                if (null != this.mRender)
                {
                    this.mRender.setRotate(this.mRotate);
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setRotateEulerAngle, BasicInfo is {0}, X = {1}, Y = {2}, Z = {3}, W = {4}", this.getBasicInfoStr(), this.mRotate.x, this.mRotate.y, this.mRotate.z, this.mRotate.w), LogTypeId.eLogBeingMove);
                }
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
            if (!UtilMath.isEqualVec3(this.mScale, value))
            {
                this.mScale = value;

                if (null != this.mRender)
                {
                    this.mRender.setScale(this.mScale);
                }

                if (MacroDef.ENABLE_LOG)
                {
                    Ctx.mInstance.mLogSys.log(string.Format("BeingEntity::setScale, BasicInfo is {0}, X = {1}, Y = {2}, Z = {3}", this.getBasicInfoStr(), this.mScale.x, this.mScale.y, this.mScale.z), LogTypeId.eLogBeingMove);
                }
            }
        }

        public void setSelfName(string name)
        {
            if (null != this.mRender)
            {
                this.mRender.setSelfName(name);
            }
        }

        public Bounds getBounds()
        {
            Bounds retBounds = new Bounds(Vector3.zero, Vector3.zero);

            if (null != this.mRender)
            {
                retBounds = this.mRender.getBounds();
            }

            return retBounds;
        }

        public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (null != this.mRender)
            {
                this.mRender.AddRelativeForce(force, mode);
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
            this.mEntity_KBE = entity;
        }

        //public void updateTransform()
        //{
        //    if (null != this.mEntity_KBE)
        //    {
        //        this.setPos(new Vector3(mEntity_KBE.position.x, mEntity_KBE.position.y, mEntity_KBE.position.z));
        //        this.setRotation(Quaternion.Euler(new Vector3(mEntity_KBE.direction.y, mEntity_KBE.direction.z, mEntity_KBE.direction.x)));
        //    }
        //}

        public KBEngine.Entity getEntity()
        {
            return this.mEntity_KBE;
        }

        public void baseCall(string methodname, params object[] arguments)
        {
            if (null != this.mEntity_KBE)
            {
                this.mEntity_KBE.baseCall(methodname, arguments);
            }
        }

        public void cellCall(string methodname, params object[] arguments)
        {
            if (null != this.mEntity_KBE)
            {
                this.mEntity_KBE.cellCall(methodname, arguments);
            }
        }

        // 获取基本信息字符串
        protected string getBasicInfoStr()
        {
            return string.Format("ThisId = {0}, TypeId = {1}", this.getThisId(), this.getTypeId());
        }
    }
}