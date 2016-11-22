using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 动态场景格子，元素可任意大小
     */
    public class DynSceneGrid
    {
        protected MList<GridElementBase> mElemList;        // 所有元素列表
        protected bool mIsNeedUpdate;             // 元素列表是否需要更新
        protected FrameTimerItem mNextFrametimer;       // 需要下一帧才能更新的数据

        protected Transform mCenterPos;    // 中心点
        protected float mRadius;           // 半径
        protected float mElemNormalWidth;        // 元素宽度
        protected float mYDelta;

        protected List<Vector3> m_posList;

        public DynSceneGrid()
        {
            this.mElemList = new MList<GridElementBase>();
            this.mIsNeedUpdate = false;
            m_posList = new List<Vector3>();
        }

        public void dispose()
        {
            if (this.mNextFrametimer != null)
            {
                Ctx.m_instance.m_frameTimerMgr.removeFrameTimer(this.mNextFrametimer);
                this.mNextFrametimer = null;
            }
        }

        public Transform centerPos
        {
            get
            {
                return this.mCenterPos;
            }
            set
            {
                this.mCenterPos = value;
            }
        }

        public float radius
        {
            get
            {
                return this.mRadius;
            }
            set
            {
                this.mRadius = value;
            }
        }

        public float elemNormalWidth
        {
            get
            {
                return this.mElemNormalWidth;
            }
            set
            {
                this.mElemNormalWidth = value;
            }
        }

        public float yDelta
        {
            get
            {
                return this.mYDelta;
            }
            set
            {
                this.mYDelta = value;
            }
        }

        // 启动下一帧定时器
        protected void startNextFrameTimer()
        {
            if (this.mNextFrametimer == null)
            {
                this.mNextFrametimer = new FrameTimerItem();
                this.mNextFrametimer.mTimerDisp = onNextFrameHandle;
                this.mNextFrametimer.mInternal = 1;
                this.mNextFrametimer.mTotalFrameCount = 1;
            }
            else
            {
                this.mNextFrametimer.reset();
            }

            Ctx.m_instance.m_frameTimerMgr.addFrameTimer(this.mNextFrametimer);
        }

        public void onNextFrameHandle(FrameTimerItem timer)
        {
            updateElem();
        }

        // 元素改变
        protected void onElemChangedHandle(IDispatchObject dispObj)
        {
            if (!this.mIsNeedUpdate)
            {
                this.mIsNeedUpdate = true;
                startNextFrameTimer();
            }
        }

        public void updateElem()
        {
            this.mIsNeedUpdate = false;

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
            UtilLogic.newRectSplit(this.mCenterPos, this.mElemNormalWidth, this.mRadius, this.mYDelta, getValidElemCount(this.mElemList), ref m_posList);

            // 更新元素信息
            int posIdx = 0;
            for (int idx = 0; idx < this.mElemList.Count(); ++idx)
            {
                if (this.mElemList[idx].bValid)
                {
                    this.mElemList[idx].setNormalPos(m_posList[posIdx]);
                    ++posIdx;
                }
            }
        }

        protected void expandUpdateElem()
        {
            // 计算正常位置
            int validCount = getValidElemCount(this.mElemList);
            m_posList.Clear();
            UtilLogic.newRectSplit(this.mCenterPos, this.mElemNormalWidth, this.mRadius, this.mYDelta, validCount, ref m_posList);

            // 更新元素信息
            float deltaX = 0;
            int posIdx = 0;

            float origX = 0;
            float origZ = 0;
            float expandDeltaX = 0;
            float expandDeltaZ = 0;
            float nextOrigX = 0;        // 后面一个的位置
            float nextExpandX = 0;        // 后面一个的位置

            for (int elemIdx = 0; elemIdx < this.mElemList.Count(); ++elemIdx)
            {
                if (this.mElemList[elemIdx].bValid)
                {
                    m_posList[posIdx] += new Vector3(deltaX, 0, 0);

                    if (!this.mElemList[elemIdx].isNormalState())
                    {
                        // 计算原始的位置
                        origX = m_posList[posIdx].x;
                        origZ = m_posList[posIdx].z;
                        // 计算由于放大需要的偏移量
                        expandDeltaX = (this.mElemList[elemIdx].getExpandWidth() - this.mElemList[elemIdx].getNormalWidth()) / 2;
                        expandDeltaZ = (this.mElemList[elemIdx].getExpandHeight() - this.mElemList[elemIdx].getNormalHeight()) / 2;
                        // 计算正确的中心位置
                        m_posList[posIdx] += new Vector3(expandDeltaX, 0, expandDeltaZ);
                        // 如果后面还有元素
                        if(posIdx < validCount - 1)
                        {
                            // 计算下一个元素在当前元素没有扩大的情况下，中心点位置
                            nextOrigX = m_posList[posIdx + 1].x + deltaX;
                            // 计算下一个元素在当前元素有扩大的情况下，中心点位置
                            nextExpandX = m_posList[posIdx].x + this.mElemList[elemIdx].getExpandWidth() / 2 + this.mElemList[elemIdx + 1].getNormalWidth() / 2;
                            // 计算后面的元素应该偏移量
                            deltaX += (nextExpandX - nextOrigX);
                        }
                    }

                    this.mElemList[elemIdx].setExpandPos(m_posList[posIdx]);

                    ++posIdx;
                }
            }
        }

        protected int getValidElemCount(MList<GridElementBase> elemList_)
        {
            int count = 0;

            foreach (var item in elemList_.list())
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
            foreach (var item in this.mElemList.list())
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

            this.mElemList.Add(elem);
            elem.addUpdateHandle(onElemChangedHandle);
            //onElemChangedHandle(elem);    // 添加后不改变

            return elem;
        }

        public void addElem(GridElementBase elem, int idx = -1)
        {
            if(-1 == idx)
            {
                this.mElemList.Add(elem);
            }
            else
            {
                this.mElemList.Insert(idx, elem);
            }
        }

        public void removeElem(GridElementBase elem)
        {
            this.mElemList.Remove(elem);
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