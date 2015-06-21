using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 动态场景格子，元素可任意大小
     */
    public class DynSceneGrid
    {
        protected MList<GridElementBase> m_elemList;        // 所有元素列表
        protected bool m_bNeedUpdate;             // 元素列表是否需要更新
        protected FrameTimerItem m_nextFrametimer;       // 需要下一帧才能更新的数据
        protected Transform m_centerPos;    // 中心点
        protected float m_radius;           // 半径
        protected float m_elemWidth;        // 元素宽度
        protected List<Vector3> m_posList;
        protected float m_yDelta;

        public DynSceneGrid()
        {
            m_elemList = new MList<GridElementBase>();
            m_bNeedUpdate = false;
            m_posList = new List<Vector3>();
        }

        public void dispose()
        {
            if (m_nextFrametimer != null)
            {
                Ctx.m_instance.m_frameTimerMgr.delObject(m_nextFrametimer);
                m_nextFrametimer = null;
            }
        }

        public Transform centerPos
        {
            get
            {
                return m_centerPos;
            }
            set
            {
                m_centerPos = value;
            }
        }

        public float radius
        {
            get
            {
                return m_radius;
            }
            set
            {
                m_radius = value;
            }
        }

        public float elemWidth
        {
            get
            {
                return m_elemWidth;
            }
            set
            {
                m_elemWidth = value;
            }
        }

        public float yDelta
        {
            get
            {
                return m_yDelta;
            }
            set
            {
                m_yDelta = value;
            }
        }

        // 启动下一帧定时器
        protected void startNextFrameTimer()
        {
            if (m_nextFrametimer == null)
            {
                m_nextFrametimer = new FrameTimerItem();
                m_nextFrametimer.m_timerDisp = onNextFrameHandle;
                m_nextFrametimer.m_internal = 1;
                m_nextFrametimer.m_totalFrameCount = 1;
            }
            else
            {
                m_nextFrametimer.reset();
            }

            Ctx.m_instance.m_frameTimerMgr.addObject(m_nextFrametimer);
        }

        public void onNextFrameHandle(FrameTimerItem timer)
        {
            updateElem();
        }

        // 元素改变
        protected void onElemChangedHandle(IDispatchObject dispObj)
        {
            if (!m_bNeedUpdate)
            {
                m_bNeedUpdate = true;
                startNextFrameTimer();
            }
        }

        protected void updateElem()
        {
            m_bNeedUpdate = false;

            // 计算位置信息
            m_posList.Clear();
            UtilMath.newRectSplit(m_centerPos, m_elemWidth, m_radius, m_yDelta, getValidElemCount(), ref m_posList);

            // 更新元素信息
            int posIdx = 0;
            for(int idx = 0; idx < m_elemList.Count(); ++idx)
            {
                if(m_elemList[idx].bValid)
                {
                    m_elemList[idx].setPos(m_posList[posIdx]);
                    m_elemList[idx].updatePos();
                    ++posIdx;
                }
            }
        }

        protected int getValidElemCount()
        {
            int count = 0;

            foreach(var item in m_elemList.list)
            {
                if(item.bValid)
                {
                    ++count;
                }
            }

            return count;
        }

        protected bool hasExpandElem()
        {
            foreach (var item in m_elemList.list)
            {
                if (!item.isNormalState())
                {
                    return true;
                }
            }

            return false;
        }

        public GridElementBase createAndAddElem(GridElementType type)
        {
            GridElementBase elem = null;

            if(GridElementType.eBasic == type)
            {
                elem = new GridElementBase();
            }
            if (GridElementType.eScale == type)
            {
                elem = new ScaleGridElement();
            }

            m_elemList.Add(elem);
            elem.addUpdateHandle(onElemChangedHandle);
            onElemChangedHandle(elem);

            return elem;
        }

        public void removeElem(GridElementBase elem)
        {
            m_elemList.Remove(elem);
        }
    }
}