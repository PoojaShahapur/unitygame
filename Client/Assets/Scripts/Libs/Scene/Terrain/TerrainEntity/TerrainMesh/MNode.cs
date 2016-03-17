namespace SDK.Lib
{
    public enum TransformSpace
    {
        TS_LOCAL,
        TS_PARENT,
        TS_WORLD,
    }

    public class Listener
    {
        public Listener()
        {

        }

        public virtual void nodeUpdated(MNode node)
        {

        }

        public virtual void nodeDestroyed(MNode node)
        {

        }

        public virtual void nodeAttached(MNode node)
        {

        }

        public virtual void nodeDetached(MNode node)
        {

        }
    }

    public class MNode : AuxComponent
    {
        protected string mName;
        protected MQuaternion mOrientation;
        protected MVector3 mPosition;
        protected MVector3 mScale;
        protected bool mInheritOrientation;
        protected bool mInheritScale;

        public void setPosition(MVector3 pos)
        {
            UtilApi.setPos(this.selfGo.transform, pos.toNative());
        }

        public MNode()
        {
            mParent = 0;
            mNeedParentUpdate = false;
            mNeedChildUpdate = false;
            mParentNotified = false;
            mQueuedForUpdate = false;
            mOrientation = MQuaternion.IDENTITY;
            mPosition = MVector3.ZERO;
            mScale = MVector3.UNIT_SCALE;
            mInheritOrientation = true;
            mInheritScale = true;
            mDerivedOrientation = MQuaternion.IDENTITY;
            mDerivedPosition = MVector3.ZERO;
            mDerivedScale = MVector3.UNIT_SCALE;
            mInitialPosition = MVector3.ZERO;
            mInitialOrientation = MQuaternion.IDENTITY;
            mInitialScale = MVector3.UNIT_SCALE;
            mCachedTransformOutOfDate = true;
            mListener = 0;
            mDebug = 0;

            mName = "";

            needUpdate();

        }
        //-----------------------------------------------------------------------
        public MNode(string name)
        {
            mParent = 0;
        mNeedParentUpdate = false;
        mNeedChildUpdate = false;
        mParentNotified = false;
        mQueuedForUpdate = false;
        mName = name;
        mOrientation = MQuaternion.IDENTITY;
        mPosition = MVector3.ZERO;
        mScale = MVector3.UNIT_SCALE;
        mInheritOrientation = true;
        mInheritScale = true;
        mDerivedOrientation = MQuaternion.IDENTITY;
        mDerivedPosition = MVector3.ZERO;
        mDerivedScale = MVector3.UNIT_SCALE;
        mInitialPosition = MVector3.ZERO;
        mInitialOrientation = MQuaternion.IDENTITY;
        mInitialScale = MVector3.UNIT_SCALE;
        mCachedTransformOutOfDate = true;
        mListener(0);
        mDebug(0);
            needUpdate();
        }

        MNode getParent()
    {
        return mParent;
    }

    void setParent(MNode parent)
    {
        bool different = (parent != mParent);

        mParent = parent;
        // Request update from parent
        mParentNotified = false;
        needUpdate();

        // Call listener (note, only called if there's something to do)
        if (mListener && different)
        {
            if (mParent)
                mListener->nodeAttached(this);
            else
                mListener->nodeDetached(this);
        }

    }

     public MMatrix4 _getFullTransform()
    {
        if (mCachedTransformOutOfDate)
        {
#if OGRE_NODE_INHERIT_TRANSFORM
            Ogre::Matrix4 tr;
            tr.makeTransform(mPosition, mScale, mOrientation);

            if(mParent == NULL)
            {
                mCachedTransform = tr;
            }
            else if(mInheritOrientation && mInheritScale) // everything is inherited
            {
                mCachedTransform = mParent->_getFullTransform() * tr;
            }
            else if(!mInheritOrientation && !mInheritScale) // only position is inherited
            {
                mCachedTransform = tr;
                mCachedTransform.setTrans(tr.getTrans() + mParent->_getFullTransform().getTrans());
            }
            else // shear is inherited together with orientation, controlled by mInheritOrientation
            {
                const Ogre::Matrix4& parentTr = mParent->_getFullTransform();
                Ogre::Vector3 parentScale(
                    parentTr.transformDirectionAffine(Vector3::UNIT_X).length(),
                    parentTr.transformDirectionAffine(Vector3::UNIT_Y).length(),
                    parentTr.transformDirectionAffine(Vector3::UNIT_Z).length());

                assert(mInheritOrientation ^ mInheritScale);
                mCachedTransform = (mInheritOrientation ? Matrix4::getScale(1.0f / parentScale)  * parentTr : Matrix4::getScale(parentScale)) * tr;
            }
#else
            // Use derived values
            mCachedTransform.makeTransform(
                _getDerivedPosition(),
                _getDerivedScale(),
                _getDerivedOrientation());
#endif
            mCachedTransformOutOfDate = false;
        }
        return mCachedTransform;
    }

    public void _update(bool updateChildren, bool parentHasChanged)
{
    // always clear information about parent notification
    mParentNotified = false;

    // See if we should process everyone
    if (mNeedParentUpdate || parentHasChanged)
    {
        // Update transforms from parent
        _updateFromParent();
    }

    if (updateChildren)
    {
        if (mNeedChildUpdate || parentHasChanged)
        {
            ChildNodeMap::iterator it, itend;
            itend = mChildren.end();
            for (it = mChildren.begin(); it != itend; ++it)
            {
                Node* child = it->second;
                child->_update(true, true);
            }
        }
        else
        {
            // Just update selected children
            ChildUpdateSet::iterator it, itend;
            itend = mChildrenToUpdate.end();
            for (it = mChildrenToUpdate.begin(); it != itend; ++it)
            {
                Node* child = *it;
                child->_update(true, false);
            }

        }

        mChildrenToUpdate.clear();
        mNeedChildUpdate = false;
    }
}

public void _updateFromParent()
    {
        updateFromParentImpl();

        // Call listener (note, this method only called if there's something to do)
        if (mListener)
        {
            mListener->nodeUpdated(this);
        }
    }

    public void updateFromParentImpl()
    {
        mCachedTransformOutOfDate = true;

        if (mParent)
        {
#if OGRE_NODE_INHERIT_TRANSFORM
            // Decompose full transform to position, orientation and scale, shear is lost here.
            _getFullTransform().decomposition(mDerivedPosition, mDerivedScale, mDerivedOrientation);
#else
            // Update orientation
            const Quaternion& parentOrientation = mParent->_getDerivedOrientation();
            if (mInheritOrientation)
            {
                // Combine orientation with that of parent
                mDerivedOrientation = parentOrientation* mOrientation;
            }
            else
            {
                // No inheritance
                mDerivedOrientation = mOrientation;
            }

            // Update scale
            const Vector3& parentScale = mParent->_getDerivedScale();
            if (mInheritScale)
            {
                // Scale own position by parent scale, NB just combine
                // as equivalent axes, no shearing
                mDerivedScale = parentScale* mScale;
            }
            else
            {
                // No inheritance
                mDerivedScale = mScale;
            }

            // Change position vector based on parent's orientation & scale
            mDerivedPosition = parentOrientation* (parentScale* mPosition);

            // Add altered position vector to parents
            mDerivedPosition += mParent->_getDerivedPosition();
#endif
        }
        else
        {
            // Root node, no parent
            mDerivedOrientation = mOrientation;
            mDerivedPosition = mPosition;
            mDerivedScale = mScale;
        }

        mNeedParentUpdate = false;

    }

    public MNode createChild(MVector3 inTranslate, MQuaternion inRotate)
{
    Node* newNode = createChildImpl();
    newNode->translate(inTranslate);
    newNode->rotate(inRotate);
    this->addChild(newNode);

    return newNode;
}

public MNode createChild(string name, MVector3 inTranslate, MQuaternion inRotate)
{
    Node* newNode = createChildImpl(name);
    newNode->translate(inTranslate);
    newNode->rotate(inRotate);
    this->addChild(newNode);

    return newNode;
}

public void addChild(MNode child)
{
    if (child->mParent)
    {
        OGRE_EXCEPT(Exception::ERR_INVALIDPARAMS,
            "Node '" + child->getName() + "' already was a child of '" +
            child->mParent->getName() + "'.",
            "Node::addChild");
    }

    mChildren.insert(ChildNodeMap::value_type(child->getName(), child));
    child->setParent(this);

}

public ushort numChildren()
    {
        return static_cast<unsigned short>( mChildren.size() );
    }

    public Node getChild(ushort index)
    {
        if( index<mChildren.size() )
        {
            ChildNodeMap::const_iterator i = mChildren.begin();
            while (index--) ++i;
            return i->second;
        }
        else
            return NULL;
    }
    //-----------------------------------------------------------------------
    public MNode removeChild(ushort index)
{
    if (index < mChildren.size())
    {
        ChildNodeMap::iterator i = mChildren.begin();
        while (index--) ++i;
        Node* ret = i->second;
        // cancel any pending update
        cancelUpdate(ret);

        mChildren.erase(i);
        ret->setParent(NULL);
        return ret;
    }
    else
    {
        OGRE_EXCEPT(
            Exception::ERR_INVALIDPARAMS,
            "Child index out of bounds.",
            "Node::getChild");
    }
    return 0;
}
//-----------------------------------------------------------------------
public MNode removeChild(MNode child)
{
    if (child)
    {
        ChildNodeMap::iterator i = mChildren.find(child->getName());
        // ensure it's our child
        if (i != mChildren.end() && i->second == child)
        {
            // cancel any pending update
            cancelUpdate(child);

            mChildren.erase(i);
            child->setParent(NULL);
        }
    }
    return child;
}

public MQuaternion getOrientation()
    {
        return mOrientation;
    }


    public void setOrientation(MQuaternion q )
{
    assert(!q.isNaN() && "Invalid orientation supplied as parameter");
    mOrientation = q;
    mOrientation.normalise();
    needUpdate();
}

public void setOrientation(float w, float x, float y, float z)
{
    setOrientation(Quaternion(w, x, y, z));
}

public void resetOrientation()
{
    mOrientation = Quaternion::IDENTITY;
    needUpdate();
}

public void setPosition(MVector3 pos)
{
    assert(!pos.isNaN() && "Invalid vector supplied as parameter");
    mPosition = pos;
    needUpdate();
}

public void setPosition(float x, float y, float z)
{
    Vector3 v(x, y, z);
    setPosition(v);
}

public MVector3 getPosition()
    {
        return mPosition;
    }
    
    public MMatrix3 getLocalAxes()
    {
        Vector3 axisX = Vector3::UNIT_X;
Vector3 axisY = Vector3::UNIT_Y;
Vector3 axisZ = Vector3::UNIT_Z;

axisX = mOrientation* axisX;
axisY = mOrientation* axisY;
axisZ = mOrientation* axisZ;

        return Matrix3(axisX.x, axisY.x, axisZ.x,
                       axisX.y, axisY.y, axisZ.y,
                       axisX.z, axisY.z, axisZ.z);
    }

    public void translate(Vector3& d, TransformSpace relativeTo)
{
    switch (relativeTo)
    {
        case TS_LOCAL:
            // position is relative to parent so transform downwards
            mPosition += mOrientation * d;
            break;
        case TS_WORLD:
            // position is relative to parent so transform upwards
            if (mParent)
            {
                mPosition += mParent->convertWorldToLocalDirection(d, true);
            }
            else
            {
                mPosition += d;
            }
            break;
        case TS_PARENT:
            mPosition += d;
            break;
    }
    needUpdate();

}

public void translate(float x, float y, float z, TransformSpace relativeTo)
{
    Vector3 v(x, y, z);
    translate(v, relativeTo);
}

public void translate(MMatrix3 axes, MVector3 move, TransformSpace relativeTo)
{
    Vector3 derived = axes * move;
    translate(derived, relativeTo);
}

public void translate(MMatrix3 axes, float x, float y, float z, TransformSpace relativeTo)
{
    Vector3 d(x, y, z);
    translate(axes, d, relativeTo);
}

public void roll(MRadian angle, TransformSpace relativeTo)
{
    rotate(Vector3::UNIT_Z, angle, relativeTo);
}

public void pitch(MRadian angle, TransformSpace relativeTo)
{
    rotate(Vector3::UNIT_X, angle, relativeTo);
}

public void yaw(MRadian angle, TransformSpace relativeTo)
{
    rotate(Vector3::UNIT_Y, angle, relativeTo);

}

public void rotate(MVector3 axis, MRadian angle, TransformSpace relativeTo)
{
    Quaternion q;
    q.FromAngleAxis(angle, axis);
    rotate(q, relativeTo);
}

public void rotate(const Quaternion& q, TransformSpace relativeTo)
{
    switch (relativeTo)
    {
        case TS_PARENT:
            // Rotations are normally relative to local axes, transform up
            mOrientation = q * mOrientation;
            break;
        case TS_WORLD:
            // Rotations are normally relative to local axes, transform up
            mOrientation = mOrientation * _getDerivedOrientation().Inverse()
                * q * _getDerivedOrientation();
            break;
        case TS_LOCAL:
            // Note the order of the mult, i.e. q comes after
            mOrientation = mOrientation * q;
            break;
    }

    // Normalise quaternion to avoid drift
    mOrientation.normalise();

    needUpdate();
}

public void _setDerivedPosition( const Vector3& pos )
{
    //find where the node would end up in parent's local space
    if (mParent)
        setPosition(mParent->convertWorldToLocalPosition(pos));
}

public void _setDerivedOrientation( const Quaternion& q )
{
    //find where the node would end up in parent's local space
    if (mParent)
        setOrientation(mParent->convertWorldToLocalOrientation(q));
}

public Quaternion & Node::_getDerivedOrientation(void) const
    {
        if (mNeedParentUpdate)
        {
            _updateFromParent();
        }
        return mDerivedOrientation;
    }

    public const Vector3 & Node::_getDerivedPosition(void) const
    {
        if (mNeedParentUpdate)
        {
            _updateFromParent();
        }
        return mDerivedPosition;
    }
    
    public const Vector3 & Node::_getDerivedScale(void) const
    {
        if (mNeedParentUpdate)
        {
            _updateFromParent();
        }
        return mDerivedScale;
    }

    public Vector3 Node::convertWorldToLocalPosition( const Vector3 &worldPos )
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }
#if OGRE_NODE_INHERIT_TRANSFORM
        return _getFullTransform().inverseAffine().transformAffine(worldPos);
#else
    return mDerivedOrientation.Inverse() * (worldPos - mDerivedPosition) / mDerivedScale;
#endif
}

