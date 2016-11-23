using System;
using System.Collections.Generic;

namespace SDK.Lib
{
    // �赲��
    public class MStopPoint
    {
        public MStopPoint()
        {

        }

        public void dispose()
        {

        }
    }

    // ��ǰ�����״̬
    public enum State
    {
        Closed,         // ����Ѿ�ȷ���˶��㣬�������״̬
        Open,           // ����Ѿ����������ǻ�û��ȷ�Ͼ������״̬
        Unknown         // �����״̬�������״̬
    }

    // ��������
    public class Vertex
    {
        public int mId;
        public State mState;
        public Vertex mNearestVert;
        public float mDistance;
        public bool mIsNeighborValid;      // �ھ������Ƿ���Ч����Ϊ���ܶ�̬�޸��赲��
        public List<int> mVertsIdVec;          // �����ھӶ��� Id�������ֵֻ����ʹ�õ�ʱ��Żᶯ̬���ɣ���ʼ����ʱ�򲢲�����
        public MStopPoint mStopPoint;            // �赲����Ϣ

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
        protected List<Vertex> mVertsVec;      // ���еĶ��㣬������ʱ�����еĶ���ȫ��������������Ҫ��ʱ���ٴ����������Ҫ��ʱ���ٴ���������Ҫ�����ж�
        protected int mVertsCount;           // �����ܹ�����
        protected int mXCount;               // X ��������
        protected int mYCount;               // Y ��������
        protected float mGridWidth;          // ���ӿ��
        protected float mGridHeight;         // ���Ӹ߶�

        // Dijkstra �㷨��Ҫ������
        protected Vertex mStartVert;
        protected Vertex mEndVert;

        // ����·���б�
        protected List<Vertex> mPathList;  // ʹ�� List ����Ҫ��ʹ�� push_front ����ӿ�
        protected List<Vertex> mSmoothPathList;
                                            // ��������Ҫ�õ� 8 ���ھӶ�������
        protected int[] mNeighborVertIdArr;
        protected List<int> mClosedVec;   // �Ѿ�ȷ�ϵĶ����б�

        // ·�������б�
        protected PathCache mPathCache;

        // ��ȡ�ھ���Ϣ�ĸ�������
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
         * @brief ��ʼ��
         * @xCount ���ӵ�������ע�⣬���������� xCount + 1������ power(2, n) + 1 ��������Ƕ�������
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

        // ת������� Id ����������
        public void convVertIdToXY(int vertId, ref int x, ref int y)
        {
            y = vertId / this.mXCount;
            x = vertId - y * this.mXCount;
        }

        public int convXYToVertId(int x, int y)
        {
            return (y * this.mXCount + x);
        }

        // �Ƿ����赲����
        public bool isInStopPt(int nx, int ny)
        {
            if (nx >= 0 && nx < this.mXCount
                && ny >= 0 && ny < this.mYCount)
            {
                int index = ny * this.mXCount + nx;
                if (this.mVertsVec[index].mStopPoint != null)         // ������赲��
                {
                    return true;
                }
            }

            return false;
        }

        /**
        * @brief �ھӸ��ӳɱ�
        * @param vertId ��ʼ���� Id
        * @Param neighborVertId �ھӶ��� Id
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
            float neighborCost = float.MaxValue;            // Ĭ�������ֵ

            if (vertId == neighborVertId)       // ������Լ����ͷ��� 0
            {
                return 0;
            }

            convVertIdToXY(neighborVertId, ref xNeighbor, ref yNeighbor);
            if (isInStopPt(xNeighbor, yNeighbor))       // ����ھ����赲����
            {
                return neighborCost;
            }

            convVertIdToXY(vertId, ref x, ref y);
            if (Math.Abs((long)(xNeighbor - x)) > 1 || Math.Abs((long)(yNeighbor - y)) > 1) // ������� 1 ����˵���м��м��������ֱ�ӵ���
            {
                return neighborCost;
            }

            int nx = 0;
            int ny = 0;

            for (int i = 0; i < 8; ++i)
            {
                nx = x + m_dx[i];
                ny = y + m_dy[i];

                if (convXYToVertId(nx, ny) == neighborVertId)       // ����������ھ�
                {
                    // �϶������赲���У���Ϊ������赲���У������Ѿ��ж���
                    if (isHorizontalOrVerticalNeighbor(vertId, neighborVertId))     // �����ˮƽ���ߴ�ֱ����б��
                    {
                        neighborCost = this.mCost[i];
                    }
                    else
                    {
                        // ��Ҫ�ж�б���ϵ���һ��б�ߵ����������Ƿ����赲��
                        if (!isInStopPt(x, yNeighbor) && !isInStopPt(xNeighbor, y))     // ����Խ����ϵ��������Ӷ������赲��
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

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 1))         // б��
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

            if ((Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 0) ||       // ˮƽ
                (Math.Abs((long)(xNeighbor - x)) == 0 && Math.Abs((long)(yNeighbor - y)) == 1) ||       // ��ֱ
                (Math.Abs((long)(xNeighbor - x)) == 1 && Math.Abs((long)(yNeighbor - y)) == 1))         // б��
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

            // ���� 8 ���ھӶ���
            for (int i = 0; i < 8; ++i)
            {
                nx = x + m_dx[i];
                ny = y + m_dy[i];

                if (nx >= 0 && nx < this.mXCount &&
                    ny >= 0 && ny < this.mYCount)       // ����ھӶ����ڷ�Χ��
                {
                    if (!isInStopPt(nx, ny))        // ��������赲����
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
            // ��Ҫ�޸��ھ���������������������ھ�
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
            int iy = (int)(fx / this.mGridHeight);

            centerX = ix * this.mGridWidth + this.mGridWidth / 2;
            centerY = ix * this.mGridHeight + this.mGridHeight / 2;
        }
    }
}