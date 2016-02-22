using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 控制摄像机运动
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
            Ctx.m_instance.m_inputMgr.addKeyListener(EventID.KEYDOWN_EVENT, onKeyDown);
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

        public void onKeyDown(KeyCode key)
        {
            if (KeyCode.W == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z  + 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.S == key)
            {
                m_localPos = m_targetTrans.localPosition;
                m_localPos.z = m_localPos.z - 0.1f;
                m_targetTrans.localPosition = m_localPos;
                m_cameraController.updateControl();
            }
            else if (KeyCode.A == key)
            {
                m_localRot = m_targetTrans.localEulerAngles;
                m_localRot.y = m_localRot.y + 0.1f;
                m_targetTrans.localEulerAngles = m_localRot;
                m_cameraController.updateControl();
            }
            else if (KeyCode.D == key)
            {
                m_localRot = m_targetTrans.localEulerAngles;
                m_localRot.y = m_localRot.y - 0.1f;
                m_targetTrans.localEulerAngles = m_localRot;
                m_cameraController.updateControl();
            }
        }
    }
}