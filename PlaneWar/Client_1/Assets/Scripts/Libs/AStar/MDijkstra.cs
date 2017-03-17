using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    public partial class MGraph
    {
        protected void buildPath(Vertex endVert)
        {
            Vertex vert = endVert;
            while (vert != null)
            {
                this.mPathList.Insert(0, vert);
                vert = vert.mNearestVert;
            }
            this.mPathList.Insert(0, this.mStartVert);      // 把最初的顶点放进去
        }

        protected void initVerts(int startId, int endId)
        {
            this.mStartVert = null;
            this.mEndVert = null;

            int nx = 0;
            int ny = 0;

            convVertIdToXY(startId, ref nx, ref ny);
            if (nx >= 0 && nx < this.mXCount
                && ny >= 0 && ny < this.mYCount)
            {
                this.mStartVert = this.mVertsVec[startId];
            }

            convVertIdToXY(endId, ref nx, ref ny);
            if (nx >= 0 && nx < this.mXCount
                && ny >= 0 && ny < this.mYCount)
            {
                this.mEndVert = this.mVertsVec[endId];
            }

            if (this.mStartVert == null || this.mEndVert == null)
            {
                throw new Exception("Failed to find the start/end node(s)!");
            }

            this.mStartVert.mDistance = 0;
        }

        protected void resetAllVerts(int startId)
        {
            Vertex pVert = null;
            // 初始化数据
            for (int vertIdx = 0; vertIdx < this.mVertsCount; ++vertIdx)
            {
                pVert = this.mVertsVec[vertIdx];
                pVert.mState = State.Unknown;    // 全部顶点初始化为未知对短路径状态
                pVert.mDistance = adjacentCost(startId, vertIdx); //将与 startId 点有连线的顶点加上权值
                pVert.mNearestVert = null;    // 初始化路径的前一个顶点
            }

            this.mStartVert.mDistance = 0;    //  startId 至 startId 路径为0
            this.mStartVert.mState = State.Closed;    // m_startVert->mState 表示 startId 至 startId 不需要求路径，已经添加到确认队列中了
        }

        protected bool findNextClosedVert(ref float minDist, ref int minIdx, List<int> closedVec)
        {
            Vertex pVert = null;
            int closedIdx = 0;
            int neighborVertIdx = 0;
            minDist = float.MaxValue;    // 当前所知离 startId 顶点最近距离
            bool bFindNextClosedVert = false;

            // 只要遍历周围 8 个顶点就可以了，因为格子寻路只能是和自己周围的 8 个格子才有权重，和其它的格子是没有权重的
            for (closedIdx = 0; closedIdx < closedVec.Count; ++closedIdx)
            {
                if (!this.mVertsVec[closedVec[closedIdx]].mIsNeighborValid)     // 如果邻居数据是无效的
                {
                    findNeighborVertIdArr(closedVec[closedIdx]);
                    this.mVertsVec[closedVec[closedIdx]].setNeighborVertsId(this.mNeighborVertIdArr);
                }

                for (neighborVertIdx = 0; neighborVertIdx < this.mVertsVec[closedVec[closedIdx]].mVertsIdVec.Count; ++neighborVertIdx)
                {
                    pVert = this.mVertsVec[this.mVertsVec[closedVec[closedIdx]].mVertsIdVec[neighborVertIdx]];
                    if (pVert.mState != State.Closed && pVert.mDistance < minDist)
                    {
                        minDist = pVert.mDistance; // w顶点离 startId 顶点更近
                        minIdx = pVert.mId;
                        bFindNextClosedVert = true;             // 说明查找到了
                    }
                }
            }

            return bFindNextClosedVert;
        }

        protected void modifyVertsDist(ref float minDist, ref int minIdx)
        {
            int neighborVertIdx = 0;
            Vertex pVert = null;

            // 只需要遍历最新加入 closed 的顶点的邻居顶点
            if (!this.mVertsVec[minIdx].mIsNeighborValid)       // 如果邻居数据是无效的
            {
                findNeighborVertIdArr(minIdx);
                this.mVertsVec[minIdx].setNeighborVertsId(this.mNeighborVertIdArr);
            }
            for (neighborVertIdx = 0; neighborVertIdx < this.mVertsVec[minIdx].mVertsIdVec.Count; ++neighborVertIdx) // 修正当前最短路径距离
            {
                pVert = this.mVertsVec[this.mVertsVec[minIdx].mVertsIdVec[neighborVertIdx]];
                // 如果经过V顶点的路径比现在这条路径的长度短的话
                if (pVert.mState != State.Closed && (minDist + adjacentCost(minIdx, pVert.mId) < pVert.mDistance))
                {
                    // 说明找到了最短的路径，修改D[w] 和 p[w]
                    pVert.mDistance = minDist + adjacentCost(minIdx, pVert.mId); // 修改当前路径长度
                    pVert.mNearestVert = this.mVertsVec[minIdx];
                }
            }
        }

        public List<Vertex> getShortestPath()
        {
            return this.mPathList;
        }

        public void createShortestPath(int startId, int endId)
        {
            this.mPathList.Clear();
            this.mClosedVec.Clear();

            initVerts(startId, endId);

            int openVertIdx = 0;        // 还未确定添加到队列中的顶点遍历的索引
            int minIdx = 0;
            float minDist = 0;
            Vertex pVert = null;
            bool bFindShortestPath = false;

            resetAllVerts(startId);

            this.mClosedVec.Add(startId);

            // 总共就 nVerts 个顶点，第 startId 个已经确认，只需要确认剩下的 nVerts - 1 就可以了
            for (openVertIdx = 1; openVertIdx < this.mVertsCount; ++openVertIdx)
            {
                // 要遍历 m_closedVec 中的点，不能只遍历 minIdx 附近的点，可能有些路径开始权重比较小，后面权重比较大，但是有些路径开始权重比较大，后期权重比较小
                if (findNextClosedVert(ref minDist, ref minIdx, this.mClosedVec))   // 如果查找到了下一个最短的未确认的索引
                {
                    // 注意起始顶点和第二个顶点之间是没有 mNearestVert ，因此需要手工将第一个顶点放到路径列表中去
                    pVert = this.mVertsVec[minIdx];
                    pVert.mState = State.Closed; // 将目前找到的最近的顶点置为 State::Closed 
                    this.mClosedVec.Add(minIdx);

                    modifyVertsDist(ref minDist, ref minIdx);
                }
                else                // 如果找不到下一个最小的顶点索引，就直接退出吧，没有最短距离
                {
                    bFindShortestPath = false;
                    break;
                }

                if (minIdx == endId)            // 如果查找到
                {
                    bFindShortestPath = true;
                    break;
                }
            }

            if (bFindShortestPath)
            {
                buildPath(this.mEndVert);   // 生成路径列表
                smoothPath();
                convVertList2VertIdVec(this.mPathCache.getAndAddPathCache(startId, endId).mVertsIdVec);        // 缓存目录
            }
        }

        public List<Vertex> getOrCreateShortestPath(int startId, int endId)
        {
            if (this.mPathList.Count == 0)
            {
                createShortestPath(startId, endId);
            }

            return this.mPathList;
        }

        protected void convVertIdVec2VertList(List<int> vertsIdVec)
        {
            this.mPathList.Clear();
            foreach (var vertId in vertsIdVec)
            {
                this.mPathList.Add(this.mVertsVec[vertId]);
            }
        }

        protected void convVertList2VertIdVec(List<int> vertsIdVec)
        {
            foreach (var vert in this.mPathList)
            {
                vertsIdVec.Add(vert.mId);
            }
        }

        public bool isPathCacheValid(int startId, int endId)
        {
            return this.mPathCache.isPathValid(startId, endId);
        }

        public List<Vertex> getShortestPathFromPathCache(int startId, int endId)
        {
            if (isPathCacheValid(startId, endId))
            {
                convVertIdVec2VertList(this.mPathCache.getPathCache(startId, endId).mVertsIdVec);
            }
            else
            {
                this.mPathList.Clear();
            }

            return this.mPathList;
        }

        public void clearPath()
        {
            this.mPathList.Clear();
        }

        public void clearAllStopPoint()
        {
            foreach (var vert in this.mVertsVec)
            {
                if (vert.mStopPoint != null)      // 可能通过 mId2StopPtMap[idx] 导致添加数据，因此这里判断
                {
                    setNeighborInvalidByVertId(vert.mId);
                    vert.mStopPoint = null;
                }
            }
        }

        protected void smoothPath()
        {
            this.mSmoothPathList.Clear();

            if (this.mPathList.Count > 2)
            {
                this.mSmoothPathList.Add(this.mPathList[0]);

                Vertex startVert = null;
                Vertex endVert = null;
                startVert = this.mPathList[0];
                endVert = this.mPathList[1];

                int idx = 0;

                for (idx = 2; idx < this.mPathList.Count; ++idx)
                {
                    if (isStraightBetweenVert(startVert, this.mPathList[idx]))
                    {
                        endVert = this.mPathList[idx];
                        if (1 == Math.Abs(this.mPathList.Count - idx))
                        {
                            this.mSmoothPathList.Add(endVert);
                        }
                    }
                    else
                    {
                        this.mSmoothPathList.Add(endVert);
                        startVert = endVert;

                        if (Math.Abs(this.mPathList.Count - idx) > 1)
                        {
                            endVert = this.mPathList[idx];
                        }
                        else
                        {
                            this.mSmoothPathList.Add(this.mPathList[idx]);
                        }
                    }
                }
            }
            else
            {
                this.mSmoothPathList.AddRange(this.mPathList);
            }
        }

        protected bool isStraightBetweenVert(Vertex startVert, Vertex endVert)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;

            int curId = 0;

            convVertIdToXY(startVert.mId, ref startX, ref startY);
            convVertIdToXY(endVert.mId, ref endX, ref endY);

            minX = Math.Min(startX, endX);
            minY = Math.Min(startY, endY);
            maxX = Math.Max(startX, endX);
            maxY = Math.Max(startY, endY);

            for (int yIdx = minY; yIdx <= maxY; ++yIdx)
            {
                for (int xIdx = minX; xIdx <= maxX; ++xIdx)
                {
                    curId = convXYToVertId(xIdx, yIdx);
                    if (this.mVertsVec[curId].mStopPoint != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}