using System.Collections.Generic;

namespace SDK.Lib
{
    public class MSceneNode : MNode
    {
        protected bool mShowBoundingBox;
        protected bool mHideBoundingBox;
        protected MSceneManager mCreator;
        protected MAxisAlignedBox mWorldAABB;
        protected bool mYawFixed;
        protected MVector3 mYawFixedAxis;
        protected MSceneNode mAutoTrackTarget;
        protected MVector3 mAutoTrackOffset;
        protected MVector3 mAutoTrackLocalDirection;
        protected bool mIsInSceneGraph;
        protected Dictionary<string, MMovableObject> mObjectsByName;

        public MSceneNode(string name = "")
        {
            selfGo = UtilApi.createGameObject(name);
        }

        public MSceneNode(MSceneManager creator)
        {
            mShowBoundingBox = false;
            mHideBoundingBox = false;
            mCreator = creator;
            mYawFixed = false;
            mAutoTrackTarget = null;
            mIsInSceneGraph = false;

            needUpdate();
        }

        public MSceneNode(MSceneManager creator, string name)
        {
            mShowBoundingBox = false;
            mHideBoundingBox = false;
            mCreator = creator;
            mYawFixed = false;
            mAutoTrackTarget = null;
            mIsInSceneGraph = false;
            needUpdate();
        }

        override public void _update(bool updateChildren, bool parentHasChanged)
        {
            _update(updateChildren, parentHasChanged);
            _updateBounds();
        }

        override public void setParent(MNode parent)
        {
            setParent(parent);

            if (parent != null)
            {
                MSceneNode sceneParent = (MSceneNode)(parent);
                setInSceneGraph(sceneParent.isInSceneGraph());
            }
            else
            {
                setInSceneGraph(false);
            }
        }

        public void setInSceneGraph(bool inGraph)
        {
            if (inGraph != mIsInSceneGraph)
            {
                mIsInSceneGraph = inGraph;
                foreach (KeyValuePair<string, MNode> kv in mChildren)
                {
                    MSceneNode sceneChild = (MSceneNode)(kv.Value);
                    sceneChild.setInSceneGraph(inGraph);
                }
            }
        }

    //    public void attachObject(MovableObject* obj)
    //    {
    //        if (obj->isAttached())
    //        {
    //            OGRE_EXCEPT(Exception::ERR_INVALIDPARAMS,
    //                "Object already attached to a SceneNode or a Bone",
    //                "SceneNode::attachObject");
    //        }

    //        obj->_notifyAttached(this);

    //        // Also add to name index
    //        std::pair<ObjectMap::iterator, bool> insresult =
    //            mObjectsByName.insert(ObjectMap::value_type(obj->getName(), obj));
    //        assert(insresult.second && "Object was not attached because an object of the "
    //            "same name was already attached to this node.");
    //        (void)insresult;

    //        // Make sure bounds get updated (must go right to the top)
    //        needUpdate();
    //    }

    //    public unsigned short SceneNode::numAttachedObjects(void) const
    //    {
    //    return static_cast<unsigned short>( mObjectsByName.size() );
    //}

    //public MovableObject getAttachedObject(unsigned short index)
    //{
    //    if (index < mObjectsByName.size())
    //    {
    //        ObjectMap::iterator i = mObjectsByName.begin();
    //        // Increment (must do this one at a time)            
    //        while (index--) ++i;

    //        return i->second;
    //    }
    //    else
    //    {
    //        OGRE_EXCEPT(Exception::ERR_INVALIDPARAMS, "Object index out of bounds.", "SceneNode::getAttachedObject");
    //    }
    //}

    //public MovableObject getAttachedObject(const String& name)
    //{
    //    // Look up 
    //    ObjectMap::iterator i = mObjectsByName.find(name);

    //    if (i == mObjectsByName.end())
    //    {
    //        OGRE_EXCEPT(Exception::ERR_ITEM_NOT_FOUND, "Attached object " +
    //            name + " not found.", "SceneNode::getAttachedObject");
    //    }

    //    return i->second;

    //}

    //public MovableObject detachObject(unsigned short index)
    //{
    //    if (index < mObjectsByName.size())
    //    {

    //        ObjectMap::iterator i = mObjectsByName.begin();
    //        // Increment (must do this one at a time)            
    //        while (index--) ++i;

    //        MovableObject* ret = i->second;
    //        mObjectsByName.erase(i);
    //        ret->_notifyAttached((SceneNode*)0);

    //        // Make sure bounds get updated (must go right to the top)
    //        needUpdate();

    //        return ret;
    //    }
    //    else
    //    {
    //        OGRE_EXCEPT(Exception::ERR_INVALIDPARAMS, "Object index out of bounds.", "SceneNode::getAttchedEntity");
    //    }

