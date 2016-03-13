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
            mBase = -mWorldSize * 0.5f;
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
            //load(0, true);

            //x = UtilMath.min(x, (long)mSize - 1L);
            //x = UtilMath.max(x, 0L);
            //y = UtilMath.min(y, (long)mSize - 1L);
            //y = UtilMath.max(y, 0L);

            //getHeightData(x, y) = h;
            //Rect rect;
            //rect.left = x;
            //rect.right = x + 1;
            //rect.top = y;
            //rect.bottom = y + 1;
            //dirtyRect(rect);
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
                    outpos.y = height;
                    outpos.x = x * mScale + mBase;
                    outpos.z = y * -mScale - mBase;
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

        public bool calculateNormals(ref MTRectI rect, ref MVector3[] normalArray)
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
    }
}