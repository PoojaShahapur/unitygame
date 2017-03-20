using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的摄像机 Entity
     */
    public class CamEntity : IDispatchObject
    {
        public AddOnceEventDispatch mCameraCreatedDispatch;     // 本地相机创建分发

        protected Camera mCam;     // 相机数据

        public CamEntity()
        {
            this.mCam = null;
            this.mCameraCreatedDispatch = new AddOnceEventDispatch();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        // 摄像机是否真正的创建
        public bool isValid()
        {
            return (null != this.mCam);
        }

        // 设置本地相机系统
        public void setNativeCam(Camera cam)
        {
            if (this.mCam != cam)
            {
                this.mCam = cam;
                this.mCameraCreatedDispatch.dispatchEvent(this);
            }
        }

        public Camera getNativeCam()
        {
            return this.mCam;
        }
    }
}