    //}

    //public MovableObject detachObject(const String& name)
    //{
    //    ObjectMap::iterator it = mObjectsByName.find(name);
    //    if (it == mObjectsByName.end())
    //    {
    //        OGRE_EXCEPT(Exception::ERR_ITEM_NOT_FOUND, "Object " + name + " is not attached "
    //            "to this node.", "SceneNode::detachObject");
    //    }
    //    MovableObject* ret = it->second;
    //    mObjectsByName.erase(it);
    //    ret->_notifyAttached((SceneNode*)0);
    //    // Make sure bounds get updated (must go right to the top)
    //    needUpdate();

    //    return ret;

    //}

    //public void detachObject(MovableObject* obj)
    //{
    //    ObjectMap::iterator i, iend;
    //    iend = mObjectsByName.end();
    //    for (i = mObjectsByName.begin(); i != iend; ++i)
    //    {
    //        if (i->second == obj)
    //        {
    //            mObjectsByName.erase(i);
    //            break;
    //        }
    //    }
    //    obj->_notifyAttached((SceneNode*)0);

    //    // Make sure bounds get updated (must go right to the top)
    //    needUpdate();

    //}

    //public void detachAllObjects(void)
    //{
    //    ObjectMap::iterator itr;
    //    for (itr = mObjectsByName.begin(); itr != mObjectsByName.end(); ++itr)
    //    {
    //        MovableObject* ret = itr->second;
    //        ret->_notifyAttached((SceneNode*)0);
    //    }
    //    mObjectsByName.clear();
    //    // Make sure bounds get updated (must go right to the top)
    //    needUpdate();
    //}

        public virtual bool isInSceneGraph()
        {
            return mIsInSceneGraph;
        }
    
    public virtual void _notifyRootNode()
        {
            mIsInSceneGraph = true;
        }

    public void _updateBounds()
    {
        mWorldAABB.setNull();

        foreach (KeyValuePair<string, MMovableObject> kv in mObjectsByName)
        {
            mWorldAABB.merge(kv.Value.getWorldBoundingBox(true));
        }

        foreach (KeyValuePair<string, MNode> kv in mChildren)
        {
            MSceneNode sceneChild = (MSceneNode)(kv.Value);
            mWorldAABB.merge(sceneChild.mWorldAABB);
        }

    }

    public void showBoundingBox(bool bShow)
    {
        mShowBoundingBox = bShow;
    }

    public bool getShowBoundingBox()
    {
        return mShowBoundingBox;
    }

public void hideBoundingBox(bool bHide)
{
    mHideBoundingBox = bHide;
}

//public void updateFromParentImpl()
//    {
//        base.updateFromParentImpl();

//        ObjectMap::const_iterator i;
//        for (i = mObjectsByName.begin(); i != mObjectsByName.end(); ++i)
//        {
//            MovableObject* object = i->second;
//            object->_notifyMoved();
//        }
//    }

    public MNode createChildImpl()
{
    UtilApi.assert(mCreator != null);
    return mCreator.createSceneNode();
}

public MNode createChildImpl(string name)
{
    UtilApi.assert(mCreator != null);
    return mCreator.createSceneNode(name);
}

public MAxisAlignedBox _getWorldAABB()
    {
        return mWorldAABB;
    }

    public MSceneManager getCreator()
        {
            return mCreator;
        }

