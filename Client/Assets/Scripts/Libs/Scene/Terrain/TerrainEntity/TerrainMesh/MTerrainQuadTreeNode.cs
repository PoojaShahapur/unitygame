using UnityEngine;

namespace SDK.Lib
{
    public enum QuadTreeChildIndex
    {
        eLEFT_BOTTOM = 0,
        eRIGHT_BOTTOM = 1,
        eLEFT_TOP = 2,
        eRIGHT_TOP = 3,
        eTOTAL,
    }

    public class MTerrainQuadTreeNode
    {
        protected MTerrain mTerrain;
        protected MTerrainQuadTreeNode mParent;
        protected MTerrainQuadTreeNode[] mChildren;

        protected ushort mOffsetX, mOffsetY;
        protected ushort mBoundaryX, mBoundaryY;

        protected ushort mSize;
        protected ushort mBaseLod;
        protected ushort mDepth;
        protected ushort mQuadrant;
        protected MVector3 mLocalCentre;
        protected MAxisAlignedBox mAABB;
        protected MAxisAlignedBox mWorldAabb;
        protected float mBoundingRadius;
        protected int mCurrentLod;
        protected bool mSelfOrChildRendered;
        protected MVertexDataRecord mVertexDataRecord;
        protected TerrainTileRender mTileRender;

        protected MSceneNode mLocalNode;
        protected int mCurIndexBufferIndex;
        protected bool mIsVertexDataInit;

        protected bool m_bShowBoundBox;         // 显示 BoundBox
        protected MAABBMeshRender m_aabbMeshRender;

        public MTerrainQuadTreeNode(MTerrain terrain,
        MTerrainQuadTreeNode parent, ushort xoff, ushort yoff, ushort size,
        ushort lod, ushort depth, ushort quadrant)
        {
            mTerrain = terrain;
            mParent = parent;
            mOffsetX = xoff;
            mOffsetY = yoff;
            mBoundaryX = (ushort)(xoff + size);
            mBoundaryY = (ushort)(yoff + size);
            mSize = size;
            mBaseLod = lod;
            mDepth = depth;
            mQuadrant = quadrant;
            mBoundingRadius = 0;
            mCurrentLod = -1;
            mSelfOrChildRendered = false;
            mLocalNode = null;
            mCurIndexBufferIndex = 0;

            mAABB = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            mWorldAabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            mIsVertexDataInit = false;
            m_bShowBoundBox = true;

            if (terrain.getMaxBatchSize() < size)
            {
                mChildren = new MTerrainQuadTreeNode[(int)QuadTreeChildIndex.eTOTAL];
                ushort childSize = (ushort)(((size - 1) * 0.5f) + 1);
                ushort childOff = (ushort)(childSize - 1);
                ushort childLod = (ushort)(lod - 1);
                ushort childDepth = (ushort)(depth + 1);

                mChildren[(int)QuadTreeChildIndex.eLEFT_BOTTOM] = new MTerrainQuadTreeNode(terrain, this, xoff, yoff, childSize, childLod, childDepth, 0);
                mChildren[(int)QuadTreeChildIndex.eRIGHT_BOTTOM] = new MTerrainQuadTreeNode(terrain, this, (ushort)(xoff + childOff), yoff, childSize, childLod, childDepth, 1);
                mChildren[(int)QuadTreeChildIndex.eLEFT_TOP] = new MTerrainQuadTreeNode(terrain, this, xoff, (ushort)(yoff + childOff), childSize, childLod, childDepth, 2);
                mChildren[(int)QuadTreeChildIndex.eRIGHT_TOP] = new MTerrainQuadTreeNode(terrain, this, (ushort)(xoff + childOff), (ushort)(yoff + childOff), childSize, childLod, childDepth, 3);
            }
            else
            {
                mAABB.setMinimum(new MVector3(-mTerrain.getMaxBatchWorldSize() / 2, -10, -mTerrain.getMaxBatchWorldSize() / 2));
                mAABB.setMaximum(new MVector3(mTerrain.getMaxBatchWorldSize() / 2, 10, mTerrain.getMaxBatchWorldSize() / 2));

                mBaseLod = 0;
                mVertexDataRecord = new MVertexDataRecord();
                mTileRender = new TerrainTileRender(this);
                mTileRender.pntGo = mTerrain._getRootSceneNode().selfGo;
                mTileRender.setTmplMaterial(mTerrain.getMatTmpl());
            }

            ushort midoffset = (ushort)((size - 1) / 2);
            ushort midpointx = (ushort)(mOffsetX + midoffset);
            ushort midpointy = (ushort)(mOffsetY + midoffset);

            mTerrain.getPoint(midpointx, midpointy, 0, ref mLocalCentre);

            if (terrain.getMaxBatchSize() == size)
            {
                updateWorldAABB();
            }
        }

