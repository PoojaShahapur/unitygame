using UnityEngine;

namespace SDK.Lib
{
    public enum NeighbourIndex
    {
        NEIGHBOUR_EAST = 0,
        NEIGHBOUR_NORTHEAST = 1,
        NEIGHBOUR_NORTH = 2,
        NEIGHBOUR_NORTHWEST = 3,
        NEIGHBOUR_WEST = 4,
        NEIGHBOUR_SOUTHWEST = 5,
        NEIGHBOUR_SOUTH = 6,
        NEIGHBOUR_SOUTHEAST = 7,

        NEIGHBOUR_COUNT = 8
    };

    public enum Alignment
    {
        ALIGN_X_Z = 0,
        ALIGN_X_Y = 1,
        ALIGN_Y_Z = 2
    }

    public class MTerrain
    {
        protected MSceneNode mRootNode;
        protected bool mIsLoaded;
        protected bool mModified;
        protected bool mHeightDataModified;
        protected float[] mHeightData;
        public Alignment mAlign;
        protected float mWorldSize;
        protected ushort mSize;
        protected ushort mMaxBatchSize;
        protected ushort mMinBatchSize;
        protected MVector3 mPos;
        protected MTerrainQuadTreeNode mQuadTree;
        protected ushort mNumLodLevels;
        protected ushort mNumLodLevelsPerLeafNode;
        protected ushort mTreeDepth;
        protected float mBase;
        protected float mScale;

        protected bool mPrepareInProgress;
        protected MTerrain[] mNeighbours;
        protected HeightMapData m_heightMapData;
        protected TerrainMat mTerrainMat;

        public MTerrain()
        {
            mIsLoaded = false;
            mModified = false;
            mHeightDataModified = false;
            mHeightData = null;
            mAlign = Alignment.ALIGN_X_Z;
            mWorldSize = 0;
            mSize = 0;
            mMaxBatchSize = 0;
            mMinBatchSize = 0;
            mPos = MVector3.ZERO;
            mQuadTree = null;
            mTreeDepth = 0;
            mBase = 0;
            mScale = 0;

            m_heightMapData = new HeightMapData();
            mTerrainMat = new TerrainMat();
        }

        public MVector3 getPosition()
        {
            return mPos;
        }

        public MAxisAlignedBox getAABB()
        {
            if (mQuadTree == null)
                return MAxisAlignedBox.BOX_NULL;
            else
                return mQuadTree.getAABB();
        }

        public MAxisAlignedBox getWorldAABB()
        {
            MMatrix4 m = MMatrix4.IDENTITY;
            m.setTrans(getPosition());

            MAxisAlignedBox ret = getAABB();
            ret.transformAffine(ref m);
            return ret;
        }

        public float getMinHeight()
        {
            if (mQuadTree == null)
                return 0;
            else
                return mQuadTree.getMinHeight();
        }

        public float getMaxHeight()
        {
            if (mQuadTree == null)
                return 0;
            else
                return mQuadTree.getMaxHeight();
        }

        public bool prepare(MImportData importData)
        {
            mPrepareInProgress = true;

            mSize = importData.terrainSize;
            mWorldSize = importData.worldSize;
            mMaxBatchSize = importData.maxBatchSize;
            mMinBatchSize = importData.minBatchSize;
            setPosition(importData.pos);

            updateBaseScale();
            determineLodLevels();

            int numVertices = mSize * mSize;

            mHeightData = new float[numVertices];

            if (!string.IsNullOrEmpty(importData.heightPath))
            {
                m_heightMapData.loadHeightMap(importData.heightPath);

                int srcy = 0;
                float height = 0;
                for (int idy = 0; idy < mSize; ++idy)
                {
                    srcy = mSize - idy - 1;
                    for (int idx = 0; idx < mSize; ++idx)
                    {
                        height = m_heightMapData.getOrigHeight(idx, idy);
                        mHeightData[idy * mSize + idx] = height * importData.inputScale + importData.inputBias;
                    }
                }
            }

            mTerrainMat.setDiffuseMap(importData.diffusePath);
            mTerrainMat.loadDiffuseMat();

            mQuadTree = new MTerrainQuadTreeNode(this, null, 0, 0, mSize, (ushort)(mNumLodLevels - 1), 0, 0);
            mQuadTree.prepare();
            distributeVertexData();

            mModified = true;
            mHeightDataModified = true;

            mPrepareInProgress = false;

            return true;
        }

