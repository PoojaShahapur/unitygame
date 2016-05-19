using SDK.Lib;

namespace BehaviorLibrary
{
    /**
     * @brief 凡是 BehaviorTree 之外的 AI 逻辑都写在这里
     */
    public class AIController : SceneEntityBase
    {
        protected SceneEntityBase m_entity;        // 控制的场景 Entity
        protected BTID m_btID;          // 行为树 ID 
        protected BehaviorTreeRes m_btRes;        // 行为树资源
        protected BehaviorTree m_bt;              // 行为树
        protected bool m_bNeedReloadBT;

        public AIController()
        {
            m_bNeedReloadBT = false;
        }

        public void initControl(SkinModelSkelAnim skinAniModel)
        {
            
        }

        public BTID btID
        {
            get
            {
                return m_btID;
            }
            set
            {
                if (m_btID != value)
                {
                    m_bNeedReloadBT = true;
                }
                m_btID = value;
                syncUpdateBT();
            }
        }

        override public void dispose()
        {
            if(m_btRes != null)
            {
                Ctx.m_instance.m_aiSystem.behaviorTreeMgr.unload(m_btRes.getResUniqueId(), null);
                m_btRes = null;
            }

            if(m_bt != null)
            {
                m_bt = null;
            }

            Ctx.m_instance.m_aiSystem.aiControllerMgr.removeController(this);
        }

        override public void onTick(float delta)
        {
            //if(m_radar != null)
            //{
            //    m_radar.UpdateRadar();      // 更新雷达数据
            //}

            // 初始化黑盒数据
            m_bt.blackboardData.AddData(BlackboardKey.PSCARD, m_entity);
            // 真正运行行为树
            m_bt.Behave();
        }

        // 设置 AI 控制器操作的场景对象
        public void possess(SceneEntityBase entity)
        {
            m_entity = entity;
        }

        public void syncUpdateBT()
        {
            if (m_bNeedReloadBT)
            {
                m_bt = Ctx.m_instance.m_aiSystem.behaviorTreeMgr.getBT(m_btID);
            }

            m_bNeedReloadBT = false;
        }
    }
}