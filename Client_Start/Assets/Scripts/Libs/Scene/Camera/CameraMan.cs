using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 控制摄像机运动， CameraMan 只能 Y 轴旋转，如果要旋转摄像机绕其它轴，需要设置摄像机的坐标系
     */
    public class CameraMan
    {
        protected GameObject m_targetGo;
        protected Transform m_targetTrans;
        protected CameraController m_cameraController;
        protected Vector3 m_localPos;
        protected Vector3 m_localRot;

        public CameraMan(GameObject targetGo)
        {
            m_localPos = Vector3.zero;
            m_targetGo = targetGo;
            if (m_targetGo == null)
            {
                m_targetGo = UtilApi.createGameObject("CameraGo");
            }

            m_targetTrans = m_targetGo.transform;
            Ctx.m_instance.m_inputMgr.addKeyListener(EventID.KEYPRESS_EVENT, onKeyPress);
        }

        public void setActor(GameObject targetGo)
        {
            m_targetGo = targetGo;
            if (m_targetGo == null)
            {
                m_targetGo = UtilApi.createGameObject("CameraGo");
            }
        }

        public void setCameraController(CameraController controller)
        {
            m_cameraController = controller;
        }

        virtual public void onKeyPress(KeyCode key)
        {
            if (KeyCode.W == key)
            {
                //m_localRot = m_targetTrans.localEulerAngles;
                //m_localRot.x = UtilApi.incEulerAngles(m_localRot.x, 1);
                //m_targetTrans.localEulerAngles = m_localRot;
                //m_cameraController.updateControl();
                m_cameraController.incTheta(1);
            }
            else if (KeyCode.S == key)
            {
                //m_localRot = m_targetTrans.localEulerAngles;
                //m_localRot.x = UtilApi.decEulerAngles(m_localRot.x, 1);
                //m_targetTrans.localEulerAngles = m_localRot;
                //m_cameraController.updateControl();
                m_cameraController.decTheta(1);
            }
            else if (KeyCode.A == key)
            {
                m_localRot = m_targetTrans.localEulerAngles;
                m_localRot.y = UtilApi.incEulerAngles(m_localRot.y, 1);
                m_targetTrans.localEulerAngles = m_localRot;
                m_cameraController.updateControl();
            }
            else if (KeyCode.D == key)
            {
                m_localRot = m_targetTrans.localEulerAngles;
                m_localRot.y = UtilApi.decEulerAngles(m_localRot.y, 1);
                m_targetTrans.localEulerAngles = m_localRot;
                m_cameraController.updateControl();
            }
            else if (KeyCode.UpArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z + 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.DownArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z - 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.RightArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.x = m_localPos.x + 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.LeftArrow == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.x = m_localPos.x - 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
        }
    }
}