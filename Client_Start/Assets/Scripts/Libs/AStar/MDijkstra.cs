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
            this.mPathList.Insert(0, this.mStartVert);      // ������Ķ���Ž�ȥ
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
            // ��ʼ������
            for (int vertIdx = 0; vertIdx < this.mVertsCount; ++vertIdx)
            {
                pVert = this.mVertsVec[vertIdx];
                pVert.mState = State.Unknown;    // ȫ�������ʼ��Ϊδ֪�Զ�·��״̬
                pVert.mDistance = adjacentCost(startId, vertIdx); //���� startId �������ߵĶ������Ȩֵ
                pVert.mNearestVert = null;    // ��ʼ��·����ǰһ������
            }

            this.mStartVert.mDistance = 0;    //  startId �� startId ·��Ϊ0
            this.mStartVert.mState = State.Closed;    // m_startVert->mState ��ʾ startId �� startId ����Ҫ��·�����Ѿ���ӵ�ȷ�϶�������
        }

        protected bool findNextClosedVert(ref float minDist, ref int minIdx, List<int> closedVec)
        {
            Vertex pVert = null;
            int closedIdx = 0;
            int neighborVertIdx = 0;
            minDist = float.MaxValue;    // ��ǰ��֪�� startId �����������
            bool bFindNextClosedVert = false;

            // ֻҪ������Χ 8 ������Ϳ����ˣ���Ϊ����Ѱ·ֻ���Ǻ��Լ���Χ�� 8 �����Ӳ���Ȩ�أ��������ĸ�����û��Ȩ�ص�
            for (closedIdx = 0; closedIdx < closedVec.Count; ++closedIdx)
            {
                if (!this.mVertsVec[closedVec[closedIdx]].mIsNeighborValid)     // ����ھ���������Ч��
                {
                    findNeighborVertIdArr(closedVec[closedIdx]);
                    this.mVertsVec[closedVec[closedIdx]].setNeighborVertsId(this.mNeighborVertIdArr);
                }

                for (neighborVertIdx = 0; neighborVertIdx < this.mVertsVec[closedVec[closedIdx]].mVertsIdVec.Count; ++neighborVertIdx)
                {
                    pVert = this.mVertsVec[this.mVertsVec[closedVec[closedIdx]].mVertsIdVec[neighborVertIdx]];
                    if (pVert.mState != State.Closed && pVert.mDistance < minDist)
                    {
                        minDist = pVert.mDistance; // w������ startId �������
                        minIdx = pVert.mId;
                        bFindNextClosedVert = true;             // ˵�����ҵ���
                    }
                }
            }

            return bFindNextClosedVert;
        }

        protected void modifyVertsDist(ref float minDist, ref int minIdx)
        {
            int neighborVertIdx = 0;
            Vertex pVert = null;

            // ֻ��Ҫ�������¼��� closed �Ķ�����ھӶ���
            if (!this.mVertsVec[minIdx].mIsNeighborValid)       // ����ھ���������Ч��
            {
                findNeighborVertIdArr(minIdx);
                this.mVertsVec[minIdx].setNeighborVertsId(this.mNeighborVertIdArr);
            }
            for (neighborVertIdx = 0; neighborVertIdx < this.mVertsVec[minIdx].mVertsIdVec.Count; ++neighborVertIdx) // ������ǰ���·������
            {
                pVert = this.mVertsVec[this.mVertsVec[minIdx].mVertsIdVec[neighborVertIdx]];
                // �������V�����·������������·���ĳ��ȶ̵Ļ�
                if (pVert.mState != State.Closed && (minDist + adjacentCost(minIdx, pVert.mId) < pVert.mDistance))
                {
                    // ˵���ҵ�����̵�·�����޸�D[w] �� p[w]
                    pVert.mDistance = minDist + adjacentCost(minIdx, pVert.mId); // �޸ĵ�ǰ·������
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

            int openVertIdx = 0;        // ��δȷ����ӵ������еĶ������������
            int minIdx = 0;
            float minDist = 0;
            Vertex pVert = null;
            bool bFindShortestPath = false;

            resetAllVerts(startId);

            this.mClosedVec.Add(startId);

            // �ܹ��� nVerts �����㣬�� startId ���Ѿ�ȷ�ϣ�ֻ��Ҫȷ��ʣ�µ� nVerts - 1 �Ϳ�����
            for (openVertIdx = 1; openVertIdx < this.mVertsCount; ++openVertIdx)
            {
                // Ҫ���� m_closedVec �еĵ㣬����ֻ���� minIdx �����ĵ㣬������Щ·����ʼȨ�رȽ�С������Ȩ�رȽϴ󣬵�����Щ·����ʼȨ�رȽϴ󣬺���Ȩ�رȽ�С
                if (findNextClosedVert(ref minDist, ref minIdx, this.mClosedVec))   // ������ҵ�����һ����̵�δȷ�ϵ�����
                {
                    // ע����ʼ����͵ڶ�������֮����û�� mNearestVert �������Ҫ�ֹ�����һ������ŵ�·���б���ȥ
                    pVert = this.mVertsVec[minIdx];
                    pVert.mState = State.Closed; // ��Ŀǰ�ҵ�������Ķ�����Ϊ State::Closed 
                    this.mClosedVec.Add(minIdx);

                    modifyVertsDist(ref minDist, ref minIdx);
                }
                else                // ����Ҳ�����һ����С�Ķ�����������ֱ���˳��ɣ�û����̾���
                {
                    bFindShortestPath = false;
                    break;
                }

                if (minIdx == endId)            // ������ҵ�
                {
                    bFindShortestPath = true;
                    break;
                }
            }

            if (bFindShortestPath)
            {
                buildPath(this.mEndVert);   // ����·���б�
                smoothPath();
                convVertList2VertIdVec(this.mPathCache.getAndAddPathCache(startId, endId).mVertsIdVec);        // ����Ŀ¼
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
                if (vert.mStopPoint != null)      // ����ͨ�� mId2StopPtMap[idx] ����������ݣ���������ж�
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