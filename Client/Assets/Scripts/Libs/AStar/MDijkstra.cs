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
                m_pathList.Insert(0, vert);
                vert = vert.m_nearestVert;
            }
            m_pathList.Insert(0, m_startVert);      // 把最初的顶点放进去
        }

        protected void initVerts(int startId, int endId)
        {
            m_startVert = null;
            m_endVert = null;

            int nx = 0;
            int ny = 0;

            convVertIdToXY(startId, ref nx, ref ny);
            if (nx >= 0 && nx < m_xCount
                && ny >= 0 && ny < m_yCount)
            {
                m_startVert = m_vertsVec[startId];
            }

            convVertIdToXY(endId, ref nx, ref ny);
            if (nx >= 0 && nx < m_xCount
                && ny >= 0 && ny < m_yCount)
            {
                m_endVert = m_vertsVec[endId];
            }

            if (m_startVert == null || m_endVert == null)
            {
                throw new Exception("Failed to find the start/end node(s)!");
            }


            m_startVert.m_distance = 0;
        }

        protected void resetAllVerts(int startId)
        {
            Vertex pVert = null;
            // 初始化数据
            for (int vertIdx = 0; vertIdx < m_vertsCount; ++vertIdx)
            {
                pVert = m_vertsVec[vertIdx];
                pVert.m_state = State.Unknown;    // 全部顶点初始化为未知对短路径状态
                pVert.m_distance = adjacentCost(startId, vertIdx); //将与 startId 点有连线的顶点加上权值
                pVert.m_nearestVert = null;    // 初始化路径的前一个顶点
            }

            m_startVert.m_distance = 0;    //  startId 至 startId 路径为0
            m_startVert.m_state = State.Closed;    // m_startVert->m_state 表示 startId 至 startId 不需要求路径，已经添加到确认队列中了
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
                if (!m_vertsVec[closedVec[closedIdx]].m_bNeighborValid)     // 如果邻居数据是无效的
                {
                    findNeighborVertIdArr(closedVec[closedIdx]);
                    m_vertsVec[closedVec[closedIdx]].setNeighborVertsId(m_neighborVertIdArr);
                }

                for (neighborVertIdx = 0; neighborVertIdx < m_vertsVec[closedVec[closedIdx]].m_vertsIdVec.Count; ++neighborVertIdx)
                {
                    pVert = m_vertsVec[m_vertsVec[closedVec[closedIdx]].m_vertsIdVec[neighborVertIdx]];
                    if (pVert.m_state != State.Closed && pVert.m_distance < minDist)
                    {
                        minDist = pVert.m_distance; // w顶点离 startId 顶点更近
                        minIdx = pVert.m_id;
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
            if (!m_vertsVec[minIdx].m_bNeighborValid)       // 如果邻居数据是无效的
            {
                findNeighborVertIdArr(minIdx);
                m_vertsVec[minIdx].setNeighborVertsId(m_neighborVertIdArr);
            }
            for (neighborVertIdx = 0; neighborVertIdx < m_vertsVec[minIdx].m_vertsIdVec.Count; ++neighborVertIdx) // 修正当前最短路径距离
            {
                pVert = m_vertsVec[m_vertsVec[minIdx].m_vertsIdVec[neighborVertIdx]];
                // 如果经过V顶点的路径比现在这条路径的长度短的话
                if (pVert.m_state != State.Closed && (minDist + adjacentCost(minIdx, pVert.m_id) < pVert.m_distance))
                {
                    // 说明找到了最短的路径，修改D[w] 和 p[w]
                    pVert.m_distance = minDist + adjacentCost(minIdx, pVert.m_id); // 修改当前路径长度
                    pVert.m_nearestVert = m_vertsVec[minIdx];
                }
            }
        }

        public List<Vertex> getShortestPath()
        {
            return m_pathList;
        }

        public void createShortestPath(int startId, int endId)
        {
            m_pathList.Clear();
            m_closedVec.Clear();

            initVerts(startId, endId);

            int openVertIdx = 0;        // 还未确定添加到队列中的顶点遍历的索引
            int minIdx = 0;
            float minDist = 0;
            Vertex pVert = null;
            bool bFindShortestPath = false;

            resetAllVerts(startId);

            m_closedVec.Add(startId);

            // 总共就 nVerts 个顶点，第 startId 个已经确认，只需要确认剩下的 nVerts - 1 就可以了
            for (openVertIdx = 1; openVertIdx < m_vertsCount; ++openVertIdx)
            {
                // 要遍历 m_closedVec 中的点，不能只遍历 minIdx 附近的点，可能有些路径开始权重比较小，后面权重比较大，但是有些路径开始权重比较大，后期权重比较小
                if (findNextClosedVert(ref minDist, ref minIdx, m_closedVec))   // 如果查找到了下一个最短的未确认的索引
                {
                    // 注意起始顶点和第二个顶点之间是没有 m_nearestVert ，因此需要手工将第一个顶点放到路径列表中去
                    pVert = m_vertsVec[minIdx];
                    pVert.m_state = State.Closed; // 将目前找到的最近的顶点置为 State::Closed 
                    m_closedVec.Add(minIdx);

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
                buildPath(m_endVert);   // 生成路径列表
                smoothPath();
                convVertList2VertIdVec(m_pathCache.getAndAddPathCache(startId, endId).m_vertsIdVec);        // 缓存目录
            }
        }

        public List<Vertex> getOrCreateShortestPath(int startId, int endId)
        {
            if (m_pathList.Count == 0)
            {
                createShortestPath(startId, endId);
            }

            return m_pathList;
        }

        protected void convVertIdVec2VertList(List<int> vertsIdVec)
        {
            m_pathList.Clear();
            foreach (var vertId in vertsIdVec)
            {
                m_pathList.Add(m_vertsVec[vertId]);
            }
        }

        protected void convVertList2VertIdVec(List<int> vertsIdVec)
        {
            foreach (var vert in m_pathList)
            {
                vertsIdVec.Add(vert.m_id);
            }
        }

        public bool isPathCacheValid(int startId, int endId)
        {
            return m_pathCache.isPathValid(startId, endId);
        }

        public List<Vertex> getShortestPathFromPathCache(int startId, int endId)
        {
            if (isPathCacheValid(startId, endId))
            {
                convVertIdVec2VertList(m_pathCache.getPathCache(startId, endId).m_vertsIdVec);
            }
            else
            {
                m_pathList.Clear();
            }

            return m_pathList;
        }

        public void clearPath()
        {
            m_pathList.Clear();
        }

        public void clearAllStopPoint()
        {
            foreach (var vert in m_vertsVec)
            {
                if (vert.m_pStopPoint != null)      // 可能通过 m_id2StopPtMap[idx] 导致添加数据，因此这里判断
                {
                    setNeighborInvalidByVertId(vert.m_id);
                    vert.m_pStopPoint = null;
                }
            }
        }

        protected void smoothPath()
        {
            m_smoothPathList.Clear();

            if (m_pathList.Count > 2)
            {
                m_smoothPathList.Add(m_pathList[0]);

                Vertex startVert = null;
                Vertex endVert = null;
                startVert = m_pathList[0];
                m_pathList.RemoveAt(0);
                endVert = m_pathList[0];
                m_pathList.RemoveAt(0);

                int idx = 0;

                for (idx = 0; idx < m_pathList.Count; ++idx)
                {
                    if (isStraightBetweenVert(startVert, m_pathList[idx]))
                    {
                        endVert = m_pathList[idx];
                        if (1 == Math.Abs(m_pathList.Count - idx))
                        {
                            m_smoothPathList.Add(endVert);
                        }
                    }
                    else
                    {
                        m_smoothPathList.Add(endVert);
                        startVert = endVert;

                        if (Math.Abs(m_pathList.Count - idx) > 1)
                        {
                            endVert = m_pathList[idx];
                        }
                        else
                        {
                            m_smoothPathList.Add(m_pathList[idx]);
                        }
                    }
                }
            }
            else
            {
                m_smoothPathList.AddRange(m_pathList);
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

            convVertIdToXY(startVert.m_id, ref startX, ref startY);
            convVertIdToXY(endVert.m_id, ref endX, ref endY);

            minX = Math.Min(startX, endX);
            minY = Math.Min(startY, endY);
            maxX = Math.Max(startX, endX);
            maxY = Math.Max(startY, endY);

            for (int yIdx = minY; yIdx <= maxY; ++yIdx)
            {
                for (int xIdx = minX; xIdx <= maxX; ++xIdx)
                {
                    curId = convXYToVertId(xIdx, yIdx);
                    if (m_vertsVec[curId].m_pStopPoint != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}