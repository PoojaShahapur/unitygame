using SDK.Common;
using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 攻击箭头
     */
    public class AttackArrow : ITickedObject
    {
        public SceneDZData m_sceneDZData;
        protected List<ArrowItem> m_arrowList = new List<ArrowItem>();          // 当前显示的箭头
        protected List<ArrowItem> m_buffList = new List<ArrowItem>();           // 缓冲箭头

        protected Vector3 m_lastMousePos;
        protected Vector3 m_curMousePos;

        protected Vector3 m_currentPos;         // 当前鼠标场景中的位置
        protected Ray m_ray;
        protected float m_dist = 0f;
        protected Vector3 m_rot;            // 选装
        protected ArrowItem m_tmpRet;       // 临时返回

        public AttackArrow(SceneDZData sceneDZData)
        {
            m_sceneDZData = sceneDZData;
        }

        // 开始显示箭头
        public void startArrow()
        {
            Ctx.m_instance.m_tickMgr.addObject(this);
            updatePos();
        }

        // 停止显示箭头
        public void stopArrow()
        {
            Ctx.m_instance.m_tickMgr.delObject(this);
            delArrow();
        }

        public void OnTick(float delta)
        {
            if (needUpdate())
            {
                updateRot();
                updateArrow();
            }
        }

        protected void getCurMouseScenePos()
        {
            m_currentPos = Ctx.m_instance.m_coordConv.getCurTouchScenePos();
        }

        protected void updatePos()
        {
            // 计算位置
            getCurMouseScenePos();
            UtilApi.setPos(m_sceneDZData.m_attackArrowGO.transform, new Vector3(m_currentPos.x, 1, m_currentPos.z));
        }

        protected void updateRot()
        {
            // 计算旋转
            getCurMouseScenePos();

            // 位置一定要转换到 m_attackArrowGO 里面，不要转换到 m_arrowListGO ，因为 m_arrowListGO 是不断旋转的，如果转换到 m_arrowListGO ，导致数据抖动
            m_currentPos = m_sceneDZData.m_attackArrowGO.transform.InverseTransformPoint(m_currentPos);

            m_rot.y = -Mathf.Atan2(m_currentPos.z, m_currentPos.x) * Mathf.Rad2Deg;            // 弧度转成度数
            UtilApi.setRot(m_sceneDZData.m_arrowListGO.transform, Quaternion.Euler(m_rot));
        }

        protected void updateArrow()
        {
            if(m_arrowList.Count == 0)
            {
                getArrowItem();
                m_arrowList.Add(m_tmpRet);
            }
        }

        protected void delArrow()
        {
            int idx = 0;
            while (idx < m_arrowList.Count)
            {
                m_buffList.Add(m_arrowList[idx]);
                m_arrowList[idx].m_go.SetActive(false);

                ++idx;
            }

            m_arrowList.Clear();
        }

        protected void getArrowItem()
        {
            if (m_buffList.Count > 0)
            {
                m_tmpRet = m_buffList[0];
                m_buffList.Remove(m_tmpRet);
                m_tmpRet.m_go.SetActive(true);
            }
            else
            {
                m_tmpRet = new ArrowItem();
                m_tmpRet.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathModel] + "ArrowItem.prefab";
                m_tmpRet.m_parentTran = m_sceneDZData.m_arrowListGO.transform;
                m_tmpRet.load();        // 立即加载
            }

            m_tmpRet.normalRot();
        }

        protected bool needUpdate()
        {
            m_lastMousePos = m_curMousePos;
            m_curMousePos = Input.mousePosition;

            if(m_lastMousePos.Equals(m_curMousePos))
            {
                return false;
            }

            return true;
        }

        public void setClientDispose()
        {

        }

        public bool getClientDispose()
        {
            return false;
        }
    }
}