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
        protected AnimFSM m_animFSM;                // 动画状态机
        protected AILocalState m_aiLocalState;

        protected string m_btID;

        protected float speed = 0;
        protected float direction = 0;

        public BeingEntity()
        {
            m_skinAniModel = new SkinAniModel();
            m_skinAniModel.handleCB = onSkeletonLoaded;
            m_aiLocalState = new AILocalState();
        }

        public SkinAniModel skinAniModel
        {
            get
            {
                return m_skinAniModel;
            }
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

        public void setLocalPos(Vector3 pos)
        {
            m_skinAniModel.transform.localPosition = pos;
        }

        public void OnTick(float delta)
        {
            // 更新一些 ai
            if (m_aiController != null)
            {
                m_aiController.OnTick(delta);
            }
            // 更新行为
            if (m_behaviorTree != null)
            {
                m_behaviorTree.inputParam.beingEntity = this;
                m_behaviorTree.Behave();
            }
            if(m_animFSM != null)
            {
                m_animFSM.UpdateFSM();
            }
        }

        // 添加 AI
        virtual public void addAiByID(string id)
        {
            m_btID = id;
            initAi(m_btID);
        }

        protected void initAi(string id)
        {
            if (!string.IsNullOrEmpty(id) && m_behaviorTree == null && m_skinAniModel.rootGo != null)
            {
                // 生成行为树
                BehaviorTree behaviorTree = Ctx.m_instance.m_aiSystem.getBehaviorTreeMgr().getBTByID(id) as BehaviorTree;
                m_behaviorTree = behaviorTree;
                // 生成 ai 控制器
                if (m_aiController == null)
                {
                    m_aiController = new AIController();
                    m_aiController.initControl(m_skinAniModel);
                }

                m_aiController.vehicle.sceneGo = m_skinAniModel.rootGo;
                // 初始化 Steerings 参数信息
                initSteerings();
            }
        }

        protected void initAnimFSM()
        {
            if (m_animFSM == null)
            {
                m_animFSM = new AnimFSM();
                m_animFSM.beingEntity = this;
                m_animFSM.InitFSM();
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

        public void setPartModel(int modelDef, string assetBundleName, string partName)
        {
            m_skinAniModel.m_modelList[modelDef].m_bundleName = assetBundleName;
            m_skinAniModel.m_modelList[modelDef].m_partName = partName;
            m_skinAniModel.loadPartModel(modelDef);
        }

        public virtual void onSkeletonLoaded()
        {
            initAi(m_btID);
            initAnimFSM();
        }

        // 目前只有怪有 Steerings ,加载这里是为了测试，全部都有 Steerings
        virtual protected void initSteerings()
        {

        }
	}
}