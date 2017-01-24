using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    // 阻挡点
    public class MStopPoint
    {
        public MStopPoint()
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
        public int mId;
        public State mState;
        public Vertex mNearestVert;
        public float mDistance;
        public bool mIsNeighborValid;      // 邻居数据是否有效，因为可能动态修改阻挡点
        public List<int> mVertsIdVec;          // 保存邻居顶点 Id，这个数值只有在使用的时候才会动态生成，初始化的时候并不生成
        public MStopPoint mStopPoint;            // 阻挡点信息

        public Vertex()
        {
            mVertsIdVec = new List<int>();
            reset();
        }

        public void dispose()
        {
            if (mStopPoint != null)
            {
                mStopPoint = null;
            }
        }

        public void reset()
        {
            //mId = 0;
            mState = State.Unknown;
            mNearestVert = null;
            mDistance = float.MaxValue;
            mIsNeighborValid = false;
            mStopPoint = null;
            mVertsIdVec.Clear();
        }

        public void setNeighborVertsId(int[] neighborVertIdArr, int len = 8)
        {
            mVertsIdVec.Clear();
            mIsNeighborValid = true;
            for (int idx = 0; idx < len; ++idx)
            {
                if (neighborVertIdArr[idx] != -1)
                {
                    mVertsIdVec.Add(neighborVertIdArr[idx]);
                }
            }
        }
    }

    public partial class MGraph
    {
        protected List<Vertex> mVertsVec;      // 所有的顶点，启动的时候，所有的顶点全部创建，不是需要的时候再创建，如果需要的时候再创建，就需要各种判断
        protected int mVertsCount;           // 定点总共数量
        protected int mXCount;               // X 顶点数量
        protected int mYCount;               // Y 顶点数量
        protected float mGridWidth;          // 格子宽度
        protected float mGridHeight;         // 格子高度

        // Dijkstra 算法需要的数据
        protected Vertex mStartVert;
        protected Vertex mEndVert;

        // 最终路径列表
        protected List<Vertex> mPathList;  // 使用 List ，主要是使用 push_front 这个接口
        protected List<Vertex> mSmoothPathList;
                                            // 计算中需要用的 8 个邻居顶点索引
        protected int[] mNeighborVertIdArr;
        protected List<int> mClosedVec;   // 已经确认的队列列表

        // 路径缓存列表
        protected PathCache mPathCache;

        // 获取邻居信息的辅助数据
        protected int[] m_dx;
        protected int[] m_dy;
        protected float[] mCost;

        public MGraph()
        {
            this.mVertsVec = new List<Vertex>();
            this.mPathList = new List<Vertex>();
            this.mSmoothPathList = new List<Vertex>();
            this.mNeighborVertIdArr = new int[8];
            this.mClosedVec = new List<int>();
            this.mPathCache = new PathCache();

            m_dx = new int[8] { -1, 0, 1, -1, 1, -1, 0, 1 };
            m_dy = new int[8] { -1, -1, 1, 0, 0, 1, 1, 1 };
            this.mCost = new float[8] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
        }

        public void dispose()
        {

        }


        public Vertex getVertexById(int vertId)
        {
            if (vertId < this.mVertsVec.Count)
            {
                return this.mVertsVec[vertId];
            }

            return null;
        }

        public List<Vertex> getVertsVec()
        {
            return this.mVertsVec;
        }

        public int getVertsCount()
        {
            return this.mVertsVec.Count;
        }

        /**
         * @brief 初始化
         * @xCount 格子的数量，注意，顶点数量是 xCount + 1，类似 power(2, n) + 1 ，这个才是顶点数量
         */
        public void init(int xCount, int yCount, float gridWidth, float gridHeight)
        {
            this.mXCount = xCount;
            this.mYCount = yCount;
            this.mGridWidth = gridWidth;
            this.mGridHeight = gridHeight;

            this.mVertsCount = this.mXCount * this.mYCount;

            int idx = 0;
            Vertex pVertex = null;

            for (idx = 0; idx < this.mVertsCount; ++idx)
            {
                pVertex = new Vertex();
                this.mVertsVec.Add(pVertex);
                pVertex.reset();
                pVertex.mId = idx;
            }
        }

        // 转换顶点的 Id 到顶点索引
        public void convVertIdToXY(int vertId, ref int x, ref int y)
        {
            y = vertId / this.mXCount;
            x = vertId - y * this.mXCount;
        }

        public int convXYToVertId(int x, int y)
        {
            return (y * this.mXCount + x);
        }

        // 是否在阻挡点内
        public bool isInStopPt(int nx, int ny)
        {
            if (nx >= 0 && nx < this.mXCount
                && ny >= 0 && ny < this.mYCount)
            {
                int index = ny * this.mXCount + nx;
                if (this.mVertsVec[index].mStopPoint != null)         // 如果有阻挡点
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
                        neighborCost = this.mCost[i];
                    }
                    else
                    {
                        // 需要判断斜线上的另一个斜线的两个格子是否是阻挡点
                        if (!isInStopPt(x, yNeighbor) && !isInStopPt(xNeighbor, y))     // 如果对角线上的两个格子都不是阻挡点
                        {
                            neighborCost = this.mCost[i];
                        }
                    }

                    break;
                }
            }

            return neighborCost;
        }

        public void addStopPoint(int nx, int ny, MStopPoint pStopPoint)
        {
            int vertId = convXYToVertId(nx, ny);
            this.mVertsVec[vertId].mStopPoint = pStopPoint;

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

                if (nx >= 0 && nx < this.mXCount &&
                    ny >= 0 && ny < this.mYCount)       // 如果邻居顶点在范围内
                {
                    if (!isInStopPt(nx, ny))        // 如果不在阻挡点内
                    {
                        this.mNeighborVertIdArr[i] = convXYToVertId(nx, ny);
                    }
                    else
                    {
                        this.mNeighborVertIdArr[i] = -1;
                    }
                }
                else
                {
                    this.mNeighborVertIdArr[i] = -1;
                }
            }
        }

        protected void setNeighborInvalidByVertId(int vertId)
        {
            // 需要修改邻居是这个顶点的其它顶点的邻居
            if (!this.mVertsVec[vertId].mIsNeighborValid)
            {
                findNeighborVertIdArr(vertId);
                this.mVertsVec[vertId].setNeighborVertsId(this.mNeighborVertIdArr);
            }

            for (int neighborIdx = 0; neighborIdx < this.mVertsVec[vertId].mVertsIdVec.Count; ++neighborIdx)
            {
                this.mVertsVec[this.mVertsVec[vertId].mVertsIdVec[neighborIdx]].mIsNeighborValid = false;
            }
        }

        public Vertex getVertexByPos(float fx, float fy)
        {
            int ix = (int)(fx / this.mGridWidth);
            int iy = (int)(fx / this.mGridHeight);

            return this.mVertsVec[convXYToVertId(ix, iy)];
        }

        public void getVertexCenterByPos(float fx, float fy, ref float centerX, ref float centerY)
        {
            int ix = (int)(fx / this.mGridWidth);
            //int iy = (int)(fx / this.mGridHeight);

            centerX = ix * this.mGridWidth + this.mGridWidth / 2;
            centerY = ix * this.mGridHeight + this.mGridHeight / 2;
        }
    }
}