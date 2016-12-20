namespace SDK.Lib
{
    public class MOctree
    {
        public MAxisAlignedBox mBox;
        public MVector3 mHalfSize;
        public MOctree[,,] mChildren;

        public MList<MOctreeNode> mNodes;
        protected int mNumNodes;

        protected MOctree mParent;

        public MAxisAlignedBox mEntityWorldBox;
        public MVector3 mEntityWorldBoxHalfSize;
        protected bool mEntityWorldBoxNeedUpdate;

        public MOctree(MOctree parent)
        {
            mNodes = new MList<MOctreeNode>();
            mHalfSize = new MVector3(0, 0, 0);
            mChildren = new MOctree[2, 2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        mChildren[i, j, k] = null;
                    }
                }
            }

            mParent = parent;
            mNumNodes = 0;

            mEntityWorldBox = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            mEntityWorldBoxNeedUpdate = false;
        }

        public void _addNode(MOctreeNode n)
        {
            mNodes.Add(n);
            n.setOctant(this);

            _ref();
        }

        public void _removeNode(MOctreeNode n)
        {
            mNodes.Remove(n);
            n.setOctant(null);

            //update total counts.
            _unref();
        }

        public void _getCullBounds(ref MAxisAlignedBox b)
        {
            /*
            MVector3 min = mBox.getMinimum() - mHalfSize;
            MVector3 max = mBox.getMaximum() + mHalfSize;
            b.setExtents(ref min, ref max);
            */

            /*
            MVector3 min = mEntityWorldBox.getMinimum() - mEntityWorldBoxHalfSize;
            MVector3 max = mEntityWorldBox.getMaximum() + mEntityWorldBoxHalfSize;
            b.setExtents(ref min, ref max);
            */

            b = mEntityWorldBox;
        }

        public int numNodes()
        {
            return mNumNodes;
        }

        public void _ref()
        {
            mNumNodes++;

            if (mParent != null) mParent._ref();
        }

        public void _unref()
        {
            mNumNodes--;

            if (mParent != null) mParent._unref();
        }

        public bool _isTwiceSize(MAxisAlignedBox box)
        {
            if (box.isInfinite())
                return false;

            MVector3 halfMBoxSize = mBox.getHalfSize();
            MVector3 boxSize = box.getSize();
            return ((boxSize.x <= halfMBoxSize.x) && (boxSize.y <= halfMBoxSize.y) && (boxSize.z <= halfMBoxSize.z));
        }

        public void _getChildIndexes(MAxisAlignedBox box, ref int x, ref int y, ref int z)
        {
            MVector3 tmp = mBox.getMinimum();
            MVector3 center = mBox.getMaximum().midPoint(ref tmp);
            tmp = box.getMinimum();
            MVector3 ncenter = box.getMaximum().midPoint(ref tmp);

            if (ncenter.x > center.x)
                x = 1;
            else
                x = 0;

            if (ncenter.y > center.y)
                y = 1;
            else
                y = 0;

            if (ncenter.z > center.z)
                z = 1;
            else
                z = 0;
        }

        public bool getEntityWorldBoxNeedUpdate()
        {
            return mEntityWorldBoxNeedUpdate;
        }

        public void setEntityWorldBoxNeedUpdate(bool value)
        {
            mEntityWorldBoxNeedUpdate = value;
            if(mParent != null)
            {
                mParent.setEntityWorldBoxNeedUpdate(mEntityWorldBoxNeedUpdate);
            }
        }

        public MAxisAlignedBox getEntityWorldBox()
        {
            return mEntityWorldBox;
        }

        public void updateEntityWorldBox()
        {
            mEntityWorldBoxNeedUpdate = false;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (mChildren[i, j, k] != null)
                        {
                            mChildren[i, j, k].updateEntityWorldBox();
                        }
                    }
                }
            }

            mEntityWorldBox.setNull();

            int idx = 0;
            int len = 0;
            len = mNodes.Count();
            while(idx < len)
            {
                mEntityWorldBox.merge(mNodes[idx]._getWorldAABB());
                ++idx;
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (mChildren[i, j, k] != null)
                        {
                            mEntityWorldBox.merge(mChildren[i, j, k].getEntityWorldBox());
                        }
                    }
                }
            }

            mEntityWorldBoxHalfSize = (mEntityWorldBox.getMaximum() - mEntityWorldBox.getMinimum()) / 2;
        }
    }
}