        public bool isLeaf()
        {
            //return mChildren[0] == null;
            return mChildren == null;
        }
        public MTerrainQuadTreeNode getChild(ushort child)
        {
            if (isLeaf() || child >= 4)
                return null;

            return mChildren[child];
        }

        public MTerrainQuadTreeNode getParent()
        {
            return mParent;
        }

        public MTerrain getTerrain()
        {
            return mTerrain;
        }

        public void prepare()
        {
            if (!isLeaf())
            {
                for (int i = 0; i < 4; ++i)
                    mChildren[i].prepare();
            }
        }

        public MAxisAlignedBox getAABB()
        {
            return mAABB;
        }

        public float getBoundingRadius()
        {
            return mBoundingRadius;
        }

        public float getMinHeight()
        {
            switch (mTerrain.getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                default:
                    return mAABB.getMinimum().z;
                case Alignment.ALIGN_X_Z:
                    return mAABB.getMinimum().y;
                case Alignment.ALIGN_Y_Z:
                    return mAABB.getMinimum().x;
            };
        }

        public float getMaxHeight()
        {
            switch (mTerrain.getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                default:
                    return mAABB.getMaximum().z;
                case Alignment.ALIGN_X_Z:
                    return mAABB.getMaximum().y;
                case Alignment.ALIGN_Y_Z:
                    return mAABB.getMaximum().x;
            };
        }

        public bool rectContainsNode(ref MTRectI rect)
        {
            return (rect.left <= mOffsetX && rect.right > mBoundaryX &&
                rect.top <= mOffsetY && rect.bottom > mBoundaryY);
        }

        public void resetBounds(ref MTRectI rect)
        {
            if (rectContainsNode(ref rect))
            {
                mAABB.setNull();
                mBoundingRadius = 0;

                if (!isLeaf())
                {
                    for (int i = 0; i < 4; ++i)
                        mChildren[i].resetBounds(ref rect);
                }
            }
        }

        public void mergeIntoBounds(long x, long y, ref MVector3 pos)
        {
            if (pointIntersectsNode(x, y))
            {
                MVector3 localPos = pos - mLocalCentre;
                mAABB.merge(ref localPos);

                updateWorldAABB();

                mBoundingRadius = UtilMath.max(mBoundingRadius, localPos.length());

                if (!isLeaf())
                {
                    for (int i = 0; i < 4; ++i)
                        mChildren[i].mergeIntoBounds(x, y, ref pos);
                }
            }
        }

        public bool pointIntersectsNode(long x, long y)
        {
            return x >= mOffsetX && x < mBoundaryX &&
                y >= mOffsetY && y < mBoundaryY;
        }

        public void assignVertexData(ushort treeDepthStart, ushort treeDepthEnd, ushort resolution, uint sz)
        {
            //UtilApi.assert(treeDepthStart >= mDepth, "Should not be calling this");

            mIsVertexDataInit = true;

            if (this.isLeaf())
            {
                createCpuVertexData();
            }
            else
            {
                UtilApi.assert(!isLeaf(), "No more levels below this!");

                for (int i = 0; i < 4; ++i)
                    mChildren[i].assignVertexData(treeDepthStart, treeDepthEnd, resolution, sz);
            }
        }

        public void createCpuVertexData()
        {
            mCurIndexBufferIndex = 0;
            MTRectI updateRect = new MTRectI((int)mOffsetX, (int)mOffsetY, (int)mBoundaryX, (int)mBoundaryY);
            updateVertexBuffer(null, null, ref updateRect);
        }

        public void updateVertexBuffer(float[] posbuf, float[] deltabuf, ref MTRectI rect)
        {
            UtilApi.assert(rect.left >= mOffsetX && rect.right <= mBoundaryX &&
                rect.top >= mOffsetY && rect.bottom <= mBoundaryY);

            resetBounds(ref rect);

            float uvScale = 1.0f / (mTerrain.getSize() - 1);
            ushort inc = 1;
            float height = 0;
            MVector3 pos = new MVector3(0, 0, 0);
            for (ushort y = (ushort)rect.top; y < rect.bottom; y += inc)
            {
                for (ushort x = (ushort)rect.left; x < rect.right; x += inc)
                {
                    height = mTerrain.getHeightAtPoint(x, y);
                    mTerrain.getPoint(x, y, height, ref pos);
                    mergeIntoBounds(x, y, ref pos);
                    pos -= mLocalCentre;
                    writePosVertex(x, y, ref pos, uvScale);
                }
            }
        }

