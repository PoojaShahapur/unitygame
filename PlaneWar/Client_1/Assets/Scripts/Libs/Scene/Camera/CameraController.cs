using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机控制器
     */
    public class CameraController
    {
        protected CamEntity mCamEntity;
        protected Camera mCamera;  // 摄像机

        protected ICamTargetEntiry mTargetEntity;   // 目标实体
        protected GameObject mTargetGo;    // 目标对象

        protected Transform mTargetTrans;  // 目标转换
        protected Vector3 mPos;       // 临时变量
        protected Coordinate mCoord;  // 坐标系统

        public CameraController(CamEntity camera, ICamTargetEntiry targetEntity)
        {
            this.mCamEntity = camera;
            this.mCamera = this.mCamEntity.getNativeCam();

            if (null != targetEntity)
            {
                this.mTargetEntity = targetEntity;
                this.mTargetGo = this.mTargetEntity.getNativeTarget();

                this.mTargetEntity.addTargetCreatedHandle(this.onCamTargetCreated);
            }

            if (this.mTargetGo == null)
            {
                this.mTargetGo = UtilApi.createGameObject("CameraGo");
            }

            this.mTargetTrans = this.mTargetGo.GetComponent<Transform>();

            if(null != this.mCamera)
            {
                camera.mCameraCreatedDispatch.addEventHandle(null, this.onCamCreated);
            }
        }

        virtual public void dispose()
        {
            this.mCamEntity.mCameraCreatedDispatch.removeEventHandle(null, this.onCamCreated);

            if (null != this.mTargetEntity)
            {
                this.mTargetEntity.removeTargetCreatedHandle(this.onCamTargetCreated);
            }
        }

        virtual public void onCamCreated(IDispatchObject dispObj)
        {
            CamEntity camera = dispObj as CamEntity;
            this.mCamera = camera.getNativeCam();
        }

        virtual public void onCamTargetCreated(IDispatchObject dispObj)
        {
            ICamTargetEntiry targetEntity = dispObj as ICamTargetEntiry;
            this.mTargetGo = targetEntity.getNativeTarget();
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