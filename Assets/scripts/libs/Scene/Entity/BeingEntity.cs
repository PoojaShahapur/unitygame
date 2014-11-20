using BehaviorLibrary;
using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
	/**
	 * @brief 生物 
	 */
    public class BeingEntity : ITickedObject
	{
        protected SkinAniModel m_skinAniModel;      // 模型数据
        protected BehaviorTree m_behaviorTree;      // 行为树
        protected AIController m_aiController;      // ai 控制

        protected float speed = 0;
        protected float direction = 0;

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
        }

        public AIController aiController
        {
            get
            {
                return m_aiController;
            }
            set
            {
                m_aiController = value;
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

        // 添加 AI
        virtual public void addAiByID(string id)
        {
            BehaviorTree behaviorTree = Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().getBTByID(id) as BehaviorTree;
            m_behaviorTree = behaviorTree;
            if(m_aiController == null)
            {
                m_aiController = new AIController();
                m_aiController.initControl(m_skinAniModel);
            }
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