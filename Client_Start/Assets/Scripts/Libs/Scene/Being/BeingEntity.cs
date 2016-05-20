using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 生物 
	 */
    public class BeingEntity : SceneEntityBase
    {
        protected SkinModelSkelAnim m_skinAniModel;      // 模型数据

        protected float speed = 0;
        protected float direction = 0;

        public BeingEntity()
        {
            //m_skinAniModel = new SkinModelSkelAnim();
            //m_skinAniModel.handleCB = onSkeletonLoaded;
        }

        public SkinModelSkelAnim skinAniModel
        {
            get
            {
                return m_skinAniModel;
            }
        }

        public void setLocalPos(Vector3 pos)
        {
            //UtilApi.setPos(m_skinAniModel.transform, pos);
        }

        override public void onTick(float delta)
        {
            
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
	}
}