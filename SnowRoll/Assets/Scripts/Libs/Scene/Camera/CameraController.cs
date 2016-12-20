using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机控制器
     */
    public class CameraController
    {
        protected Camera mCamera;  // 摄像机
        protected GameObject mTargetGo;    // 目标对象
        protected Transform mTargetTrans;  // 目标转换
        protected Vector3 mPos;       // 临时变量
        protected Coordinate mCoord;  // 坐标系统

        public CameraController(Camera camera, GameObject target)
        {
            mCamera = camera;
            mTargetGo = target;
            if (mTargetGo == null)
            {
                mTargetGo = UtilApi.createGameObject("CameraGo");
            }
            mTargetTrans = mTargetGo.GetComponent<Transform>();
        }

        // 增加 theta
        virtual public void incTheta(float delta)
        {
            
        }

        // 减少 theta
        virtual public void decTheta(float delta)
        {
                
        }

        public void setTarget(GameObject target)
        {
            mTargetGo = target;
            mTargetTrans = mTargetGo.GetComponent<Transform>();
        }

        virtual public void updateControl()
        {
            if (null != mCoord)
            {
                mCoord.updateCoord();
            }
            Ctx.mInstance.mCamSys.invalidCamera();
        }
    }
}