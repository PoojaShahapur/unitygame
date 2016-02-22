﻿using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 摄像机控制器
     */
    public class CameraController
    {
        protected Camera m_camera;  // 摄像机
        protected GameObject m_targetGo;    // 目标对象
        protected Transform m_targetTrans;  // 目标转换
        protected Vector3 m_pos;       // 临时变量

        public CameraController(Camera camera, GameObject target)
        {
            m_camera = camera;
            m_targetGo = target;
            if (m_targetGo == null)
            {
                m_targetGo = UtilApi.createGameObject("CameraGo");
            }
            m_targetTrans = m_targetGo.GetComponent<Transform>();
        }

        public void setTarget(GameObject target)
        {
            m_targetGo = target;
            m_targetTrans = m_targetGo.GetComponent<Transform>();
        }

        virtual public void updateControl()
        {

        }
    }
}