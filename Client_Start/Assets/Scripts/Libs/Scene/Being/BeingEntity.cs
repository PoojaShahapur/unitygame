using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 生物 Entity，有感知，可以交互的
	 */
    public class BeingEntity : SceneEntityBase
    {
        protected SkinModelSkelAnim m_skinAniModel;      // 模型数据

        public float mMoveSpeed;     // 移动速度
        public float mRotateSpeed;   // 旋转速度
        public float mScaleSpeed;    // 缩放速度

        public BeingEntity()
        {
            //m_skinAniModel = new SkinModelSkelAnim();
            //m_skinAniModel.handleCB = onSkeletonLoaded;

            mMoveSpeed = 1;
            mRotateSpeed = 10;
            mScaleSpeed = 1;
        }

        public SkinModelSkelAnim skinAniModel
        {
            get
            {
                return m_skinAniModel;
            }
        }

        // 骨骼设置，骨骼不能更换
        public void setSkeleton(string name)
        {
            //if(string.IsNullOrEmpty(m_skinAniModel.m_skeletonName))
            //{
            //    m_skinAniModel.m_skeletonName = name;
            //    m_skinAniModel.loadSkeleton();
            //}
        }

        public void setPartModel(int modelDef, string assetBundleName, string partName)
        {
            //m_skinAniModel.m_modelList[modelDef].m_bundleName = string.Format("{0}{1}", assetBundleName, ".prefab");
            //m_skinAniModel.m_modelList[modelDef].m_partName = partName;
            //m_skinAniModel.loadPartModel(modelDef);
        }

        public virtual void onSkeletonLoaded()
        {
            
        }

        // 目前只有怪有 Steerings ,加载这里是为了测试，全部都有 Steerings
        virtual protected void initSteerings()
        {

        }

        virtual public string getDesc()
        {
            return "";
        }

        public BeingBehaviorControl behaviorControl
        {
            get
            {
                return getBeingBehaviorControl();
            }
        }

        virtual public BeingBehaviorControl getBeingBehaviorControl()
        {
            return null;
        }

        public EffectControl effectControl
        {
            get
            {
                return getEffectControl();
            }
        }

        virtual public EffectControl getEffectControl()
        {
            return null;
        }

        public uint qwThisID
        {
            get
            {
                return 0;
            }
        }

        public void playFlyNum(int num)
        {

        }

        public void setMoveSpeed(float value)
        {
            this.mMoveSpeed = value;
        }

        public void setRotateSpeed(float value)
        {
            this.mRotateSpeed = value;
        }

        public void setScaleSpeed(float value)
        {
            this.mScaleSpeed = value;
        }

        public void setDestPos(UnityEngine.Vector3 pos)
        {
            if(null != mMovement)
            {
                (mMovement as BeingEntityMovement).moveToPos(pos);
            }
        }

        public void setDestRotate(UnityEngine.Vector3 pos)
        {
            if (null != mMovement)
            {
                (mMovement as BeingEntityMovement).moveToPos(pos);
            }
        }

        override public void init()
        {
            // 基类初始化
            base.init();
            // 自动处理，例如添加到管理器
            this.autoHandle();
            // 初始化渲染器
            this.initRender();
            // 加载渲染器资源
            this.loadRenderRes();
            // 更新位置
            this.updateTransform();
        }

        override public void loadRenderRes()
        {
            mRender.load();
        }

        override public void onTick(float delta)
        {
            base.onTick(delta);
        }
    }
}