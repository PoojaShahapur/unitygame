namespace SDK.Lib
{
    public class MOctreeNode : MSceneNode
    {
        protected MAxisAlignedBox mLocalAABB;
        protected MOctree mOctant;
        protected float[] mCorners;

        public MOctreeNode( MSceneManager creator )
            : base(creator)
        {
            mOctant = null;
        }

        public MOctreeNode( MSceneManager creator, string name ) 
            : base(creator, name )
        {
            mOctant = null;
        }

        public void _removeNodeAndChildren()
        {
            (MOctreeSceneManager)(mCreator)._removeOctreeNode(this);
            ChildNodeMap::iterator it = mChildren.begin();
            while (it != mChildren.end())
            {
                static_cast<OctreeNode*>(it->second)->_removeNodeAndChildren();
                ++it;
            }
        }

        override public MNode removeChild(ushort index)
        {
            MOctreeNode on = (MOctreeNode)(base.removeChild(index));
            on._removeNodeAndChildren();
            return on;
        }

        override public MNode removeChild(MNode child)
        {
            MOctreeNode on = (MOctreeNode)(base.removeChild(child));
            on._removeNodeAndChildren();
            return on;
        }

        public void removeAllChildren()
        {
            ChildNodeMap::iterator i, iend;
            iend = mChildren.end();
            for (i = mChildren.begin(); i != iend; ++i)
            {
                OctreeNode* on = static_cast<OctreeNode*>(i->second);
                on->setParent(0);
                on->_removeNodeAndChildren();
            }
            mChildren.clear();
            mChildrenToUpdate.clear();
        }

        public MNode removeChild( string name )
{
    OctreeNode* on = static_cast<OctreeNode*>(SceneNode::removeChild(name));
        on -> _removeNodeAndChildren(); 
    return on; 
}

    public void _updateBounds()
    {
        mWorldAABB.setNull();
        mLocalAABB.setNull();

        // Update bounds from own attached objects
        ObjectMap::iterator i = mObjectsByName.begin();
        AxisAlignedBox bx;

        while (i != mObjectsByName.end())
        {

            // Get local bounds of object
            bx = i->second->getBoundingBox();

            mLocalAABB.merge(bx);

            mWorldAABB.merge(i->second->getWorldBoundingBox(true));
            ++i;
        }


        //update the OctreeSceneManager that things might have moved.
        // if it hasn't been added to the octree, add it, and if has moved
        // enough to leave it's current node, we'll update it.
        if (!mWorldAABB.isNull() && mIsInSceneGraph)
        {
            static_cast<OctreeSceneManager*>(mCreator)->_updateOctreeNode(this);
        }

    }

    public bool _isIn(MAxisAlignedBox box )
    {
        // Always fail if not in the scene graph or box is null
        if (!mIsInSceneGraph || box.isNull()) return false;

        // Always succeed if AABB is infinite
        if (box.isInfinite())
            return true;

            MVector3 min = mWorldAABB.getMinimum();
        MVector3 center = mWorldAABB.getMaximum().midPoint(ref min);

        MVector3 bmin = box.getMinimum();
        MVector3 bmax = box.getMaximum();

        bool centre = (bmax > center && bmin < center);
        if (!centre)
            return false;

        MVector3 octreeSize = bmax - bmin;
        MVector3 nodeSize = mWorldAABB.getMaximum() - mWorldAABB.getMinimum();
        return nodeSize < octreeSize;
    }

    public MOctree getOctant()
    {
        return mOctant;
    }

    public void setOctant(MOctree o)
    {
        mOctant = o;
    }

    public MAxisAlignedBox _getLocalAABB()
    {
        return mLocalAABB;
    }
}
}