        public void setPosition(MVector3 pos)
        {
            if (pos != mPos)
            {
                mPos = pos;
                mRootNode.setPosition(pos);

                updateBaseScale();
                mModified = true;
            }
        }

        public void updateBaseScale()
        {
            //mBase = -mWorldSize * 0.5f;
            //mScale = mWorldSize / (float)(mSize - 1);

            mBase = mWorldSize * 0.5f;
            mScale = mWorldSize / (float)(mSize - 1);
        }

        public float[] getHeightData()
        {
            return mHeightData;
        }

        public float getHeightData(long x, long y)
        {
            UtilApi.assert(x >= 0 && x < mSize && y >= 0 && y < mSize);
            return mHeightData[y * mSize + x];
        }
        public float getHeightAtPoint(long x, long y)
        {
            x = UtilMath.min(x, (long)mSize - 1L);
            x = UtilMath.max(x, 0L);
            y = UtilMath.min(y, (long)mSize - 1L);
            y = UtilMath.max(y, 0L);

            long skip = 1;
            long x1 = UtilMath.min((x / skip) * skip, (long)mSize - 1L);
            long x2 = UtilMath.min(((x + skip) / skip) * skip, (long)mSize - 1L);
            long y1 = UtilMath.min((y / skip) * skip, (long)mSize - 1L);
            long y2 = UtilMath.min(((y + skip) / skip) * skip, (long)mSize - 1L);

            float rx = ((float)(x % skip) / skip);
            float ry = ((float)(y % skip) / skip);

            return getHeightData(x1, y1) * (1.0f - rx) * (1.0f - ry)
                + getHeightData(x2, y1) * rx * (1.0f - ry)
                + getHeightData(x1, y2) * (1.0f - rx) * ry
                + getHeightData(x2, y2) * rx * ry;
        }

        public void setHeightAtPoint(long x, long y, float h)
        {
            
        }

        public float getHeightAtTerrainPosition(float x, float y)
        {
            float factor = (float)mSize - 1.0f;
            float invFactor = 1.0f / factor;

            long startX = (long)(x * factor);
            long startY = (long)(y * factor);
            long endX = startX + 1;
            long endY = startY + 1;

            float startXTS = startX * invFactor;
            float startYTS = startY * invFactor;
            float endXTS = endX * invFactor;
            float endYTS = endY * invFactor;

            endX = UtilMath.min(endX, (long)mSize - 1);
            endY = UtilMath.min(endY, (long)mSize - 1);

            float xParam = (x - startXTS) / invFactor;
            float yParam = (y - startYTS) / invFactor;

            MVector3 v0 = new MVector3(startXTS, startYTS, getHeightAtPoint(startX, startY));
            MVector3 v1 = new MVector3(endXTS, startYTS, getHeightAtPoint(endX, startY));
            MVector3 v2 = new MVector3(endXTS, endYTS, getHeightAtPoint(endX, endY));
            MVector3 v3 = new MVector3(startXTS, endYTS, getHeightAtPoint(startX, endY));

            MPlane plane = new MPlane();
            if (startY % 2 != 0)
            {
                bool secondTri = ((1.0 - yParam) > xParam);
                if (secondTri)
                    plane.redefine(ref v0, ref v1, ref v3);
                else
                    plane.redefine(ref v1, ref v2, ref v3);
            }
            else
            {
                bool secondTri = (yParam > xParam);
                if (secondTri)
                    plane.redefine(ref v0, ref v2, ref v3);
                else
                    plane.redefine(ref v0, ref v1, ref v2);
            }

            return (-plane.normal.x * x
                    - plane.normal.y * y
                    - plane.d) / plane.normal.z;
        }

        public float getHeightAtWorldPosition(float x, float y, float z)
        {
            MVector3 terrPos = new MVector3(0, 0, 0);

            getTerrainPosition(x, y, z, ref terrPos);
            return getHeightAtTerrainPosition(terrPos.x, terrPos.y);
        }

        public float getHeightAtWorldPosition(ref MVector3 pos)
        {
            return getHeightAtWorldPosition(pos.x, pos.y, pos.z);
        }

