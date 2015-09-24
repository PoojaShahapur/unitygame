using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    // 阻挡点
    public class StopPoint
    {
        public StopPoint()
        {

        }

        public void dispose()
        {

        }
    }

    // 当前顶点的状态
    public enum State
    {
        Closed,         // 如果已经确认了顶点，就是这个状态
        Open,           // 如果已经遍历，但是还没有确认就是这个状态
        Unknown         // 最初的状态就是这个状态
    }

    // 顶点数据
    public class Vertex
    {
        public int m_id;
        public State m_state;
        public Vertex m_nearestVert;
        public float m_distance;
        public bool m_bNeighborValid;      // 邻居数据是否有效，因为可能动态修改阻挡点
        public List<int> m_vertsIdVec;          // 保存邻居顶点 Id，这个数值只有在使用的时候才会动态生成，初始化的时候并不生成
        public StopPoint m_pStopPoint;            // 阻挡点信息

        public Vertex()
        {
            m_vertsIdVec = new List<int>();
            reset();
        }

        public void dispose()
        {
            if (m_pStopPoint != null)
            {
                m_pStopPoint = null;
            }
        }

        public void reset()
        {
            //m_id = 0;
            m_state = State.Unknown;
            m_nearestVert = null;
            m_distance = float.MaxValue;
            m_bNeighborValid = false;
            m_pStopPoint = null;
            m_vertsIdVec.Clear();
        }

        public void setNeighborVertsId(int[] neighborVertIdArr, int len = 8)
        {
            m_vertsIdVec.Clear();
            m_bNeighborValid = true;
            for (int idx = 0; idx < len; ++idx)
            {
                if (neighborVertIdArr[idx] != -1)
                {
                    m_vertsIdVec.Add(neighborVertIdArr[idx]);
                }
            }
        }
    }

    public partial class MGraph
    {
        protected List<Vertex> m_vertsVec;      // 所有的顶点，启动的时候，所有的顶点全部创建，不是需要的时候再创建，如果需要的时候再创建，就需要各种判断
        protected int m_vertsCount;           // 定点总共数量
        protected int m_xCount;               // X 顶点数量
        protected int m_yCount;               // Y 顶点数量
        protected float m_gridWidth;          // 格子宽度
        protected float m_gridHeight;         // 格子高度

        // Dijkstra 算法需要的数据
        protected Vertex m_startVert;
        protected Vertex m_endVert;

        // 最终路径列表
        protected List<Vertex> m_pathList;  // 使用 List ，主要是使用 push_front 这个接口
        protected List<Vertex> m_smoothPathList;
                                            // 计算中需要用的 8 个邻居顶点索引
        protected int[] m_neighborVertIdArr;
        protected List<int> m_closedVec;   // 已经确认的队列列表

        // 路径缓存列表
        protected PathCache m_pathCache;

        // 获取邻居信息的辅助数据
        protected int[] m_dx;
        protected int[] m_dy;
        protected float[] m_cost;

        public MGraph()
        {
            m_vertsVec = new List<Vertex>();
            m_pathList = new List<Vertex>();
            m_smoothPathList = new List<Vertex>();
            m_neighborVertIdArr = new int[8];
            m_closedVec = new List<int>();
            m_pathCache = new PathCache();

            m_dx = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };
            m_dy = new int[8] { -1, -1, 1, 0, 0, 1, 1, 1 };
            m_cost = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        }

        public void dispose()
        {

        }


        public Vertex getVertexById(int vertId)
        {
            if (vertId < m_vertsVec.Count)
            {
                return m_vertsVec[vertId];
            }

            return null;
        }

        public List<Vertex> getVertsVec()
        {
            return m_vertsVec;
        }

        public int getVertsCount()
        {
            return m_vertsVec.Count;
        }

        public void init(int xCount, int yCount, float gridWidth, float gridHeight)
        {
            m_xCount = xCount;
            m_yCount = yCount;
            m_gridWidth = gridWidth;
            m_gridHeight = gridHeight;

            m_vertsCount = m_xCount * m_yCount;

            int idx = 0;
            Vertex pVertex = null;

            for (idx = 0; idx < m_vertsCount; ++idx)
            {
                pVertex = new Vertex();
                m_vertsVec.Add(pVertex);
                pVertex.reset();
                pVertex.m_id = idx;
            }
        }

        // 转换顶点的 Id 到顶点索引
        public void convVertIdToXY(int vertId, ref int x, ref int y)
        {
            y = vertId / m_xCount;
            x = vertId - y * m_xCount;
        }

        public int convXYToVertId(int x, int y)
        {
            return (y * m_xCount + x);
        }

        // 是否在阻挡点内
        public bool isInStopPt(int nx, int ny)
        {
            if (nx >= 0 && nx < m_xCount
                && ny >= 0 && ny < m_yCount)
            {
                int index = ny * m_xCount + nx;
                if (m_vertsVec[index].m_pStopPoint != null)         // 如果有阻挡点
                {
                    return true;
                }
            }

            return false;
        }

        /**
        * @brief 邻居格子成本
        * @param vertId 起始顶点 Id
        * @Param neighborVertId 邻居顶点 Id
        *	0	1	2
        *	3		4
        *	5	6	7
        */
        protected float adjacentCost(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;
            float neighborCost = float.MaxValue;            // 默认是最大值

            if (vertId == neighborVertId)       // 如果是自己，就返回 0
            {
                return 0;
            }

            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);
            if (isInStopPt(xNeighbor, yNeighbor))       // 如果邻居在阻挡点中
            {
                return neighborCost;
            }

            convVertIdToXY(vertId, ref x, ref y);
            if (Math.Abs((long)(xNeighbor - x)) > 1 || Math.Abs((long)(yNeighbor - y)) > 1) // 如果相差不是 1 ，就说明中间有间隔，不能直接到达
            {
                return neighborCost;
            }

            int nx = 0;
            int ny = 0;

            for (int i = 0; i < 8; ++i)
            {
                nx = x + m_dx[i];
                ny = y + m_dy[i];

                if (convXYToVertId(nx, ny) == neighborVertId)       // 如果正好是邻居
                {
                    // 肯定不在阻挡点中，因为如果在阻挡点中，上面已经判断了
                    if (isHorizontalOrVerticalNeighbor(vertId, neighborVertId))     // 如果是水平或者垂直，是斜线
                    {
                        neighborCost = m_cost[i];
                    }
                    else
                    {
                        // 需要判断斜线上的另一个斜线的两个格子是否是阻挡点
                        if (!isInStopPt(x, yNeighbor) && !isInStopPt(xNeighbor, y))     // 如果对角线上的两个格子都不是阻挡点
                        {
                            neighborCost = m_cost[i];
                        }
                    }

                    break;
                }
            }

            return neighborCost;
        }

        public void addStopPoint(int nx, int ny, StopPoint pStopPoint)
        {
            int vertId = convXYToVertId(nx, ny);
            m_vertsVec[vertId].m_pStopPoint = pStopPoint;

            setNeighborInvalidByVertId(vertId);
        }

        protected bool isHorizontalOrVerticalNeighbor(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 0) ||
                (Math.Abs((long)(xNeighbor - x)) == 0 && Math.Abs((long)(yNeighbor - y)) == 1))
            {
                return true;
            }

            return false;
        }

        protected bool isHorizontalNeighbor(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 0))
            {
                return true;
            }

            return false;
        }

        protected bool isVerticalNeighbor(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if ((Math.Abs((long)(xNeighbor - x)) == 0 && Math.Abs((long)(yNeighbor - y)) == 1))
            {
                return true;
            }

            return false;
        }

        protected bool isSlashNeighbor(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 1))         // 斜线
            {
                return true;
            }

            return false;
        }

        protected bool isNeighbor(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 0) ||       // 水平
                (Math.Abs((long)(xNeighbor - x)) == 0 && Math.Abs((long)(yNeighbor - y)) == 1) ||       // 垂直
                (Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 1))         // 斜线
            {
                return true;
            }

            return false;
        }

        protected bool isBackSlashStopPoint(int vertId, int neighborVertId)
        {
            int x = 0;
            int y = 0;
            int xNeighbor = 0;
            int yNeighbor = 0;

            convVertIdToXY(vertId, ref x, ref y);
            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);

            if (isInStopPt(x, yNeighbor) || isInStopPt(xNeighbor, y))
            {
                return true;
            }

            return false;
        }

        protected void findNeighborVertIdArr(int vertId)
        {
            int x = 0;
            int y = 0;
            convVertIdToXY(vertId, ref x, ref y);

            int nx = 0;
            int ny = 0;

            // 遍历 8 个邻居顶点
            for (int i = 0; i < 8; ++i)
            {
                nx = x + m_dx[i];
                ny = y + m_dy[i];

                if (nx >= 0 && nx < m_xCount &&
                    ny >= 0 && ny < m_yCount)       // 如果邻居顶点在范围内
                {
                    if (!isInStopPt(nx, ny))        // 如果不在阻挡点内
                    {
                        m_neighborVertIdArr[i] = convXYToVertId(nx, ny);
                    }
                    else
                    {
                        m_neighborVertIdArr[i] = -1;
                    }
                }
                else
                {
                    m_neighborVertIdArr[i] = -1;
                }
            }
        }

        protected void setNeighborInvalidByVertId(int vertId)
        {
            // 需要修改邻居是这个顶点的其它顶点的邻居
            if (!m_vertsVec[vertId].m_bNeighborValid)
            {
                findNeighborVertIdArr(vertId);
                m_vertsVec[vertId].setNeighborVertsId(m_neighborVertIdArr);
            }

            for (int neighborIdx = 0; neighborIdx < m_vertsVec[vertId].m_vertsIdVec.Count; ++neighborIdx)
            {
                m_vertsVec[m_vertsVec[vertId].m_vertsIdVec[neighborIdx]].m_bNeighborValid = false;
            }
        }

        public Vertex getVertexByPos(float fx, float fy)
        {
            int ix = (int)(fx / m_gridWidth);
            int iy = (int)(fx / m_gridHeight);

            return m_vertsVec[convXYToVertId(ix, iy)];
        }

        public void getVertexCenterByPos(float fx, float fy, ref float centerX, ref float centerY)
        {
            int ix = (int)(fx / m_gridWidth);
            int iy = (int)(fx / m_gridHeight);

            centerX = ix * m_gridWidth + m_gridWidth / 2;
            centerY = ix * m_gridHeight + m_gridHeight / 2;
        }
    }
}