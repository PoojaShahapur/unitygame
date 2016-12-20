using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第三人称摄像机控制器
     */
    public class ThirdCameraController : CameraController
    {
        protected Transform mCameraTrans;            // 摄像机的转换

        public ThirdCameraController(Camera camera, GameObject target)
            : base(camera, target)
        {
            mCoord = new SphericalCoordinate();
            mCameraTrans = mCamera.GetComponent<Transform>();

            //this.setParam(5, 45, 180);
            //this.setParam(10, 45, 0);
            //this.setParam(10, 45, 45);
            this.setParam(1, 45, 0);
        }

        // 增加 theta
        override public void incTheta(float deltaDegree)
        {
            mCoord.incTheta(deltaDegree);
            this.updateControl();
        }

        // 减少 theta
        override public void decTheta(float deltaDegree)
        {
            mCoord.decTheta(deltaDegree);
            this.updateControl();
        }

        public void setParam(float radius, float theta, float fai)
        {
            mCoord.setParam(radius, theta, fai);
            //mCoord.syncTrans(mCameraTrans);
            this.updateControl();
        }

        override public void updateControl()
        {
            base.updateControl();

            //mCameraTrans.rotation = mTargetTrans.rotation;
            //mCameraTrans.position = mTargetTrans.position;
            mPos = mTargetTrans.localToWorldMatrix * new Vector4(mCoord.getX(), mCoord.getY(), mCoord.getZ(), 1);
            mCameraTrans.position = mPos;
            //mLocalPos = mCameraTrans.localPosition;
            //mLocalPos.x = mCoord.getX();
            //mLocalPos.y = mCoord.getY();
            //mLocalPos.z = mCoord.getZ();
            //mCameraTrans.localPosition = mLocalPos;
            // 如果 mCoord.m_theta == 0；这个时候 LookAt 会变成垂直
            mCameraTrans.LookAt(mTargetTrans);
            //if(mCoord.getTheta() == 0)
            //{
            //    mCameraTrans.eulerAngles = new Vector3(0, mTargetTrans.eulerAngles.y, 0);
            //}
        }
    }
}