        public void getPoint(long x, long y, ref MVector3 outpos)
        {
            getPointAlign(x, y, mAlign, ref outpos);
        }

        public void getPoint(long x, long y, float height, ref MVector3 outpos)
        {
            getPointAlign(x, y, height, mAlign, ref outpos);
        }

        public void getPointAlign(long x, long y, Alignment align, ref MVector3 outpos)
        {
            getPointAlign(x, y, getHeightData(x, y), align, ref outpos);
        }

        public void getPointAlign(long x, long y, float height, Alignment align, ref MVector3 outpos)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    //outpos.y = height;
                    //outpos.x = x * mScale + mBase;
                    //outpos.z = y * -mScale - mBase;

                    outpos.y = height;
                    outpos.x = x * mScale;
                    outpos.z = y * mScale;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outpos.x = height;
                    outpos.z = x * -mScale - mBase;
                    outpos.y = y * mScale + mBase;
                    break;
                case Alignment.ALIGN_X_Y:
                    outpos.z = height;
                    outpos.x = x * mScale + mBase;
                    outpos.y = y * mScale + mBase;
                    break;
            };
        }

        public void getPointTransform(ref MMatrix4 outXform)
        {
            outXform = MMatrix4.ZERO;
            switch (mAlign)
            {
                case Alignment.ALIGN_X_Z:
                    outXform[1, 2] = 1.0f;
                    outXform[0, 0] = mScale;
                    outXform[0, 3] = mBase;
                    outXform[2, 1] = -mScale;
                    outXform[2, 3] = -mBase;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outXform[0, 2] = 1.0f;
                    outXform[2, 0] = -mScale;
                    outXform[2, 3] = -mBase;
                    outXform[1, 1] = mScale;
                    outXform[1, 3] = mBase;
                    break;
                case Alignment.ALIGN_X_Y:
                    outXform[2, 2] = 1.0f;
                    outXform[0, 0] = mScale;
                    outXform[0, 3] = mBase;
                    outXform[1, 1] = mScale;
                    outXform[1, 3] = mBase;
                    break;
            };
            outXform[3, 3] = 1.0f;
        }

        public void getVector(MVector3 inVec, ref MVector3 outVec)
        {
            getVectorAlign(inVec.x, inVec.y, inVec.z, mAlign, ref outVec);
        }

        public void getVector(float x, float y, float z, ref MVector3 outVec)
        {
            getVectorAlign(x, y, z, mAlign, ref outVec);
        }

        public void getVectorAlign(MVector3 inVec, Alignment align, ref MVector3 outVec)
        {
            getVectorAlign(inVec.x, inVec.y, inVec.z, align, ref outVec);
        }

        public void getVectorAlign(float x, float y, float z, Alignment align, ref MVector3 outVec)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    outVec.y = z;
                    outVec.x = x;
                    outVec.z = -y;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outVec.x = z;
                    outVec.y = y;
                    outVec.z = -x;
                    break;
                case Alignment.ALIGN_X_Y:
                    outVec.x = x;
                    outVec.y = y;
                    outVec.z = z;
                    break;
            };
        }

        public void getPosition(MVector3 TSpos, ref MVector3 outWSpos)
        {
            getPositionAlign(TSpos, mAlign, ref outWSpos);
        }

        public void getPosition(float x, float y, float z, ref MVector3 outWSpos)
        {
            getPositionAlign(x, y, z, mAlign, ref outWSpos);
        }

        public void getTerrainPosition(MVector3 WSpos, ref MVector3 outTSpos)
        {
            getTerrainPositionAlign(WSpos, mAlign, ref outTSpos);
        }

        public void getTerrainPosition(float x, float y, float z, ref MVector3 outTSpos)
        {
            getTerrainPositionAlign(x, y, z, mAlign, ref outTSpos);
        }

        public void getPositionAlign(MVector3 TSpos, Alignment align, ref MVector3 outWSpos)
        {
            getPositionAlign(TSpos.x, TSpos.y, TSpos.z, align, ref outWSpos);
        }

        public void getPositionAlign(float x, float y, float z, Alignment align, ref MVector3 outWSpos)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    outWSpos.y = z;
                    outWSpos.x = x * (mSize - 1) * mScale + mBase;
                    outWSpos.z = y * (mSize - 1) * -mScale - mBase;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outWSpos.x = z;
                    outWSpos.y = y * (mSize - 1) * mScale + mBase;
                    outWSpos.z = x * (mSize - 1) * -mScale - mBase;
                    break;
                case Alignment.ALIGN_X_Y:
                    outWSpos.z = z;
                    outWSpos.x = x * (mSize - 1) * mScale + mBase;
                    outWSpos.y = y * (mSize - 1) * mScale + mBase;
                    break;
            };
        }

        public void getTerrainPositionAlign(MVector3 WSpos, Alignment align, ref MVector3 outTSpos)
        {
            getTerrainPositionAlign(WSpos.x, WSpos.y, WSpos.z, align, ref outTSpos);
        }

        public void getTerrainPositionAlign(float x, float y, float z, Alignment align, ref MVector3 outTSpos)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    outTSpos.x = (x - mBase - mPos.x) / ((mSize - 1) * mScale);
                    outTSpos.y = (z + mBase - mPos.z) / ((mSize - 1) * -mScale);
                    outTSpos.z = y;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outTSpos.x = (z - mBase - mPos.z) / ((mSize - 1) * -mScale);
                    outTSpos.y = (y + mBase - mPos.y) / ((mSize - 1) * mScale);
                    outTSpos.z = x;
                    break;
                case Alignment.ALIGN_X_Y:
                    outTSpos.x = (x - mBase - mPos.x) / ((mSize - 1) * mScale);
                    outTSpos.y = (y - mBase - mPos.y) / ((mSize - 1) * mScale);
                    outTSpos.z = z;
                    break;
            };
        }

        public void getTerrainVector(MVector3 inVec, ref MVector3 outVec)
        {
            getTerrainVectorAlign(inVec.x, inVec.y, inVec.z, mAlign, ref outVec);
        }

        public void getTerrainVector(float x, float y, float z, ref MVector3 outVec)
        {
            getTerrainVectorAlign(x, y, z, mAlign, ref outVec);
        }

        public void getTerrainVectorAlign(MVector3 inVec, Alignment align, ref MVector3 outVec)
        {
            getTerrainVectorAlign(inVec.x, inVec.y, inVec.z, align, ref outVec);
        }

        public void getTerrainVectorAlign(float x, float y, float z, Alignment align, ref MVector3 outVec)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    outVec.z = y;
                    outVec.x = x;
                    outVec.y = -z;
                    break;
                case Alignment.ALIGN_Y_Z:
                    outVec.z = x;
                    outVec.y = y;
                    outVec.x = -z;
                    break;
                case Alignment.ALIGN_X_Y:
                    outVec.x = x;
                    outVec.y = y;
                    outVec.z = z;
                    break;
            };
        }

        public void getUV(long x, long y, ref MVector2 uv)
        {
            float uvScale = 1.0f / (this.getSize() - 1);
            uv.x = x * uvScale;
            uv.y = 1.0f - (y * uvScale);
        }

        public Alignment getAlignment()
        {
            return mAlign;
        }

        public ushort getSize()
        {
            return mSize;
        }

        public ushort getMaxBatchSize()
        {
            return mMaxBatchSize;
        }

        public ushort getMinBatchSize()
        {
            return mMinBatchSize;
        }

        public float getWorldSize()
        {
            return mWorldSize;
        }

        public void distributeVertexData()
        {
            mQuadTree.assignVertexData(0, 0, 0, 0);
        }

        public MTerrain getNeighbour(NeighbourIndex index)
        {
            return mNeighbours[(int)index];
        }

        public void getPointFromSelfOrNeighbour(long x, long y, ref MVector3 outpos)
        {
            checkPoint(ref x, ref y);

            if (x >= 0 && y >= 0 && x < mSize && y < mSize)
                getPoint(x, y, ref outpos);
            else
            {
                long nx = 0, ny = 0;
                NeighbourIndex ni = NeighbourIndex.NEIGHBOUR_EAST;

                getNeighbourPointOverflow(x, y, ref ni, ref nx, ref ny);
                MTerrain neighbour = getNeighbour(ni);
                if (neighbour != null)
                {
                    MVector3 neighbourPos = MVector3.ZERO;
                    neighbour.getPoint(nx, ny, ref neighbourPos);
                    outpos = neighbourPos + neighbour.getPosition() - getPosition();
                }
                else
                {
                    x = UtilMath.min(x, mSize - 1L);
                    y = UtilMath.min(y, mSize - 1L);
                    x = UtilMath.max(x, 0L);
                    y = UtilMath.max(y, 0L);

                    getPoint(x, y, ref outpos);
                }
            }
        }

        public void getUVFromSelfOrNeighbour(long x, long y, ref MVector2 outUV)
        {
            checkPoint(ref x, ref y);

            if (x >= 0 && y >= 0 && x < mSize && y < mSize)
            {
                this.getUV(x, y, ref outUV);
            }
            else
            {
                
            }
        }

        public void getNeighbourPointOverflow(long x, long y, ref NeighbourIndex outindex, ref long outx, ref long outy)
        {
            if (x < 0)
            {
                outx = x + mSize - 1;
                if (y < 0)
                    outindex = NeighbourIndex.NEIGHBOUR_SOUTHWEST;
                else if (y >= mSize)
                    outindex = NeighbourIndex.NEIGHBOUR_NORTHWEST;
                else
                    outindex = NeighbourIndex.NEIGHBOUR_WEST;
            }
            else if (x >= mSize)
            {
                outx = x - mSize + 1;
                if (y < 0)
                    outindex = NeighbourIndex.NEIGHBOUR_SOUTHEAST;
                else if (y >= mSize)
                    outindex = NeighbourIndex.NEIGHBOUR_NORTHEAST;
                else
                    outindex = NeighbourIndex.NEIGHBOUR_EAST;
            }
            else
                outx = x;

            if (y < 0)
            {
                outy = y + mSize - 1;
                if (x >= 0 && x < mSize)
                    outindex = NeighbourIndex.NEIGHBOUR_SOUTH;
            }
            else if (y >= mSize)
            {
                outy = y - mSize + 1;
                if (x >= 0 && x < mSize)
                    outindex = NeighbourIndex.NEIGHBOUR_NORTH;
            }
            else
                outy = y;
        }

        public bool getPointNormal(long x, long y, ref Vector3 pointNormal)
        {
            clampPoint(ref x, ref y, 1, mSize - 1);
            MPlane plane = new MPlane(0);

            MVector3 cumulativeNormal = MVector3.ZERO;

            MVector3 centrePoint = new MVector3(0, 0, 0);
            MVector3[] adjacentPoints = new MVector3[8];

            getPointFromSelfOrNeighbour(x, y, ref centrePoint);
            getPointFromSelfOrNeighbour(x + 1, y, ref adjacentPoints[0]);
            getPointFromSelfOrNeighbour(x + 1, y + 1, ref adjacentPoints[1]);
            getPointFromSelfOrNeighbour(x, y + 1, ref adjacentPoints[2]);
            getPointFromSelfOrNeighbour(x - 1, y + 1, ref adjacentPoints[3]);
            getPointFromSelfOrNeighbour(x - 1, y, ref adjacentPoints[4]);
            getPointFromSelfOrNeighbour(x - 1, y - 1, ref adjacentPoints[5]);
            getPointFromSelfOrNeighbour(x, y - 1, ref adjacentPoints[6]);
            getPointFromSelfOrNeighbour(x + 1, y - 1, ref adjacentPoints[7]);

            for (int i = 0; i < 8; ++i)
            {
                plane.redefine(ref centrePoint, ref adjacentPoints[i], ref adjacentPoints[(i + 1) % 8]);
                cumulativeNormal += plane.normal;
            }

            cumulativeNormal.normalise();

            pointNormal.x = cumulativeNormal.x;
            pointNormal.y = cumulativeNormal.y;
            pointNormal.z = cumulativeNormal.z;

            return true;
        }

        public bool calculateNormals(ref MTRectI rect, ref Vector3[] normalArray)
        {
            MTRectI widenedRect = new MTRectI(
                (int)UtilMath.max(0L, rect.left - 1L),
                (int)UtilMath.max(0L, rect.top - 1L),
                (int)UtilMath.min(mSize, rect.right + 1L),
                (int)UtilMath.min(mSize, rect.bottom + 1L)
            );

            MPlane plane = new MPlane(0);
            for (long y = widenedRect.top; y < widenedRect.bottom; ++y)
            {
                for (long x = widenedRect.left; x < widenedRect.right; ++x)
                {
                    MVector3 cumulativeNormal = MVector3.ZERO;

                    MVector3 centrePoint = new MVector3(0, 0, 0);
                    MVector3[] adjacentPoints = new MVector3[8];

                    getPointFromSelfOrNeighbour(x, y, ref centrePoint);
                    getPointFromSelfOrNeighbour(x + 1, y, ref adjacentPoints[0]);
                    getPointFromSelfOrNeighbour(x + 1, y + 1, ref adjacentPoints[1]);
                    getPointFromSelfOrNeighbour(x, y + 1, ref adjacentPoints[2]);
                    getPointFromSelfOrNeighbour(x - 1, y + 1, ref adjacentPoints[3]);
                    getPointFromSelfOrNeighbour(x - 1, y, ref adjacentPoints[4]);
                    getPointFromSelfOrNeighbour(x - 1, y - 1, ref adjacentPoints[5]);
                    getPointFromSelfOrNeighbour(x, y - 1, ref adjacentPoints[6]);
                    getPointFromSelfOrNeighbour(x + 1, y - 1, ref adjacentPoints[7]);

                    for (int i = 0; i < 8; ++i)
                    {
                        plane.redefine(ref centrePoint, ref adjacentPoints[i], ref adjacentPoints[(i + 1) % 8]);
                        cumulativeNormal += plane.normal;
                    }

                    cumulativeNormal.normalise();

                    long storeX = x - widenedRect.left;
                    long storeY = y - widenedRect.bottom;
                    int normalIndex = (int)((storeY * widenedRect.width()) + storeX);

                    normalArray[normalIndex].x = cumulativeNormal.x;
                    normalArray[normalIndex].y = cumulativeNormal.y;
                    normalArray[normalIndex].z = cumulativeNormal.z;
                }
            }
            return true;
        }

        public bool getPointTangent(long x, long y, ref Vector4 pointTangent)
        {
            clampPoint(ref x, ref y, 1, mSize - 1);

            int faceNum = 2 * 2 * 2;
            int indexNum = 2 * 2 * 6;
            int vertexNum = 3 * 3;

            MVector3[] pointsArray = new MVector3[vertexNum];
            MVector2[] uvArray = new MVector2[vertexNum];
            int[] indexArray = new int[indexNum];
            MVector3[] faceTangents = new MVector3[faceNum];

            getPointFromSelfOrNeighbour(x, y, ref pointsArray[4]);
            getUVFromSelfOrNeighbour(x, y, ref uvArray[4]);

            getPointFromSelfOrNeighbour(x + 1, y, ref pointsArray[5]);
            getUVFromSelfOrNeighbour(x + 1, y, ref uvArray[5]);

            getPointFromSelfOrNeighbour(x + 1, y + 1, ref pointsArray[8]);
            getUVFromSelfOrNeighbour(x + 1, y + 1, ref uvArray[8]);

            getPointFromSelfOrNeighbour(x, y + 1, ref pointsArray[7]);
            getUVFromSelfOrNeighbour(x, y + 1, ref uvArray[7]);

            getPointFromSelfOrNeighbour(x - 1, y + 1, ref pointsArray[6]);
            getUVFromSelfOrNeighbour(x - 1, y + 1, ref uvArray[6]);

            getPointFromSelfOrNeighbour(x - 1, y, ref pointsArray[3]);
            getUVFromSelfOrNeighbour(x, y, ref uvArray[3]);

            getPointFromSelfOrNeighbour(x - 1, y - 1, ref pointsArray[0]);
            getUVFromSelfOrNeighbour(x - 1, y - 1, ref uvArray[0]);

            getPointFromSelfOrNeighbour(x, y - 1, ref pointsArray[1]);
            getUVFromSelfOrNeighbour(x, y - 1, ref uvArray[1]);

            getPointFromSelfOrNeighbour(x + 1, y - 1, ref pointsArray[2]);
            getUVFromSelfOrNeighbour(x + 1, y - 1, ref uvArray[2]);

            int bufferIndex = 0;
            int baseIdx = 0;
            int vertexSize = 3;

            for (int idy = 0; idy < 3; ++idy)
            {
                for (int idx = 0; idx < 3; ++idx)
                {
                    if (idx != 2 && idy != 2)
                    {
                        baseIdx = idx + idy * vertexSize;
                        indexArray[bufferIndex] = baseIdx;
                        indexArray[bufferIndex + 1] = baseIdx + vertexSize;
                        indexArray[bufferIndex + 2] = baseIdx + vertexSize + 1;
                        indexArray[bufferIndex + 3] = baseIdx;
                        indexArray[bufferIndex + 4] = baseIdx + vertexSize + 1;
                        indexArray[bufferIndex + 5] = baseIdx + 1;

                        bufferIndex += 6;
                    }
                }
            }

            updateFaceTangents(pointsArray, uvArray, indexArray, faceTangents);
            MVector3 cumulativeTangent = MVector3.ZERO;
            for (int i = 0; i < faceNum; ++i)
            {
                cumulativeTangent += faceTangents[i];
            }

            cumulativeTangent.normalise();

            pointTangent.x = cumulativeTangent.x;
            pointTangent.y = cumulativeTangent.y;
            pointTangent.z = cumulativeTangent.z;
            pointTangent.w = 0;
            return true;
        }

        public bool calculateTangents(ref MTRectI rect, ref Vector4[] tangentArray)
        {
            MTRectI widenedRect = new MTRectI(
               (int)UtilMath.max(0L, rect.left - 1L),
               (int)UtilMath.max(0L, rect.top - 1L),
               (int)UtilMath.min(mSize, rect.right + 1L),
               (int)UtilMath.min(mSize, rect.bottom + 1L)
            );

            int faceNum = 2 * 2 * 2;
            int indexNum = 2 * 2 * 6;
            int vertexNum = 3 * 3;
            int[] indexArray = new int[indexNum];
            MVector3[] faceTangents = new MVector3[faceNum];
            MVector2[] uvs = new MVector2[vertexNum];

            for (long y = widenedRect.top; y < widenedRect.bottom; ++y)
            {
                for (long x = widenedRect.left; x < widenedRect.right; ++x)
                {
                    MVector3[] pointsArray = new MVector3[9];

                    getPointFromSelfOrNeighbour(x, y, ref pointsArray[4]);
                    getPointFromSelfOrNeighbour(x + 1, y, ref pointsArray[0]);
                    getPointFromSelfOrNeighbour(x + 1, y + 1, ref pointsArray[1]);
                    getPointFromSelfOrNeighbour(x, y + 1, ref pointsArray[2]);
                    getPointFromSelfOrNeighbour(x - 1, y + 1, ref pointsArray[3]);
                    getPointFromSelfOrNeighbour(x - 1, y, ref pointsArray[5]);
                    getPointFromSelfOrNeighbour(x - 1, y - 1, ref pointsArray[6]);
                    getPointFromSelfOrNeighbour(x, y - 1, ref pointsArray[7]);
                    getPointFromSelfOrNeighbour(x + 1, y - 1, ref pointsArray[8]);

                    calcIndex(pointsArray, 3, ref indexArray);
                    updateFaceTangents(pointsArray, uvs, indexArray, faceTangents);
                    MVector3 cumulativeTangent = MVector3.ZERO;
                    for (int i = 0; i < faceNum; ++i)
                    {
                        cumulativeTangent += faceTangents[i];
                    }

                    cumulativeTangent.normalise();

                    long storeX = x - widenedRect.left;
                    long storeY = y - widenedRect.bottom;
                    int normalIndex = (int)((storeY * widenedRect.width()) + storeX);

                    tangentArray[normalIndex].x = cumulativeTangent.x;
                    tangentArray[normalIndex].y = cumulativeTangent.y;
                    tangentArray[normalIndex].z = cumulativeTangent.z;
                    tangentArray[normalIndex].w = 0;
                }
            }
            return true;
        }

        public void calcIndex(MVector3[] vertices, int vertexNum, ref int[] indexArray)
        {
            int numIndex = 0;
            int baseIdx = 0;
            int tw = vertexNum;

            for (int zi = 0; zi <= vertexNum - 1; ++zi)
            {
                for (int xi = 0; xi <= vertexNum - 1; ++xi)
                {
                    if (xi != vertexNum - 1 && zi != vertexNum - 1)   // 循环中计数已经多加了 1 ，因此，这里如果超过范围直接返回，只有在范围内的值，才更新
                    {
                        baseIdx = xi + zi * tw;
                        indexArray[numIndex] = baseIdx;
                        indexArray[numIndex + 1] = baseIdx + tw;
                        indexArray[numIndex + 2] = baseIdx + tw + 1;
                        indexArray[numIndex + 3] = baseIdx;
                        indexArray[numIndex + 4] = baseIdx + tw + 1;
                        indexArray[numIndex + 5] = baseIdx + 1;

                        numIndex += 6;
                    }
                }
            }
        }

        protected void updateFaceTangents(MVector3[] vertices, MVector2[] uvs, int[] indexs, MVector3[] faceTangents)
        {
            uint i = 0;
            int index1 = 0, index2 = 0, index3 = 0;
            uint len = (uint)indexs.Length;
            uint ui = 0, vi = 0;
            float v0 = 0;
            float dv1 = 0, dv2 = 0;
            float denom = 0;
            float x0 = 0, y0 = 0, z0 = 0;
            float dx1 = 0, dy1 = 0, dz1 = 0;
            float dx2 = 0, dy2 = 0, dz2 = 0;
            float cx = 0, cy = 0, cz = 0;
            int posStride = 1;
            int posOffset = 0;
            int texStride = 1;
            int texOffset = 0;

            i = 0;
            while (i < len)     // 一个面是 3 个顶点，遍历一次就是一个面
            {
                index1 = indexs[(int)i];
                index2 = indexs[(int)i + 1];
                index3 = indexs[(int)i + 2];

                ui = (uint)(texOffset + index1 * texStride);
                v0 = uvs[(int)ui].y;
                ui = (uint)(texOffset + index2 * texStride);
                dv1 = uvs[(int)ui].y - v0;
                ui = (uint)(texOffset + index3 * texStride);
                dv2 = uvs[(int)ui].y - v0;

                vi = (uint)(posOffset + index1 * posStride);
                x0 = vertices[(int)vi].x;
                y0 = vertices[(int)vi].y;
                z0 = vertices[(int)vi].z;
                vi = (uint)(posOffset + index2 * posStride);
                dx1 = vertices[(int)(vi)].x - x0;
                dy1 = vertices[(int)(vi)].y - y0;
                dz1 = vertices[(int)(vi)].z - z0;
                vi = (uint)(posOffset + index3 * posStride);
                dx2 = vertices[(int)(vi)].x - x0;
                dy2 = vertices[(int)(vi)].y - y0;
                dz2 = vertices[(int)(vi)].z - z0;

                cx = dv2 * dx1 - dv1 * dx2;
                cy = dv2 * dy1 - dv1 * dy2;
                cz = dv2 * dz1 - dv1 * dz2;
                denom = (float)(1 / UtilMath.Sqrt(cx * cx + cy * cy + cz * cz));
                faceTangents[(int)i / 3].x = denom * cx;
                faceTangents[(int)i / 3].y = denom * cy;
                faceTangents[(int)i / 3].z = denom * cz;

                i += 3;     // 移动 3 个顶点，就是一个面
            }
        }

        public void determineLodLevels()
        {
            mNumLodLevelsPerLeafNode = (ushort)(UtilMath.Log2(mMaxBatchSize - 1.0f) - UtilMath.Log2(mMinBatchSize - 1.0f) + 1.0f);
            mNumLodLevels = (ushort)(UtilMath.Log2(mSize - 1.0f) - UtilMath.Log2(mMinBatchSize - 1.0f) + 1.0f);
            mTreeDepth = (ushort)(mNumLodLevels - mNumLodLevelsPerLeafNode + 1);
        }

        public void show()
        {
            mQuadTree.show();
        }

        public Material getMatTmpl()
        {
            return mTerrainMat.getDiffuseMaterial();
        }

        public void checkPoint(ref long x, ref long y)
        {
            if(x < 0)
            {
                x = 0;
            }
            if(x >= mSize)
            {
                x = mSize - 1;
            }

            if (y < 0)
            {
                y = 0;
            }
            if (y >= mSize)
            {
                y = mSize - 1;
            }
        }

        public void clampPoint(ref long x, ref long y, long min, long max)
        {
            if (min > x)
            {
                x = min;
            }
            if (max < x)
            {
                x = max;
            }
            if (min > y)
            {
                y = min;
            }
            if (max == y)
            {
                y = max;
            }
        }
    }
}