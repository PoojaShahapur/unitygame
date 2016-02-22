using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 第三人称摄像机控制器
     */
    public class ThirdCameraController : CameraController
    {
        protected SphericalCoordinate m_coord;  // 坐标系统
        protected Transform m_cameraTrans;            // 摄像机的转换

        public ThirdCameraController(Camera camera, GameObject target)
            : base(camera, target)
        {
            m_coord = new SphericalCoordinate();
            m_cameraTrans = m_camera.GetComponent<Transform>();

            this.setParam(5, Mathf.PI / 4, Mathf.PI);
        }

        public void setParam(float radius, float theta, float fai)
        {
            m_coord.setParam(radius, theta, fai);
            //m_coord.syncTrans(m_cameraTrans);
            updateControl();
        }

        override public void updateControl()
        {
            base.updateControl();

            //m_cameraTrans.rotation = m_targetTrans.rotation;
            //m_cameraTrans.position = m_targetTrans.position;
            m_pos = m_targetTrans.localToWorldMatrix * new Vector4(m_coord.getX(), m_coord.getY(), m_coord.getZ(), 1);
            m_cameraTrans.position = m_pos;
            //m_localPos = m_cameraTrans.localPosition;
            //m_localPos.x = m_coord.getX();
            //m_localPos.y = m_coord.getY();
            //m_localPos.z = m_coord.getZ();
            //m_cameraTrans.localPosition = m_localPos;
            m_cameraTrans.LookAt(m_targetTrans);
        }
    }
}