public Vector3 Node::convertLocalToWorldPosition( const Vector3 &localPos )
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }
    return _getFullTransform().transformAffine(localPos);
}

public Vector3 Node::convertWorldToLocalDirection( const Vector3 &worldDir, bool useScale)
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }

    return useScale ?
#if OGRE_NODE_INHERIT_TRANSFORM
            _getFullTransform().inverseAffine().transformDirectionAffine(worldDir) :
            mDerivedOrientation.Inverse() * worldDir;
#else
            mDerivedOrientation.Inverse() * worldDir / mDerivedScale :
        mDerivedOrientation.Inverse() * worldDir;
#endif
}

public Vector3 Node::convertLocalToWorldDirection( const Vector3 &localDir, bool useScale)
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }
    return useScale ?
        _getFullTransform().transformDirectionAffine(localDir) :
        mDerivedOrientation * localDir;
}

public Quaternion Node::convertWorldToLocalOrientation( const Quaternion &worldOrientation )
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }
    return mDerivedOrientation.Inverse() * worldOrientation;
}

public Quaternion Node::convertLocalToWorldOrientation( const Quaternion &localOrientation )
{
    if (mNeedParentUpdate)
    {
        _updateFromParent();
    }
    return mDerivedOrientation * localOrientation;

}

public void Node::removeAllChildren(void)
{
    ChildNodeMap::iterator i, iend;
    iend = mChildren.end();
    for (i = mChildren.begin(); i != iend; ++i)
    {
        i->second->setParent(0);
    }
    mChildren.clear();
    mChildrenToUpdate.clear();
}

