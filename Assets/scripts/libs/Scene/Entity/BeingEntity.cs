using BehaviorLibrary;
using SDK.Common;
using UnityEngine;
using UnitySteer.Behaviors;

namespace SDK.Lib
{
	/**
	 * @brief 生物 
	 */
    public class BeingEntity : ITickedObject
	{
        protected SkinAniModel m_skinAniModel;             // 一个数组

        // AI 数据
        protected Biped m_vehicle;
        protected BehaviorTree m_behaviorTree;

        protected float speed = 0;
        protected float direction = 0;

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
        }

        public void OnTick(float delta)
        {
            
        }

        public Biped vehicle
        {
            get
            {
                return m_vehicle;
            }
            set
            {
                m_vehicle = value;
            }
        }

        // 添加 AI
        virtual public void addAi(BehaviorTree behaviorTree)
        {
            m_vehicle = new Biped();
            m_behaviorTree = behaviorTree;
        }

        // 骨骼设置，骨骼不能更换
        public void setSkeleton(string name)
        {
            if(string.IsNullOrEmpty(m_skinAniModel.m_skeletonName))
            {
                m_skinAniModel.m_skeletonName = name;
                m_skinAniModel.loadSkeleton();
            }
        }

        public void setPartModel(PlayerModelDef modelDef, string assetBundleName, string partName)
        {
            m_skinAniModel.m_modelList[(int)modelDef].m_bundleName = assetBundleName;
            m_skinAniModel.m_modelList[(int)modelDef].m_partName = partName;
            m_skinAniModel.loadPartModel(modelDef);
        }
	}
}