        protected void writePosVertex(ushort x, ushort y, ref MVector3 pos, float uvScale)
        {
            int vertexIndex = (y - mOffsetY) * mTerrain.getMaxBatchSize() + (x - mOffsetX);
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].x = pos.x;
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].y = pos.y;
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].z = pos.z;

            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].x = x * uvScale;
            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].y = 1.0f - (y * uvScale);

            if (x != mBoundaryX - 1 && y != mBoundaryY - 1)
            {
                int vertexWidth = mTerrain.getMaxBatchSize();
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 1] = vertexIndex + vertexWidth;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 2] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 3] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 4] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 5] = vertexIndex + 1;

                mCurIndexBufferIndex += 6;
            }

            mTerrain.getPointNormal(x, y, ref mVertexDataRecord.cpuVertexData.m_vertexNormals[vertexIndex]);
            mTerrain.getPointTangent(x, y, ref mVertexDataRecord.cpuVertexData.m_vertexTangents[vertexIndex]);

            //mVertexDataRecord.cpuVertexData.m_vertexNormals[vertexIndex] = new Vector3(0, 1, 0);
            //mVertexDataRecord.cpuVertexData.m_vertexTangents[vertexIndex] = new Vector3(0, 0, 1);
        }

        public Vector3[] getVertexData()
        {
            if (isLeaf())
            {
                return mVertexDataRecord.cpuVertexData.m_vertexs;
            }

            return null;
        }

        public int getVertexDataCount()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexs.Length;
        }

        public int getTriangleCount()
        {
            return mVertexDataRecord.cpuVertexData.m_indexs.Length / 2;
        }

        public Vector2[] getUVData()
        {
            return mVertexDataRecord.cpuVertexData.m_uvs;
        }

        public Color32[] getVectexColorData()
        {
            return null;
        }

        public Vector3[] getVertexNormalsData()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexNormals;
        }

        public Vector4[] getVertexTangentsData()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexTangents;
        }

        public int[] getIndexData()
        {
            return mVertexDataRecord.cpuVertexData.m_indexs;
        }

        public void clear()
        {

        }

        public MVector3 getLocalCentre()
        {
            return mLocalCentre;
        }

        public void show()
        {
            if(isLeaf())
            {
                if(!mIsVertexDataInit)
                {
                    assignVertexData(0, 0, 0, 0);
                }
                mTileRender.show();
                showBoundBox();
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    mChildren[i].show();
                }
            }
        }

        public void hide()
        {
            if (isLeaf())
            {
                mTileRender.hide();
                hideBoundBox();
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    mChildren[i].hide();
                }
            }
        }

        public void updateAABB()
        {
            if (!isLeaf())
            {
                for (int index = 0; index < 4; ++index)
                {
                    mChildren[index].updateAABB();

                    MAxisAlignedBox childBox = mChildren[index].getAABB();
                    MVector3 boxoffset = mChildren[index].getLocalCentre() - getLocalCentre();
                    childBox.setMinimum(childBox.getMinimum() + boxoffset);
                    childBox.setMaximum(childBox.getMaximum() + boxoffset);
                    mAABB.merge(childBox);

                    //MMatrix4 worldMat = new MMatrix4();
                    //mWorldAabb.transformAffine(ref worldMat);

                    updateWorldAABB();
                }
            }
            else
            {
                updateWorldAABB();
            }
        }

        public void cullNode(MFrustum frustum)
        {
            FrustumPlane culledBy = FrustumPlane.FRUSTUM_PLANE_LEFT;
            if (frustum.isVisible(ref mWorldAabb, ref culledBy))
            {
                if(isLeaf())
                {
                    show();
                    frustum.isVisible(ref mWorldAabb, ref culledBy);
                }
                else
                {
                    for (int index = 0; index < 4; ++index)
                    {
                        mChildren[index].cullNode(frustum);
                    }
                }
            }
            else
            {
                hide();
            }
        }

        public string getLayerStr()
        {
            return mTerrain.getLayerStr();
        }

        public void updateWorldAABB()
        {
            mWorldAabb.setMinimum(mAABB.getMinimum() + mLocalCentre + mTerrain.getPosition());
            mWorldAabb.setMaximum(mAABB.getMaximum() + mLocalCentre + mTerrain.getPosition());
        }

        protected void showBoundBox()
        {
            if (m_bShowBoundBox)
            {
                if (m_aabbMeshRender == null)
                {
                    m_aabbMeshRender = new MAABBMeshRender();
                }

                m_aabbMeshRender.addVertex(ref mWorldAabb);
                m_aabbMeshRender.buildIndex();
                m_aabbMeshRender.uploadGeometry();
                m_aabbMeshRender.show();
            }
        }

        protected void hideBoundBox()
        {
            if (m_bShowBoundBox)
            {
                if (m_aabbMeshRender != null)
                {
                    m_aabbMeshRender.hide();
                }
            }
        }
    }
}