public void Node::setScale(const Vector3& inScale)
{
    assert(!inScale.isNaN() && "Invalid vector supplied as parameter");
    mScale = inScale;
    needUpdate();
}

public void Node::setScale(Real x, Real y, Real z)
{
    setScale(Vector3(x, y, z));
}

public Vector3 & Node::getScale(void) const
    {
        return mScale;
    }
    
    public void Node::setInheritOrientation(bool inherit)
{
    mInheritOrientation = inherit;
    needUpdate();
}

public bool Node::getInheritOrientation(void) const
    {
        return mInheritOrientation;
    }

    public void setInheritScale(bool inherit)
{
    mInheritScale = inherit;
    needUpdate();
}

public bool getInheritScale()
    {
        return mInheritScale;
    }

    public void scale(MVector3 inScale)
{
    mScale = mScale * inScale;
    needUpdate();

}

public void Node::scale(Real x, Real y, Real z)
{
    mScale.x *= x;
    mScale.y *= y;
    mScale.z *= z;
    needUpdate();

}

public String& Node::getName(void) const
    {
        return mName;
    }

    public void Node::setInitialState(void)
{
    mInitialPosition = mPosition;
    mInitialOrientation = mOrientation;
    mInitialScale = mScale;
}

public void Node::resetToInitialState(void)
{
    mPosition = mInitialPosition;
    mOrientation = mInitialOrientation;
    mScale = mInitialScale;

    needUpdate();
}

