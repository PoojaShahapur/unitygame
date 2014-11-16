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
            if (m_skinAniModel.animSys.animator && Camera.main)
            {
                Do(m_skinAniModel.transform, Camera.main.transform, ref speed, ref direction);
                m_skinAniModel.animSys.Do(speed * 6, direction * 180);
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
        virtual public void addAi(BehaviorTree behaviorTree)
        {
            m_vehicle = new Biped();
            m_behaviorTree = behaviorTree;
        }

        public void Do(Transform root, Transform camera, ref float speed, ref float direction)
        {
            Vector3 rootDirection = root.forward;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

            // Get camera rotation.    

            Vector3 CameraDirection = camera.forward;
            CameraDirection.y = 0.0f; // kill Y
            Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

            // Convert joystick input in Worldspace coordinates
            Vector3 moveDirection = referentialShift * stickDirection;

            Vector2 speedVec = new Vector2(horizontal, vertical);
            speed = Mathf.Clamp(speedVec.magnitude, 0, 1);

            if (speed > 0.01f) // dead zone
            {
                Vector3 axis = Vector3.Cross(rootDirection, moveDirection);
                direction = Vector3.Angle(rootDirection, moveDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
            }
            else
            {
                direction = 0.0f;
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

        public void setPartModel(PlayerModelDef modelDef, string name)
        {
            
        }
	}
}