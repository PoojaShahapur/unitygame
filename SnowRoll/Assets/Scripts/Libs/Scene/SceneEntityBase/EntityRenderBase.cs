namespace SDK.Lib
{
    /**
     * @brief 基本的渲染器，所有与显示有关的接口都在这里，这里基本只提供接口，最基本的实现在 BeingEntityRender 里面
     */
    public class EntityRenderBase : AuxComponent
    {
        protected SceneEntityBase mEntity;  // Entity 数据
        protected UnityEngine.Rigidbody mRigidbody;     // 包含的刚体

        public EntityRenderBase(SceneEntityBase entity_)
        {
            mEntity = entity_;
        }

        virtual public void setClientDispose(bool isDispose)
        {

        }

        virtual public bool isClientDispose()
        {
            return mEntity.isClientDispose();
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
            base.dispose();
        }

        // 资源释放事件，仅仅是释放基本的资源，不修改销毁流程
        override public void onDestroy()
        {
            base.onDestroy();
        }

        virtual public bool checkRender()
        {
            return false;
        }

        virtual public void load()
        {

        }

        // 场景对象不需要设置，因为如果设置了就检测不了隐藏显示了
        override public void show()
        {
            
        }

        override public void hide()
        {
            
        }

        public bool isValid()
        {
            return null != this.mSelfGo;
        }

        public UnityEngine.Vector3 getPos()
        {
            if(this.isValid())
            {
                return this.transform().localPosition;
            }

            return UnityEngine.Vector3.zero;
        }

        // 资源加载完成，初始化一些基本资源
        override protected void onSelfChanged()
        {
            base.onSelfChanged();

            this.mRigidbody = UtilApi.getComByP<UnityEngine.Rigidbody>(this.selfGo);

            // 设置可视化
            if (this.mEntity.IsVisible())
            {
                this.show();
            }
            else
            {
                this.hide();
            }
            // 设置方向位置信息
            this.setPos(this.mEntity.getPos());
            this.setRotate(this.mEntity.getRotate());
            this.setScale(this.mEntity.getScale());
        }

        override public void updateLocalTransform()
        {
            if (null != this.mSelfGo)
            {
                if (this.mIsPosDirty)
                {
                    this.mIsPosDirty = false;

                    if (null != this.mRigidbody)
                    {
                        //if (!(this.mEntity as BeingEntity).isFreezeXZ())
                        //{
                            UtilApi.setRigidbodyPos(this.mRigidbody, this.mEntity.getPos());
                        //}
                        //else
                        //{
                        //    UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                        //}
                    }
                    else
                    {
                        UtilApi.setPos(this.mSelfGo.transform, this.mEntity.getPos());
                    }
                }
                if (this.mIsRotDirty)
                {
                    this.mIsRotDirty = false;

                    //if (null != this.mRigidbody)
                    //{
                    //    if (!(this.mEntity as BeingEntity).isFreezeXZ())
                    //    {
                    //        UtilApi.setRigidbodyRot(this.mRigidbody, this.mEntity.getRotate());
                    //    }
                    //    else
                    //    {
                    //        UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                    //    }
                    //}
                    //else
                    //{
                    //    UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                    //}

                    UtilApi.setRot(this.mSelfGo.transform, this.mEntity.getRotate());
                }
                if (this.mIsScaleDirty)
                {
                    this.mIsScaleDirty = false;

                    UtilApi.setScale(this.mSelfGo.transform, this.mEntity.getScale());
                }
            }
        }

        virtual public void setTexture(string path)
        {

        }
    }
}