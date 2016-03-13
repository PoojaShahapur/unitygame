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
    }
}