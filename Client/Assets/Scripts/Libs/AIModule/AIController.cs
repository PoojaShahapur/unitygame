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
        protected BTID m_btID;       // 行为树 ID 

        public AIController()
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
                m_btID = value;
            }
        }

        public void onTick(float delta)
        {
            //if(m_radar != null)
            //{
            //    m_radar.UpdateRadar();      // 更新雷达数据
            //}

            // 初始化黑盒数据
            // 真正运行行为树
            runBT();
        }

        public void possess(ISceneEntity entity)
        {
            m_entity = entity;
        }

        protected void runBT()
        {
            BehaviorTreeRes btRes = Ctx.m_instance.m_aiSystem.behaviorTreeMgr.getAndLoadBT(m_btID);
            btRes.behaviorTree.Behave();
        }
    }
}