public Vector3& Node::getInitialPosition(void) const
    {
        return mInitialPosition;
    }

    public Quaternion& Node::getInitialOrientation(void) const
    {
        return mInitialOrientation;

    }

    public const Vector3& Node::getInitialScale(void) const
    {
        return mInitialScale;
    }

    public Node Node::getChild(const String& name) const
    {
        ChildNodeMap::const_iterator i = mChildren.find(name);

        if (i == mChildren.end())
        {
            OGRE_EXCEPT(Exception::ERR_ITEM_NOT_FOUND, "Child node named " + name +
                " does not exist.", "Node::getChild");
        }
        return i->second;

    }

    public MNode removeChild(string name)
{
    ChildNodeMap::iterator i = mChildren.find(name);

    if (i == mChildren.end())
    {
        OGRE_EXCEPT(Exception::ERR_ITEM_NOT_FOUND, "Child node named " + name +
            " does not exist.", "Node::removeChild");
    }

    Node* ret = i->second;
    // Cancel any pending update
    cancelUpdate(ret);

    mChildren.erase(i);
    ret->setParent(NULL);

    return ret;
}

public ChildNodeIterator getChildIterator()
{
    return ChildNodeIterator(mChildren.begin(), mChildren.end());
}

public ConstChildNodeIterator getChildIterator()
    {
        return ConstChildNodeIterator(mChildren.begin(), mChildren.end());
    }

    float getSquaredViewDepth(MCamera cam)
    {
        Vector3 diff = _getDerivedPosition() - cam->getDerivedPosition();

        // NB use squared length rather than real depth to avoid square root
        return diff.squaredLength();
    }

    public void needUpdate(bool forceParentUpdate)
{

    mNeedParentUpdate = true;
    mNeedChildUpdate = true;
    mCachedTransformOutOfDate = true;

    // Make sure we're not root and parent hasn't been notified before
    if (mParent && (!mParentNotified || forceParentUpdate))
    {
        mParent->requestUpdate(this, forceParentUpdate);
        mParentNotified = true;
    }

    // all children will be updated
    mChildrenToUpdate.clear();
}

public void requestUpdate(MNode child, bool forceParentUpdate)
{
    // If we're already going to update everything this doesn't matter
    if (mNeedChildUpdate)
    {
        return;
    }

    mChildrenToUpdate.insert(child);
    // Request selective update of me, if we didn't do it before
    if (mParent && (!mParentNotified || forceParentUpdate))
    {
        mParent->requestUpdate(this, forceParentUpdate);
        mParentNotified = true;
    }

}

public void cancelUpdate(Node* child)
{
    mChildrenToUpdate.erase(child);

    // Propagate this up if we're done
    if (mChildrenToUpdate.empty() && mParent && !mNeedChildUpdate)
    {
        mParent->cancelUpdate(this);
        mParentNotified = false;
    }
}


}
}