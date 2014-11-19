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
        protected AILocalState m_aiLocalState;

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
            m_aiLocalState = new AILocalState();
        }

        public AILocalState aiLocalState
        {
            get
            {
                return m_aiLocalState;
            }
            set
            {
                m_aiLocalState = value;
            }
        }

        public void OnTick(float delta)
        {
            if (m_behaviorTree != null)
            {
                m_behaviorTree.inputParam.beingEntity = this;
                m_behaviorTree.Behave();
            }
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
        virtual public void addAiByID(string id)
        {
            BehaviorTree behaviorTree = Ctx.m_instance.m_behaviorTreeMgr.getBTByID(id) as BehaviorTree;
            m_vehicle = new Biped();
            m_vehicle.initOwner(m_skinAniModel.rootGo);
            m_vehicle.AllowedMovementAxes = new Vector3(1, 0, 1);
            m_vehicle.MaxSpeed = 10;
            m_vehicle.setSpeed(5);
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