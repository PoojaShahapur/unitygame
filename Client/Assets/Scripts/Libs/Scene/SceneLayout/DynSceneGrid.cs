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
        protected float m_elemNormalWidth;        // 元素宽度
        protected float m_yDelta;

        protected List<Vector3> m_posList;

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

        public float elemNormalWidth
        {
            get
            {
                return m_elemNormalWidth;
            }
            set
            {
                m_elemNormalWidth = value;
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

        public void updateElem()
        {
            m_bNeedUpdate = false;

            if (!hasExpandElem())
            {
                normalUpdateElem();
            }
            else
            {
                expandUpdateElem();
            }
        }

        protected void normalUpdateElem()
        {
            // 计算位置信息
            m_posList.Clear();
            UtilMath.newRectSplit(m_centerPos, m_elemNormalWidth, m_radius, m_yDelta, getValidElemCount(m_elemList), ref m_posList);

            // 更新元素信息
            int posIdx = 0;
            for (int idx = 0; idx < m_elemList.Count(); ++idx)
            {
                if (m_elemList[idx].bValid)
                {
                    m_elemList[idx].setNormalPos(m_posList[posIdx]);
                    ++posIdx;
                }
            }
        }

        protected void expandUpdateElem()
        {
            // 计算正常位置
            int validCount = getValidElemCount(m_elemList);
            m_posList.Clear();
            UtilMath.newRectSplit(m_centerPos, m_elemNormalWidth, m_radius, m_yDelta, validCount, ref m_posList);

            // 更新元素信息
            float deltaX = 0;
            int posIdx = 0;

            float origX = 0;
            float origZ = 0;
            float expandDeltaX = 0;
            float expandDeltaZ = 0;
            float nextOrigX = 0;        // 后面一个的位置
            float nextExpandX = 0;        // 后面一个的位置

            for (int elemIdx = 0; elemIdx < m_elemList.Count(); ++elemIdx)
            {
                if (m_elemList[elemIdx].bValid)
                {
                    m_posList[posIdx] += new Vector3(deltaX, 0, 0);

                    if (!m_elemList[elemIdx].isNormalState())
                    {
                        // 计算原始的位置
                        origX = m_posList[posIdx].x;
                        origZ = m_posList[posIdx].z;
                        // 计算由于放大需要的偏移量
                        expandDeltaX = (m_elemList[elemIdx].getExpandWidth() - m_elemList[elemIdx].getNormalWidth()) / 2;
                        expandDeltaZ = (m_elemList[elemIdx].getExpandHeight() - m_elemList[elemIdx].getNormalHeight()) / 2;
                        // 计算正确的中心位置
                        m_posList[posIdx] += new Vector3(expandDeltaX, 0, expandDeltaZ);
                        // 如果后面还有元素
                        if(posIdx < validCount - 1)
                        {
                            // 计算下一个元素在当前元素没有扩大的情况下，中心点位置
                            nextOrigX = m_posList[posIdx + 1].x + deltaX;
                            // 计算下一个元素在当前元素有扩大的情况下，中心点位置
                            nextExpandX = m_posList[posIdx].x + m_elemList[elemIdx].getExpandWidth() / 2 + m_elemList[elemIdx + 1].getNormalWidth() / 2;
                            // 计算后面的元素应该偏移量
                            deltaX += (nextExpandX - nextOrigX);
                        }
                    }

                    m_elemList[elemIdx].setExpandPos(m_posList[posIdx]);

                    ++posIdx;
                }
            }
        }

        protected int getValidElemCount(MList<GridElementBase> elemList_)
        {
            int count = 0;

            foreach (var item in elemList_.list)
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
            //onElemChangedHandle(elem);    // 添加后不改变

            return elem;
        }

        public void addElem(GridElementBase elem, int idx = -1)
        {
            if(-1 == idx)
            {
                m_elemList.Add(elem);
            }
            else
            {
                m_elemList.Insert(idx, elem);
            }
        }

        public void removeElem(GridElementBase elem)
        {
            m_elemList.Remove(elem);
        }

        public void newRectSplit(Transform trans, float areaRadius, float fYDelta, MList<GridElementBase> elemList_, ref List<Vector3> posList)
        {
            float unitWidth = 0;
            int splitCnt = 0;
            Vector3 pos;
            int listIdx = 0;

            if (unitWidth * splitCnt > 2 * areaRadius)       // 如果当前区域不能完整放下所有的单元
            {
                float splitCellWidth = (areaRadius * 2) / splitCnt;
                while (listIdx < splitCnt)
                {
                    pos.x = trans.localPosition.x + splitCellWidth * listIdx - areaRadius;
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }
            }
            else            // 全部能放下，就居中显示
            {
                float halfWidth = (float)((unitWidth * splitCnt) * 0.5);        // 占用的区域的一般宽度
                while (listIdx < splitCnt)
                {
                    pos.x = trans.localPosition.x + unitWidth * listIdx - halfWidth;
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }
            }
        }
    }
}