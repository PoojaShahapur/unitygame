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
        protected float mBoundingRadius;
        protected int mCurrentLod;
        protected bool mSelfOrChildRendered;
        protected MVertexDataRecord mVertexDataRecord;

        protected MSceneNode mLocalNode;

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
                mBaseLod = 0;
                mVertexDataRecord = new MVertexDataRecord();
            }

            ushort midoffset = (ushort)((size - 1) / 2);
            ushort midpointx = (ushort)(mOffsetX + midoffset);
            ushort midpointy = (ushort)(mOffsetY + midoffset);

            mTerrain.getPoint(midpointx, midpointy, 0, ref mLocalCentre);
        }

        public bool isLeaf()
        {
            return mChildren[0] == null;
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
            UtilApi.assert(treeDepthStart >= mDepth, "Should not be calling this");

            if (this.isLeaf())
            {
                createCpuVertexData();
            }
        }

        public void createCpuVertexData()
        {
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
                    height = mTerrain.getHeightData(x, y);
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
            mVertexDataRecord.cpuVertexData.m_vertexData[vertexIndex].x = pos.x;
            mVertexDataRecord.cpuVertexData.m_vertexData[vertexIndex].y = pos.y;
            mVertexDataRecord.cpuVertexData.m_vertexData[vertexIndex].z = pos.z;

            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].x = x * uvScale;
            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].y = 1.0f - (y * uvScale);

            if (x != mBoundaryX && y != mBoundaryY)
            {
                int vertexWidth = mTerrain.getMaxBatchSize();
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6 + 1] = vertexIndex + vertexWidth;
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6 + 2] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6 + 3] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6 + 4] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indices[vertexIndex * 6 + 5] = vertexIndex + 1;
            }
        }
    }
}