using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public struct MKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public MKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public enum Space
    {
        WORLD_SPACE = 0,
        LOCAL_SPACE = 1,
        TERRAIN_SPACE = 2,
        POINT_SPACE = 3
    };

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

    public class MTerrain : MMovableObject
    {
        protected MSceneManager mSceneMgr;
        protected MSceneNode mRootNode;
        protected MSceneNode mTerrainEntityNode;    // 仅仅关联 Terrain
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
        protected string m_layerStr;
        protected MImportData mImportData;
        protected float mUVMultiplier;

        protected MTRectI mDirtyGeometryRect;
        protected MTRectI mDirtyDerivedDataRect;
        protected MTRectI mDirtyGeometryRectForNeighbours;

        protected const byte DERIVED_DATA_NORMALS = 2;
        protected const byte DERIVED_DATA_ALL = 7;

        protected MAxisAlignedBox mAABB;
        protected MAxisAlignedBox mWorldAabb;
        protected bool mIsInit;
        protected long mX;
        protected long mY;

        public MTerrain(MSceneManager sm)
        {
            mSceneMgr = sm;
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
            //mRootNode = new MSceneNode("Terrain");
            m_layerStr = "Default";

            this.mAABB = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            this.mWorldAabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
        }

        public void dispose()
        {

        }

        public bool isLoaded()
        {
            return mIsLoaded;
        }

        public bool isModified()
        {
            return mModified;
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
            mImportData = importData;
            mImportData.parseXml();
            mPrepareInProgress = true;
            this.mX = importData.x;
            this.mY = importData.y;

            mSize = importData.terrainSize;
            mWorldSize = importData.worldSize;
            mMaxBatchSize = importData.maxBatchSize;
            mMinBatchSize = importData.minBatchSize;

            mRootNode = mSceneMgr.getRootSceneNode().createChildSceneNode("Terrain_" + mX + "_" + mY, MVector3.ZERO, MQuaternion.IDENTITY);
            mTerrainEntityNode = mSceneMgr.getRootSceneNode().createChildSceneNode("TerrainEntity_" + mX + "_" + mY, MVector3.ZERO, MQuaternion.IDENTITY);

            mAABB.setMinimum(new MVector3(0, -100, 0));
            mAABB.setMaximum(new MVector3(getWorldSize(), 100, getWorldSize()));

            if (!this.isAttached())
            {
                mTerrainEntityNode.attachObject(this);
            }

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
            if (!mImportData.isUseSplatMap)
            {
                mTerrainMat.loadDiffuseMat();
            }
            else
            {
                mTerrainMat.setUVMultiplier(mUVMultiplier);
                mTerrainMat.loadSplatDiffuseMat();
            }

            mQuadTree = new MTerrainQuadTreeNode(this, null, 0, 0, mSize, (ushort)(mNumLodLevels - 1), 0, 0);
            mQuadTree.prepare();
            //distributeVertexData();

            mModified = true;
            mHeightDataModified = true;

            mPrepareInProgress = false;

            return true;
        }

        public float getMaxBatchWorldSize()
        {
            return (mMaxBatchSize - 1) * mScale;
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

        public MVector3 convertPosition(Space inSpace, MVector3 inPos, Space outSpace)
        {
            MVector3 ret = new MVector3(0, 0, 0);

            convertPosition(inSpace, inPos, outSpace, ref ret);
            return ret;
        }

        public MVector3 convertDirection(Space inSpace, MVector3 inDir, Space outSpace)
        {
            MVector3 ret = new MVector3(0, 0, 0);

            convertDirection(inSpace, inDir, outSpace, ref ret);
            return ret;
        }

        public void convertPosition(Space inSpace, MVector3 inPos, Space outSpace, ref MVector3 outPos)
        {
            convertSpace(inSpace, inPos, outSpace, ref outPos, true);
        }

        public void convertDirection(Space inSpace, MVector3 inDir, Space outSpace, ref MVector3 outDir)
        {
            convertSpace(inSpace, inDir, outSpace, ref outDir, false);
        }

        public void convertSpace(Space inSpace, MVector3 inVec, Space outSpace, ref MVector3 outVec, bool translation)
        {
            Space currSpace = inSpace;
            outVec = inVec;
            while (currSpace != outSpace)
            {
                switch (currSpace)
                {
                    case Space.WORLD_SPACE:
                        if (translation)
                            outVec -= mPos;
                        currSpace = Space.LOCAL_SPACE;
                        break;
                    case Space.LOCAL_SPACE:
                        switch (outSpace)
                        {
                            case Space.WORLD_SPACE:
                                if (translation)
                                    outVec += mPos;
                                currSpace = Space.WORLD_SPACE;
                                break;
                            case Space.POINT_SPACE:
                            case Space.TERRAIN_SPACE:
                                outVec = convertWorldToTerrainAxes(outVec);
                                if (translation)
                                {
                                    outVec.x -= mBase; outVec.y -= mBase;
                                    outVec.x /= (mSize - 1) * mScale; outVec.y /= (mSize - 1) * mScale;
                                }
                                currSpace = Space.TERRAIN_SPACE;
                                break;
                            case Space.LOCAL_SPACE:
                            default:
                                break;
                        };
                        break;
                    case Space.TERRAIN_SPACE:
                        switch (outSpace)
                        {
                            case Space.WORLD_SPACE:
                            case Space.LOCAL_SPACE:
                                if (translation)
                                {
                                    outVec.x *= (mSize - 1) * mScale; outVec.y *= (mSize - 1) * mScale;
                                    outVec.x += mBase; outVec.y += mBase;
                                }
                                outVec = convertTerrainToWorldAxes(outVec);
                                currSpace = Space.LOCAL_SPACE;
                                break;
                            case Space.POINT_SPACE:
                                if (translation)
                                {
                                    outVec.x *= (mSize - 1); outVec.y *= (mSize - 1);
                                    outVec.x = (float)((int)(outVec.x + 0.5));
                                    outVec.y = (float)((int)(outVec.y + 0.5));
                                }
                                currSpace = Space.POINT_SPACE;
                                break;
                            case Space.TERRAIN_SPACE:
                            default:
                                break;
                        };
                        break;
                    case Space.POINT_SPACE:
                        if (translation)
                            outVec.x /= (mSize - 1); outVec.y /= (mSize - 1);
                        currSpace = Space.TERRAIN_SPACE;
                        break;

                };
            }
        }

        static public void convertWorldToTerrainAxes(Alignment align, MVector3 worldVec, ref MVector3 terrainVec)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    terrainVec.z = worldVec.y;
                    terrainVec.x = worldVec.x;
                    terrainVec.y = -worldVec.z;
                    break;
                case Alignment.ALIGN_Y_Z:
                    terrainVec.z = worldVec.x;
                    terrainVec.x = -worldVec.z;
                    terrainVec.y = worldVec.y;
                    break;
                case Alignment.ALIGN_X_Y:
                    terrainVec = worldVec;
                    break;
            };
        }

        static public void convertTerrainToWorldAxes(Alignment align, MVector3 terrainVec, ref MVector3 worldVec)
        {
            switch (align)
            {
                case Alignment.ALIGN_X_Z:
                    worldVec.x = terrainVec.x;
                    worldVec.y = terrainVec.z;
                    worldVec.z = -terrainVec.y;
                    break;
                case Alignment.ALIGN_Y_Z:
                    worldVec.x = terrainVec.z;
                    worldVec.y = terrainVec.y;
                    worldVec.z = -terrainVec.x;
                    break;
                case Alignment.ALIGN_X_Y:
                    worldVec = terrainVec;
                    break;
            };
        }

        public MVector3 convertWorldToTerrainAxes(MVector3 inVec)
        {
            MVector3 ret = new MVector3(0, 0, 0);

            convertWorldToTerrainAxes(mAlign, inVec, ref ret);

            return ret;
        }

        public MVector3 convertTerrainToWorldAxes(MVector3 inVec)
        {
            MVector3 ret = new MVector3(0, 0, 0);

            convertTerrainToWorldAxes(mAlign, inVec, ref ret);

            return ret;
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
            //if (mImportData.isUseSplatMap)
            //{
            //    uv.x = x * uvScale * mUVMultiplier;
            //    uv.y = (1.0f - (y * uvScale) * mUVMultiplier);
            //}
            //else
            //{
                uv.x = x * uvScale;
                uv.y = 1.0f - (y * uvScale);
            //}
        }

        public float getU(long x)
        {
            float uvScale = 1.0f / (this.getSize() - 1);
            //if (mImportData.isUseSplatMap)
            //{
            //    return x * uvScale * mUVMultiplier;
            //}
            //else
            //{
                return x * uvScale;
            //}
        }

        public float getV(long y)
        {
            float uvScale = 1.0f / (this.getSize() - 1);
            //if (mImportData.isUseSplatMap)
            //{
            //    return (1.0f - (y * uvScale)) * mUVMultiplier;
            //}
            //else
            //{
            //return 1.0f - (y * uvScale);
            return y * uvScale;
            //}
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

        public void setPosition(MVector3 pos)
        {
            if (pos != mPos)
            {
                mPos = pos;
                mRootNode.setPosition(pos);
                mTerrainEntityNode.setPosition(pos);

                updateBaseScale();
                mModified = true;
            }
        }

        public MSceneNode _getRootSceneNode()
        {
            return mRootNode;
        }

        public void updateBaseScale()
        {
            //mBase = -mWorldSize * 0.5f;
            //mScale = mWorldSize / (float)(mSize - 1);

            mBase = mWorldSize * 0.5f;
            mScale = mWorldSize / (float)(mSize - 1);
            mUVMultiplier = mWorldSize / mImportData.detailWorldSize;
        }

        public void dirty()
        {
            MTRectI rect;
            rect.top = 0; rect.bottom = mSize;
            rect.left = 0; rect.right = mSize;
            dirtyRect(rect);
        }

        public void dirtyRect(MTRectI rect)
        {
            mDirtyGeometryRect.merge(ref rect);
            mDirtyGeometryRectForNeighbours.merge(ref rect);
            mDirtyDerivedDataRect.merge(ref rect);

            mModified = true;
            mHeightDataModified = true;
        }

        public void update(bool synchronous)
        {
            updateGeometry();
            updateDerivedData(synchronous, 0);
        }

        public void updateGeometry()
        {
            if (!mDirtyGeometryRect.isNull())
            {
                mQuadTree.updateVertexData(true, false, mDirtyGeometryRect, false);
                mDirtyGeometryRect.setNull();
            }

            notifyNeighbours();
        }

        public void updateDerivedData(bool synchronous, byte typeMask)
        {

        }

        public void updateGeometryWithoutNotifyNeighbours()
        {
            if (!mDirtyGeometryRect.isNull())
            {
                mQuadTree.updateVertexData(true, false, mDirtyGeometryRect, false);
                mDirtyGeometryRect.setNull();
            }
        }

        public MKeyValuePair<bool, MVector3> rayIntersects(MRay ray,
        bool cascadeToNeighbours, float distanceLimit)
        {
            MVector3 rayOrigin = ray.getOrigin() - getPosition();
            MVector3 rayDirection = ray.getDirection();
            MVector3 tmp;
            switch (getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                    UtilMath.swap(ref rayOrigin.y, ref rayOrigin.z);
                    UtilMath.swap(ref rayDirection.y, ref rayDirection.z);
                    break;
                case Alignment.ALIGN_Y_Z:
                    tmp.x = rayOrigin.z;
                    tmp.z = rayOrigin.y;
                    tmp.y = -rayOrigin.x;
                    rayOrigin = tmp;
                    tmp.x = rayDirection.z;
                    tmp.z = rayDirection.y;
                    tmp.y = -rayDirection.x;
                    rayDirection = tmp;
                    break;
                case Alignment.ALIGN_X_Z:
                    rayOrigin.z = -rayOrigin.z;
                    rayDirection.z = -rayDirection.z;
                    break;
            }
            rayOrigin.x += mWorldSize / 2;
            rayOrigin.z += mWorldSize / 2;

            rayOrigin.x /= mScale;
            rayOrigin.z /= mScale;
            rayDirection.x /= mScale;
            rayDirection.z /= mScale;
            rayDirection.normalise();
            MRay localRay = new MRay(rayOrigin, rayDirection);

            float maxHeight = getMaxHeight();
            float minHeight = getMinHeight();

            MVector3 min = new MVector3(0, minHeight, 0);
            MVector3 max = new MVector3(mSize, maxHeight, mSize);
            MAxisAlignedBox aabb = new MAxisAlignedBox(ref min, ref max);
            KeyValuePair<bool, float> aabbTest = localRay.intersects(ref aabb);
            if (!aabbTest.Key)
            {
                if (cascadeToNeighbours)
                {
                    MTerrain neighbour = raySelectNeighbour(ref ray, distanceLimit);
                    if (neighbour != null)
                        return neighbour.rayIntersects(ray, cascadeToNeighbours, distanceLimit);
                }
                return new MKeyValuePair<bool, MVector3>(false, new MVector3(0, 0, 0));
            }

            MVector3 cur = localRay.getPoint(aabbTest.Value);

            int quadX = (int)UtilMath.min(UtilMath.max((int)(cur.x), 0), (int)mSize - 2);
            int quadZ = (int)UtilMath.min(UtilMath.max((int)(cur.z), 0), (int)mSize - 2);
            int flipX = (rayDirection.x < 0 ? 0 : 1);
            int flipZ = (rayDirection.z < 0 ? 0 : 1);
            int xDir = (rayDirection.x < 0 ? -1 : 1);
            int zDir = (rayDirection.z < 0 ? -1 : 1);

            MKeyValuePair<bool, MVector3> result = new MKeyValuePair<bool, MVector3>(true, MVector3.ZERO);
            float dummyHighValue = (float)mSize * 10000.0f;

            while (cur.y >= (minHeight - 1e-3) && cur.y <= (maxHeight + 1e-3))
            {
                if (quadX < 0 || quadX >= (int)mSize - 1 || quadZ < 0 || quadZ >= (int)mSize - 1)
                    break;

                result = checkQuadIntersection(quadX, quadZ, ref localRay);
                if (result.Key)
                    break;

                float xDist = UtilMath.RealEqual(rayDirection.x, 0.0f) ? dummyHighValue :
                    (quadX - cur.x + flipX) / rayDirection.x;
                float zDist = UtilMath.RealEqual(rayDirection.z, 0.0f) ? dummyHighValue :
                (quadZ - cur.z + flipZ) / rayDirection.z;
                if (xDist < zDist)
                {
                    quadX += xDir;
                    cur += rayDirection * xDist;
                }
                else
                {
                    quadZ += zDir;
                    cur += rayDirection * zDist;
                }
            }

            if (result.Key)
            {
                result.Value.x *= mScale;
                result.Value.z *= mScale;
                result.Value.x -= mWorldSize / 2;
                result.Value.z -= mWorldSize / 2;
                switch (getAlignment())
                {
                    case Alignment.ALIGN_X_Y:
                        UtilMath.swap(ref result.Value.y, ref result.Value.z);
                        break;
                    case Alignment.ALIGN_Y_Z:
                        tmp.x = -rayOrigin.y;
                        tmp.y = rayOrigin.z;
                        tmp.z = rayOrigin.x;
                        rayOrigin = tmp;
                        break;
                    case Alignment.ALIGN_X_Z:
                        result.Value.z = -result.Value.z;
                        break;
                }
                result.Value += getPosition();
            }
            else if (cascadeToNeighbours)
            {
                MTerrain neighbour = raySelectNeighbour(ref ray, distanceLimit);
                if (neighbour != null)
                    result = neighbour.rayIntersects(ray, cascadeToNeighbours, distanceLimit);
            }
            return result;
        }

        MKeyValuePair<bool, MVector3> checkQuadIntersection(int x, int z, ref MRay ray)
        {
            MVector3 v1 = new MVector3((float)x, getHeightData(x, z), (float)z);
            MVector3 v2 = new MVector3((float)x + 1, getHeightData(x + 1, z), (float)z);
            MVector3 v3 = new MVector3((float)x, getHeightData(x, z + 1), (float)z + 1);
            MVector3 v4 = new MVector3((float)x + 1, getHeightData(x + 1, z + 1), (float)z + 1);

            MPlane p1 = new MPlane(0), p2 = new MPlane(0);
            bool oddRow = false;
            if (z % 2 != 0)
            {
                p1.redefine(ref v2, ref v4, ref v3);
                p2.redefine(ref v1, ref v2, ref v3);
                oddRow = true;
            }
            else
            {
                p1.redefine(ref v1, ref v2, ref v4);
                p2.redefine(ref v1, ref v4, ref v3);
            }

            MKeyValuePair<bool, float> planeInt = ray.intersects(ref p1);
            if (planeInt.Key)
            {
                MVector3 where = ray.getPoint(planeInt.Value);
                MVector3 rel = where - v1;
                if (rel.x >= -0.01 && rel.x <= 1.01 && rel.z >= -0.01 && rel.z <= 1.01
                    && ((rel.x >= rel.z && !oddRow) || (rel.x >= (1 - rel.z) && oddRow)))
                    return new MKeyValuePair<bool, MVector3>(true, where);
            }
            planeInt = ray.intersects(ref p2);
            if (planeInt.Key)
            {
                MVector3 where = ray.getPoint(planeInt.Value);
                MVector3 rel = where - v1;
                if (rel.x >= -0.01 && rel.x <= 1.01 && rel.z >= -0.01 && rel.z <= 1.01
                    && ((rel.x <= rel.z && !oddRow) || (rel.x <= (1 - rel.z) && oddRow)))
                    return new MKeyValuePair<bool, MVector3>(true, where);
            }

            return new MKeyValuePair<bool, MVector3>(false, new MVector3(0, 0, 0));
        }

        public void distributeVertexData()
        {
            mQuadTree.assignVertexData(0, 0, 0, 0);
        }

        public void neighbourModified(NeighbourIndex index, MTRectI edgerect, MTRectI shadowrect)
        {
            MTerrain neighbour = getNeighbour(index);
            if (neighbour == null)
                return;

            bool updateGeom = false;
            byte updateDerived = 0;

            if (!edgerect.isNull())
            {
                MTRectI heightMatchRect = new MTRectI(0, 0, 0, 0);

                getEdgeRect(index, 1, ref heightMatchRect);
                heightMatchRect = heightMatchRect.intersect(edgerect);

                for (long y = heightMatchRect.top; y < heightMatchRect.bottom; ++y)
                {
                    for (long x = heightMatchRect.left; x < heightMatchRect.right; ++x)
                    {
                        long nx = 0, ny = 0;

                        getNeighbourPoint(index, x, y, ref nx, ref ny);
                        float neighbourHeight = neighbour.getHeightAtPoint(nx, ny);
                        if (!UtilMath.RealEqual(neighbourHeight, getHeightAtPoint(x, y), 1e-3f))
                        {
                            setHeightAtPoint(x, y, neighbourHeight);
                            if (!updateGeom)
                            {
                                updateGeom = true;
                                updateDerived |= DERIVED_DATA_ALL;
                            }
                        }
                    }
                }

                if (!updateGeom)
                {
                    mDirtyDerivedDataRect.merge(ref edgerect);
                    updateDerived |= DERIVED_DATA_NORMALS;
                }
            }

            if (updateGeom)
                updateGeometry();
            if (updateDerived != 0)
                updateDerivedData(false, updateDerived);
        }

        public void getEdgeRect(NeighbourIndex index, long range, ref MTRectI outRect)
        {
            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_EAST:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                    outRect.left = (int)(mSize - range);
                    outRect.right = mSize;
                    break;
                case NeighbourIndex.NEIGHBOUR_WEST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                    outRect.left = 0;
                    outRect.right = (int)(range);
                    break;
                case NeighbourIndex.NEIGHBOUR_NORTH:
                case NeighbourIndex.NEIGHBOUR_SOUTH:
                    outRect.left = 0;
                    outRect.right = mSize;
                    break;
                case NeighbourIndex.NEIGHBOUR_COUNT:
                default:
                    break;
            };

            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_NORTH:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                    outRect.top = (int)(mSize - range);
                    outRect.bottom = mSize;
                    break;
                case NeighbourIndex.NEIGHBOUR_SOUTH:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                    outRect.top = 0;
                    outRect.bottom = (int)(range);
                    break;
                case NeighbourIndex.NEIGHBOUR_EAST:
                case NeighbourIndex.NEIGHBOUR_WEST:
                    outRect.top = 0;
                    outRect.bottom = mSize;
                    break;
                case NeighbourIndex.NEIGHBOUR_COUNT:
                default:
                    break;
            };
        }

        public void getNeighbourEdgeRect(NeighbourIndex index, MTRectI inRect, ref MTRectI outRect)
        {
            UtilApi.assert(mSize == getNeighbour(index).getSize());

            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_EAST:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                case NeighbourIndex.NEIGHBOUR_WEST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                    outRect.left = mSize - inRect.right;
                    outRect.right = mSize - inRect.left;
                    break;
                default:
                    outRect.left = inRect.left;
                    outRect.right = inRect.right;
                    break;
            };

            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_NORTH:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTH:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                    outRect.top = mSize - inRect.bottom;
                    outRect.bottom = mSize - inRect.top;
                    break;
                default:
                    outRect.top = inRect.top;
                    outRect.bottom = inRect.bottom;
                    break;
            };

        }

        public void getNeighbourPoint(NeighbourIndex index, long x, long y, ref long outx, ref long outy)
        {
            UtilApi.assert(mSize == getNeighbour(index).getSize());

            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_EAST:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                case NeighbourIndex.NEIGHBOUR_WEST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                    outx = mSize - x - 1;
                    break;
                default:
                    outx = x;
                    break;
            };

            switch (index)
            {
                case NeighbourIndex.NEIGHBOUR_NORTH:
                case NeighbourIndex.NEIGHBOUR_NORTHEAST:
                case NeighbourIndex.NEIGHBOUR_NORTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTH:
                case NeighbourIndex.NEIGHBOUR_SOUTHWEST:
                case NeighbourIndex.NEIGHBOUR_SOUTHEAST:
                    outy = mSize - y - 1;
                    break;
                default:
                    outy = y;
                    break;
            };
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

        public MTerrain raySelectNeighbour(ref MRay ray, float distanceLimit)
        {
            MRay modifiedRay = new MRay(ray.getOrigin(), ray.getDirection());
            modifiedRay.setOrigin(modifiedRay.getPoint(-mWorldSize / mSize * 0.5f));

            MVector3 tPos = new MVector3(0, 0, 0), tDir = new MVector3(0, 0, 0);

            convertPosition(Space.WORLD_SPACE, modifiedRay.getOrigin(), Space.TERRAIN_SPACE, ref tPos);

            convertDirection(Space.WORLD_SPACE, modifiedRay.getDirection(), Space.TERRAIN_SPACE, ref tDir);
            if (UtilMath.RealEqual(tDir.x, 0.0f, 1e-4f) && UtilMath.RealEqual(tDir.y, 0.0f, 1e-4f))
                return null;

            MRay terrainRay = new MRay(tPos, tDir);
            float dist = float.MaxValue;
            MKeyValuePair<bool, float> intersectResult = new MKeyValuePair<bool, float>(false, 0);
            if (tDir.x < 0.0f)
            {
                intersectResult = UtilMath.intersects(terrainRay, new MPlane(MVector3.UNIT_X, MVector3.ZERO));
                if (intersectResult.Key && intersectResult.Value < dist)
                    dist = intersectResult.Value;
            }
            else if (tDir.x > 0.0f)
            {
                intersectResult = UtilMath.intersects(terrainRay, new MPlane(MVector3.NEGATIVE_UNIT_X, new MVector3(1, 0, 0)));
                if (intersectResult.Key && intersectResult.Value < dist)
                    dist = intersectResult.Value;
            }
            if (tDir.y < 0.0f)
            {
                intersectResult = UtilMath.intersects(terrainRay, new MPlane(MVector3.UNIT_Y, MVector3.ZERO));
                if (intersectResult.Key && intersectResult.Value < dist)

                    dist = intersectResult.Value;
            }
            else if (tDir.y > 0.0f)
            {
                intersectResult = UtilMath.intersects(terrainRay, new MPlane(MVector3.NEGATIVE_UNIT_Y, new MVector3(0, 1, 0)));
                if (intersectResult.Key && intersectResult.Value < dist)
                    dist = intersectResult.Value;
            }

            if (dist * mWorldSize > distanceLimit)
                return null;

            MVector3 terrainIntersectPos = terrainRay.getPoint(dist);
            float x = terrainIntersectPos.x;
            float y = terrainIntersectPos.y;
            float dx = tDir.x;
            float dy = tDir.y;

            if (UtilMath.RealEqual(x, 1.0f, 1e-4f) && dx > 0)
                return getNeighbour(NeighbourIndex.NEIGHBOUR_EAST);
            else if (UtilMath.RealEqual(x, 0.0f, 1e-4f) && dx < 0)
                return getNeighbour(NeighbourIndex.NEIGHBOUR_WEST);
            else if (UtilMath.RealEqual(y, 1.0f, 1e-4f) && dy > 0)
                return getNeighbour(NeighbourIndex.NEIGHBOUR_NORTH);
            else if (UtilMath.RealEqual(y, 0.0f, 1e-4f) && dy < 0)
                return getNeighbour(NeighbourIndex.NEIGHBOUR_SOUTH);

            return null;
        }

        public MTerrain getNeighbour(NeighbourIndex index)
        {
            return mNeighbours[(int)index];
        }

        public void setNeighbour(NeighbourIndex index, MTerrain neighbour,
            bool recalculate, bool notifyOther)
        {
            if (mNeighbours[(int)index] != neighbour)
            {
                UtilApi.assert(neighbour != this, "Can't set self as own neighbour!");

                if (mNeighbours[(int)index] != null && notifyOther)
                    mNeighbours[(int)index].setNeighbour(getOppositeNeighbour(index), null, false, false);

                mNeighbours[(int)index] = neighbour;
                if (neighbour != null && notifyOther)
                    mNeighbours[(int)index].setNeighbour(getOppositeNeighbour(index), this, recalculate, false);

                if (recalculate)
                {
                    MTRectI edgerect = new MTRectI(0, 0, 0, 0);
                    getEdgeRect(index, 2, ref edgerect);
                    neighbourModified(index, edgerect, edgerect);
                }
            }
        }

        public NeighbourIndex getOppositeNeighbour(NeighbourIndex index)
        {
            int intindex = (int)(index);
            intindex += (int)NeighbourIndex.NEIGHBOUR_COUNT / 2;
            intindex = intindex % (int)NeighbourIndex.NEIGHBOUR_COUNT;
            return (NeighbourIndex)(intindex);
        }

        static public NeighbourIndex getNeighbourIndex(long x, long y)
        {
            if (x < 0)
            {
                if (y < 0)
                    return NeighbourIndex.NEIGHBOUR_SOUTHWEST;
                else if (y > 0)
                    return NeighbourIndex.NEIGHBOUR_NORTHWEST;
                else
                    return NeighbourIndex.NEIGHBOUR_WEST;
            }
            else if (x > 0)
            {
                if (y < 0)
                    return NeighbourIndex.NEIGHBOUR_SOUTHEAST;
                else if (y > 0)
                    return NeighbourIndex.NEIGHBOUR_NORTHEAST;
                else
                    return NeighbourIndex.NEIGHBOUR_EAST;
            }

            if (y < 0)
            {
                if (x == 0)
                    return NeighbourIndex.NEIGHBOUR_SOUTH;
            }
            else if (y > 0)
            {
                if (x == 0)
                    return NeighbourIndex.NEIGHBOUR_NORTH;
            }

            return NeighbourIndex.NEIGHBOUR_NORTH;
        }

        public void notifyNeighbours()
        {

        }

        public bool getPointNormal(long x, long y, ref Vector3 pointNormal)
        {
            clampPoint(ref x, ref y, 1, mSize - 2);
            MPlane plane = new MPlane(0);

            MVector3 cumulativeNormal = MVector3.ZERO;

            MVector3 centrePoint = new MVector3(0, 0, 0);
            MVector3[] adjacentPoints = new MVector3[8];

            getPointFromSelfOrNeighbour(x, y, ref centrePoint);
            getPointFromSelfOrNeighbour(x + 1, y, ref adjacentPoints[0]);
            getPointFromSelfOrNeighbour(x + 1, y + 1, ref adjacentPoints[7]);
            getPointFromSelfOrNeighbour(x, y + 1, ref adjacentPoints[6]);
            getPointFromSelfOrNeighbour(x - 1, y + 1, ref adjacentPoints[5]);
            getPointFromSelfOrNeighbour(x - 1, y, ref adjacentPoints[4]);
            getPointFromSelfOrNeighbour(x - 1, y - 1, ref adjacentPoints[3]);
            getPointFromSelfOrNeighbour(x, y - 1, ref adjacentPoints[2]);
            getPointFromSelfOrNeighbour(x + 1, y - 1, ref adjacentPoints[1]);

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
            clampPoint(ref x, ref y, 1, mSize - 2);

            int faceNum = 2 * 2 * 2;
            int indexNum = 2 * 2 * 6;
            int vertexNum = 3 * 3;

            MVector3[] pointsArray = new MVector3[vertexNum];
            MVector2[] uvArray = new MVector2[vertexNum];
            int[] indexArray = new int[indexNum];
            MVector3[] faceTangents = new MVector3[faceNum];

            getPointFromSelfOrNeighbour(x, y, ref pointsArray[8]);
            getPointFromSelfOrNeighbour(x + 1, y, ref pointsArray[0]);
            getPointFromSelfOrNeighbour(x + 1, y + 1, ref pointsArray[7]);
            getPointFromSelfOrNeighbour(x, y + 1, ref pointsArray[6]);
            getPointFromSelfOrNeighbour(x - 1, y + 1, ref pointsArray[5]);
            getPointFromSelfOrNeighbour(x - 1, y, ref pointsArray[4]);
            getPointFromSelfOrNeighbour(x - 1, y - 1, ref pointsArray[3]);
            getPointFromSelfOrNeighbour(x, y - 1, ref pointsArray[2]);
            getPointFromSelfOrNeighbour(x + 1, y - 1, ref pointsArray[1]);

            getUVFromSelfOrNeighbour(x, y, ref uvArray[8]);
            getUVFromSelfOrNeighbour(x + 1, y, ref uvArray[0]);
            getUVFromSelfOrNeighbour(x + 1, y + 1, ref uvArray[7]);
            getUVFromSelfOrNeighbour(x, y + 1, ref uvArray[6]);
            getUVFromSelfOrNeighbour(x - 1, y + 1, ref uvArray[5]);
            getUVFromSelfOrNeighbour(x - 1, y, ref uvArray[4]);
            getUVFromSelfOrNeighbour(x - 1, y - 1, ref uvArray[3]);
            getUVFromSelfOrNeighbour(x, y - 1, ref uvArray[2]);
            getUVFromSelfOrNeighbour(x + 1, y - 1, ref uvArray[1]);

            int bufferIndex = 0;

            for (int idx = 0; idx < 8; ++idx)
            {
                indexArray[bufferIndex] = 8;
                indexArray[bufferIndex + 1] = idx;
                indexArray[bufferIndex + 2] = (idx + 1) % 8;
                bufferIndex += 3;
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

        public void setWorldSize(float newWorldSize)
        {

        }

        public void setSize(ushort newSize)
        {

        }

        public void showAllNode()
        {
            mQuadTree.show(null);
        }

        override public void show(MFrustum frustum)
        {
            //this.cullNode(frustum);
            if(!mIsInit)
            {
                mIsInit = true;
                mQuadTree.attachMO();
            }
        }

        public Material getMatTmpl()
        {
            if (!mImportData.isUseSplatMap)
            {
                return mTerrainMat.getDiffuseMaterial();
            }
            else
            {
                return mTerrainMat.getSplatMaterial();
            }
        }

        public void checkPoint(ref long x, ref long y)
        {
            if (x < 0)
            {
                x = 0;
            }
            if (x >= mSize)
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

        public void cullNode(MFrustum frustum)
        {
            mQuadTree.cullNode(frustum);
        }

        public void updateAABB()
        {
            mQuadTree.updateAABB();
        }

        public string getLayerStr()
        {
            return m_layerStr;
        }

        override public MAxisAlignedBox getBoundingBox()
        {
            return this.mAABB;
        }

        override public MAxisAlignedBox getWorldBoundingBox(bool derive)
        {
            mWorldAabb.setMinimum(mAABB.getMinimum() + getPosition());
            mWorldAabb.setMaximum(mAABB.getMaximum() + getPosition());

            return this.mWorldAabb;
        }
    }
}