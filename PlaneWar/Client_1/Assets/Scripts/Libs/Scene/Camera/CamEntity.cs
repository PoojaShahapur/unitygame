using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景中的摄像机 Entity
     */
    public class CamEntity
    {
        public Camera mCam;     // 相机数据

        public CamEntity()
        {
            this.mCam = null;
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        // 设置本地相机系统
        public void setNativeCam(Camera cam)
        {
            this.mCam = cam;
        }

        public Camera getNativeCam()
        {
            return this.mCam;
        }
    }
}