    public void removeAndDestroyChild(string name)
{
    MSceneNode pChild = (MSceneNode)(getChild(name));
    pChild.removeAndDestroyAllChildren();

    removeChild(name);
    pChild.getCreator().destroySceneNode(name);

}

public void removeAndDestroyChild(ushort index)
{
    MSceneNode pChild = (MSceneNode)(getChild(index));
    pChild.removeAndDestroyAllChildren();

    removeChild(index);
    pChild.getCreator().destroySceneNode(pChild.getName());
}

public void removeAndDestroyAllChildren()
{
    foreach (KeyValuePair<string, MNode> kv in mChildren)
    {
        MSceneNode sn = (MSceneNode)(kv.Value);
        sn.removeAndDestroyAllChildren();
        sn.getCreator().destroySceneNode(sn.getName());
    }
    mChildren.Clear();
    needUpdate();
}

public MSceneNode createChildSceneNode(MVector3 inTranslate,
        MQuaternion inRotate)
{
    return (MSceneNode)(this.createChild(inTranslate, inRotate));
}

public MSceneNode createChildSceneNode(string name, MVector3 inTranslate,
        MQuaternion inRotate)
{
    return (MSceneNode)(this.createChild(name, inTranslate, inRotate));
}


public void setFixedYawAxis(bool useFixed, MVector3 fixedAxis)
{
    mYawFixed = useFixed;
    mYawFixedAxis = fixedAxis;
}

public void yaw(MRadian angle, TransformSpace relativeTo)
{
    if (mYawFixed)
    {
        rotate(mYawFixedAxis, angle, relativeTo);
    }
    else
    {
        rotate(MVector3.UNIT_Y, angle, relativeTo);
    }

}

public void setDirection(float x, float y, float z, TransformSpace relativeTo,
        MVector3 localDirectionVector)
{
    setDirection(new MVector3(x, y, z), relativeTo, localDirectionVector);
}

public void setDirection(MVector3 vec, TransformSpace relativeTo,
        MVector3 localDirectionVector)
{
    if (vec == MVector3.ZERO) return;

    MVector3 targetDir = vec.normalisedCopy();

    switch (relativeTo)
    {
        case TransformSpace.TS_PARENT:
            if (mInheritOrientation)
            {
                if (mParent != null)
                {
                    targetDir = mParent._getDerivedOrientation() * targetDir;
                }
            }
            break;
        case TransformSpace.TS_LOCAL:
            targetDir = _getDerivedOrientation() * targetDir;
            break;
        case TransformSpace.TS_WORLD:
            break;
    }

    MQuaternion targetOrientation;
    if (mYawFixed)
    {
        MVector3 xVec = mYawFixedAxis.crossProduct(ref targetDir);
        xVec.normalise();
        MVector3 yVec = targetDir.crossProduct(ref xVec);
        yVec.normalise();
        MQuaternion unitZToTarget = new MQuaternion(ref xVec, ref yVec, ref targetDir);

        if (localDirectionVector == MVector3.NEGATIVE_UNIT_Z)
        {
            targetOrientation =
                new MQuaternion(-unitZToTarget.y, -unitZToTarget.z, unitZToTarget.w, unitZToTarget.x);
        }
        else
        {
                    MVector3 fallbackAxis = new MVector3(0, 0, 0);
            MQuaternion localToUnitZ = localDirectionVector.getRotationTo(ref MVector3.UNIT_Z, ref fallbackAxis);
            targetOrientation = unitZToTarget * localToUnitZ;
        }
    }
    else
    {
        MQuaternion currentOrient = _getDerivedOrientation();

        MVector3 currentDir = currentOrient * localDirectionVector;

        if ((currentDir + targetDir).squaredLength() < 0.00005f)
        {
            targetOrientation =
                new MQuaternion(-currentOrient.y, -currentOrient.z, currentOrient.w, currentOrient.x);
        }
        else
        {
                    MVector3 fallbackAxis = new MVector3(0, 0, 0);
            MQuaternion rotQuat = currentDir.getRotationTo(ref targetDir, ref fallbackAxis);
            targetOrientation = rotQuat * currentOrient;
        }
    }

    if (mParent != null && mInheritOrientation)
        setOrientation(mParent._getDerivedOrientation().UnitInverse() * targetOrientation);
    else
        setOrientation(targetOrientation);
}

public void lookAt(MVector3 targetPoint, TransformSpace relativeTo,
        MVector3 localDirectionVector)
{
    MVector3 origin;
    switch (relativeTo)
    {
        default:
        case TransformSpace.TS_WORLD:
            origin = _getDerivedPosition();
            break;
        case TransformSpace.TS_PARENT:
            origin = mPosition;
            break;
        case TransformSpace.TS_LOCAL:
            origin = MVector3.ZERO;
            break;
    }

    setDirection(targetPoint - origin, relativeTo, localDirectionVector);
}

public void _autoTrack()
{
    if (mAutoTrackTarget != null)
    {
        lookAt(mAutoTrackTarget._getDerivedPosition() + mAutoTrackOffset,
            TransformSpace.TS_WORLD, mAutoTrackLocalDirection);
        // update self & children
        _update(true, true);
    }
}

public MSceneNode getParentSceneNode()
    {
        return (MSceneNode)(getParent());
    }

//    public void setVisible(bool visible, bool cascade)
//{
//    ObjectMap::iterator oi, oiend;
//    oiend = mObjectsByName.end();
//    for (oi = mObjectsByName.begin(); oi != oiend; ++oi)
//    {
//        oi->second->setVisible(visible);
//    }

//    if (cascade)
//    {
//        ChildNodeMap::iterator i, iend;
//        iend = mChildren.end();
//        for (i = mChildren.begin(); i != iend; ++i)
//        {
//            static_cast<SceneNode*>(i->second)->setVisible(visible, cascade);
//        }
//    }
//}
    }
}