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

        public void attachObject(MMovableObject obj)
        {
            if (obj.isAttached())
            {
                // Error
            }

            obj._notifyAttached(this);

            if (!mObjectsByName.ContainsKey(obj.getName()))
            {
                mObjectsByName.Add(obj.getName(), obj);
            }
            else
            {
                // Error
            }

            needUpdate();
        }

        public ushort numAttachedObjects()
        {
        return (ushort)(mObjectsByName.Count);
    }

    public MMovableObject getAttachedObject(ushort index)
    {
        if (index < mObjectsByName.Count)
        {
                Dictionary<string, MMovableObject>.Enumerator iter = mObjectsByName.GetEnumerator();
            while (index-- > 0) iter.MoveNext();

            return iter.Current.Value;
        }
        else
        {
                // Error
                return null;
        }
    }

    public MMovableObject getAttachedObject(string name)
    {
        if (!mObjectsByName.ContainsKey(name))
        {
            // Error
        }

        return mObjectsByName[name];

    }

    public MMovableObject detachObject(ushort index)
    {
        if (index < mObjectsByName.Count)
        {
                Dictionary<string, MMovableObject>.Enumerator iter = mObjectsByName.GetEnumerator();
            while (index-- > 0) iter.MoveNext();

            MMovableObject ret = iter.Current.Value;
            mObjectsByName.Remove(iter.Current.Key);
            ret._notifyAttached(null);

            needUpdate();

            return ret;
        }
        else
        {
                // Error
                return null;
        }
    }

    public MMovableObject detachObject(string name)
    {
        if (!mObjectsByName.ContainsKey(name))
        {
            // Error
        }
        MMovableObject ret = mObjectsByName[name];
        mObjectsByName.Remove(name);
        ret._notifyAttached((MSceneNode)null);

        needUpdate();

        return ret;
    }

    public void detachObject(MMovableObject obj)
    {
        foreach (KeyValuePair<string, MMovableObject> kv in mObjectsByName)
        {
            if (kv.Value == obj)
            {
                mObjectsByName.Remove(kv.Key);
                break;
            }
        }
        obj._notifyAttached((MSceneNode)null);

        needUpdate();
    }

    public void detachAllObjects()
    {
        foreach (KeyValuePair<string, MMovableObject> kv in mObjectsByName)
        {
            MMovableObject ret = kv.Value;
            ret._notifyAttached((MSceneNode)null);
        }
        mObjectsByName.Clear();
        needUpdate();
    }

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

        override public void updateFromParentImpl()
        {
            base.updateFromParentImpl();

            foreach (KeyValuePair<string, MMovableObject> kv in mObjectsByName)
            {
                MMovableObject obj = kv.Value;
                obj._notifyMoved();
            }
        }

        override public MNode createChildImpl()
{
    UtilApi.assert(mCreator != null);
    return mCreator.createSceneNode();
}

override public MNode createChildImpl(string name)
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
        _update(true, true);
    }
}

public MSceneNode getParentSceneNode()
    {
        return (MSceneNode)(getParent());
    }

        public void setVisible(bool visible, bool cascade)
        {
            foreach (KeyValuePair<string, MMovableObject> kv in mObjectsByName)
            {
                kv.Value.setVisible(visible);
            }

            if (cascade)
            {
                foreach (KeyValuePair<string, MNode> kv in mChildren)
                {
                    ((MSceneNode)kv.Value).setVisible(visible, cascade);
                }
            }
        }
    }
}