using SDK.Common;
using SDK.Lib;
using UnityEngine;
using UnitySteer.Behaviors;

namespace BehaviorLibrary
{
    /**
     * @brief 凡是 BehaviorTree 之外的 AI 逻辑都写在这里
     */
    public class AIController : ISceneEntity
    {
        protected Biped m_vehicle;
        protected Radar m_radar;                // 每一个人身上有一个雷达

        protected ISceneEntity m_entity;        // 控制的场景 Entity
        protected BTID m_btID;          // 行为树 ID 
        protected BehaviorTreeRes m_btRes;        // 行为树资源
        protected BehaviorTree m_bt;              // 行为树
        protected bool m_bNeedReloadBT;

        public AIController()
        {
            m_bNeedReloadBT = false;
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

        public Radar radar
        {
            get
            {
                return m_radar;
            }
            set
            {
                m_radar = value;
            }
        }

        public void initControl(SkinAniModel skinAniModel)
        {
            m_radar = new Radar();
            m_vehicle = new Biped();

            //m_vehicle.Radar = m_radar;
            //m_vehicle.ArrivalRadius = 1;
            //m_vehicle.AllowedMovementAxes = new Vector3(1, 0, 1);
            //m_vehicle.MaxSpeed = 10;
            //m_vehicle.setSpeed(5);
            //m_vehicle.initOwner(skinAniModel.rootGo);

            //m_radar.Vehicle = m_vehicle;
            //m_radar.initAwake();
            m_radar.DetectionRadius = 100;
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

        public void dispose()
        {
            if(m_btRes != null)
            {
                Ctx.m_instance.m_aiSystem.behaviorTreeMgr.unload(m_btRes.GetPath(), null);
                m_btRes = null;
            }

            if(m_bt != null)
            {
                m_bt = null;
            }
        }

        public void onTick(float delta)
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
        public void possess(ISceneEntity entity)
        {
            m_entity = entity;
        }

        public void syncUpdateBT()
        {
            if (m_bNeedReloadBT)
            {
                m_btRes = Ctx.m_instance.m_aiSystem.behaviorTreeMgr.getAndLoadBT(m_btID);
                m_bt = m_btRes.behaviorTree;
            }

            m_bNeedReloadBT = false;
        }
    }
}