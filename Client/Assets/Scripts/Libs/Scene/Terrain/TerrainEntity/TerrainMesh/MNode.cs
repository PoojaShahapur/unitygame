using System.Collections.Generic;

namespace SDK.Lib
{
    public enum TransformSpace
    {
        TS_LOCAL,
        TS_PARENT,
        TS_WORLD,
    }

    public class MNode : AuxComponent
    {
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

        protected static MList<MNode> msQueuedUpdates;
        protected MNode mParent;
        protected Dictionary<string, MNode> mChildren;

        protected HashSet<MNode> mChildrenToUpdate;
        protected bool mNeedParentUpdate;
        protected bool mNeedChildUpdate;
        protected bool mParentNotified;
        protected bool mQueuedForUpdate;

        protected string mName;
        protected MQuaternion mOrientation;
        protected MVector3 mPosition;
        protected MVector3 mScale;
        protected bool mInheritOrientation;
        protected bool mInheritScale;

        protected MQuaternion mDerivedOrientation;
        protected MVector3 mDerivedPosition;
        protected MVector3 mDerivedScale;
        protected MVector3 mInitialPosition;
        protected MQuaternion mInitialOrientation;
        protected MVector3 mInitialScale;
        protected MMatrix4 mCachedTransform;
        protected bool mCachedTransformOutOfDate;

        Listener mListener;

        public MNode()
        {
            mParent = null;
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
            mListener = null;

            mName = "";
            mChildrenToUpdate = new HashSet<MNode>();

            needUpdate();
        }

        public MNode(string name)
        {
            mParent = null;
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
            mListener = null;
            needUpdate();
        }

        public MNode getParent()
        {
            return mParent;
        }

        virtual public void setParent(MNode parent)
        {
            bool different = (parent != mParent);

            mParent = parent;
            mParentNotified = false;
            needUpdate();

            if (mListener != null && different)
            {
                if (mParent != null)
                    mListener.nodeAttached(this);
                else
                    mListener.nodeDetached(this);
            }
        }

        public MMatrix4 _getFullTransform()
        {
            if (mCachedTransformOutOfDate)
            {
                MVector3 pos = _getDerivedPosition();
                MVector3 scale = _getDerivedScale();
                MQuaternion quat = _getDerivedOrientation();
                mCachedTransform.makeTransform(
                    ref pos,
                    ref scale,
                    ref quat);
                mCachedTransformOutOfDate = false;
            }
            return mCachedTransform;
        }

        virtual public void _update(bool updateChildren, bool parentHasChanged)
        {
            mParentNotified = false;

            if (mNeedParentUpdate || parentHasChanged)
            {
                _updateFromParent();
            }

            if (updateChildren)
            {
                if (mNeedChildUpdate || parentHasChanged)
                {
                    foreach (KeyValuePair<string, MNode> kv in mChildren)
                    {
                        MNode child = kv.Value;
                        child._update(true, true);
                    }
                }
                else
                {
                    foreach (MNode elem in mChildrenToUpdate)
                    {
                        MNode child = elem;
                        child._update(true, false);
                    }

                }

                mChildrenToUpdate.Clear();
                mNeedChildUpdate = false;
            }
        }

        public void _updateFromParent()
        {
            updateFromParentImpl();

            if (mListener != null)
            {
                mListener.nodeUpdated(this);
            }
        }

        virtual public MNode createChildImpl()
        {
            return null;
        }

        virtual public MNode createChildImpl(string name)
        {
            return null;
        }

        virtual public void updateFromParentImpl()
        {
            mCachedTransformOutOfDate = true;

            if (mParent != null)
            {
                MQuaternion parentOrientation = mParent._getDerivedOrientation();
                if (mInheritOrientation)
                {
                    mDerivedOrientation = parentOrientation * mOrientation;
                }
                else
                {
                    mDerivedOrientation = mOrientation;
                }

                MVector3 parentScale = mParent._getDerivedScale();
                if (mInheritScale)
                {
                    mDerivedScale = parentScale * mScale;
                }
                else
                {
                    mDerivedScale = mScale;
                }

                mDerivedPosition = parentOrientation * (parentScale * mPosition);
                mDerivedPosition += mParent._getDerivedPosition();
            }
            else
            {
                mDerivedOrientation = mOrientation;
                mDerivedPosition = mPosition;
                mDerivedScale = mScale;
            }

            mNeedParentUpdate = false;
        }

        public MNode createChild(MVector3 inTranslate, MQuaternion inRotate)
        {
            MNode newNode = createChildImpl();
            newNode.translate(inTranslate);
            newNode.rotate(inRotate);
            this.addChild(newNode);

            return newNode;
        }

        public MNode createChild(string name, MVector3 inTranslate, MQuaternion inRotate)
        {
            MNode newNode = createChildImpl(name);
            newNode.translate(inTranslate);
            newNode.rotate(inRotate);
            this.addChild(newNode);

            return newNode;
        }

        public void addChild(MNode child)
        {
            if (child.mParent != null)
            {
                // error
            }

            mChildren.Add(child.getName(), child);
            child.setParent(this);
        }

        public ushort numChildren()
        {
            return (ushort)(mChildren.Count);
        }

        public MNode getChild(ushort index)
        {
            if (index < mChildren.Count)
            {
                Dictionary<string, MNode>.Enumerator iter = mChildren.GetEnumerator();
                while (index-- != 0) iter.MoveNext();
                return iter.Current.Value;
            }
            else
                return null;
        }

        virtual public MNode removeChild(ushort index)
        {
            if (index < mChildren.Count)
            {
                Dictionary<string, MNode>.Enumerator iter = mChildren.GetEnumerator();
                while (index-- != 0) iter.MoveNext();
                MNode ret = iter.Current.Value;
                cancelUpdate(ret);

                mChildren.Remove(iter.Current.Key);
                ret.setParent(null);
                return ret;
            }
            else
            {
                // Error
            }
            return null;
        }

        virtual public MNode removeChild(MNode child)
        {
            if (child != null)
            {
                MNode i = null;
                mChildren.TryGetValue(child.getName(), out i);
                if (mChildren.ContainsKey(child.getName()) && i == child)
                {
                    cancelUpdate(child);

                    mChildren.Remove(child.getName());
                    child.setParent(null);
                }
            }
            return child;
        }

        public MQuaternion getOrientation()
        {
            return mOrientation;
        }

        public void setOrientation(MQuaternion q)
        {
            UtilApi.assert(!q.isNaN(), "Invalid orientation supplied as parameter");
            mOrientation = q;
            mOrientation.normalise();
            needUpdate();
        }

        public void setOrientation(float w, float x, float y, float z)
        {
            setOrientation(new MQuaternion(w, x, y, z));
        }

        public void resetOrientation()
        {
            mOrientation = MQuaternion.IDENTITY;
            needUpdate();
        }

        public void setPosition(MVector3 pos)
        {
            UtilApi.assert(!pos.isNaN(), "Invalid vector supplied as parameter");
            mPosition = pos;
            needUpdate();
        }

        public void setPosition(float x, float y, float z)
        {
            MVector3 v = new MVector3(x, y, z);
            setPosition(v);
        }

        public MVector3 getPosition()
        {
            return mPosition;
        }

        public MMatrix3 getLocalAxes()
        {
            MVector3 axisX = MVector3.UNIT_X;
            MVector3 axisY = MVector3.UNIT_Y;
            MVector3 axisZ = MVector3.UNIT_Z;

            axisX = mOrientation * axisX;
            axisY = mOrientation * axisY;
            axisZ = mOrientation * axisZ;

            return new MMatrix3(axisX.x, axisY.x, axisZ.x,
                           axisX.y, axisY.y, axisZ.y,
                           axisX.z, axisY.z, axisZ.z);
        }

        public void translate(MVector3 d, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            switch (relativeTo)
            {
                case TransformSpace.TS_LOCAL:
                    mPosition += mOrientation * d;
                    break;
                case TransformSpace.TS_WORLD:
                    if (mParent != null)
                    {
                        mPosition += mParent.convertWorldToLocalDirection(d, true);
                    }
                    else
                    {
                        mPosition += d;
                    }
                    break;
                case TransformSpace.TS_PARENT:
                    mPosition += d;
                    break;
            }
            needUpdate();

        }

        public void translate(float x, float y, float z, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            MVector3 v = new MVector3(x, y, z);
            translate(v, relativeTo);
        }

        public void translate(MMatrix3 axes, MVector3 move, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            MVector3 derived = axes * move;
            translate(derived, relativeTo);
        }

        public void translate(MMatrix3 axes, float x, float y, float z, TransformSpace relativeTo)
        {
            MVector3 d = new MVector3(x, y, z);
            translate(axes, d, relativeTo);
        }

        public void roll(MRadian angle, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            rotate(MVector3.UNIT_Z, angle, relativeTo);
        }

        public void pitch(MRadian angle, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            rotate(MVector3.UNIT_X, angle, relativeTo);
        }

        public void yaw(MRadian angle, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            rotate(MVector3.UNIT_Y, angle, relativeTo);

        }

        public void rotate(MVector3 axis, MRadian angle, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            MQuaternion q = new MQuaternion(1);
            q.FromAngleAxis(angle, ref axis);
            rotate(q, relativeTo);
        }

        public void rotate(MQuaternion q, TransformSpace relativeTo = TransformSpace.TS_PARENT)
        {
            switch (relativeTo)
            {
                case TransformSpace.TS_PARENT:
                    mOrientation = q * mOrientation;
                    break;
                case TransformSpace.TS_WORLD:
                    mOrientation = mOrientation * _getDerivedOrientation().Inverse()
                        * q * _getDerivedOrientation();
                    break;
                case TransformSpace.TS_LOCAL:
                    mOrientation = mOrientation * q;
                    break;
            }

            mOrientation.normalise();

            needUpdate();
        }

        public void _setDerivedPosition(MVector3 pos)
        {
            if (mParent != null)
                setPosition(mParent.convertWorldToLocalPosition(pos));
        }

        public void _setDerivedOrientation(MQuaternion q)
        {
            if (mParent != null)
                setOrientation(mParent.convertWorldToLocalOrientation(q));
        }

        public MQuaternion _getDerivedOrientation()
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return mDerivedOrientation;
        }

        public MVector3 _getDerivedPosition()
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return mDerivedPosition;
        }

        public MVector3 _getDerivedScale()
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return mDerivedScale;
        }

        public MVector3 convertWorldToLocalPosition(MVector3 worldPos)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }

            return mDerivedOrientation.Inverse() * (worldPos - mDerivedPosition) / mDerivedScale;
        }

        public MVector3 convertLocalToWorldPosition(MVector3 localPos)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return _getFullTransform().transformAffine(localPos);
        }

        public MVector3 convertWorldToLocalDirection(MVector3 worldDir, bool useScale)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }

            return useScale ?
                    mDerivedOrientation.Inverse() * worldDir / mDerivedScale :
                mDerivedOrientation.Inverse() * worldDir;
        }

        public MVector3 convertLocalToWorldDirection(MVector3 localDir, bool useScale)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return useScale ?
                _getFullTransform().transformDirectionAffine(localDir) :
                mDerivedOrientation * localDir;
        }

        public MQuaternion convertWorldToLocalOrientation(MQuaternion worldOrientation)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return mDerivedOrientation.Inverse() * worldOrientation;
        }

        public MQuaternion convertLocalToWorldOrientation(MQuaternion localOrientation)
        {
            if (mNeedParentUpdate)
            {
                _updateFromParent();
            }
            return mDerivedOrientation * localOrientation;
        }

        virtual public void removeAllChildren()
        {
            foreach (KeyValuePair<string, MNode> i in mChildren)
            {
                i.Value.setParent(null);
            }
            mChildren.Clear();
            mChildrenToUpdate.Clear();
        }

        public void setScale(MVector3 inScale)
        {
            UtilApi.assert(!inScale.isNaN(), "Invalid vector supplied as parameter");
            mScale = inScale;
            needUpdate();
        }

        public void setScale(float x, float y, float z)
        {
            setScale(new MVector3(x, y, z));
        }

        public MVector3 getScale()
        {
            return mScale;
        }

        public void setInheritOrientation(bool inherit)
        {
            mInheritOrientation = inherit;
            needUpdate();
        }

        public bool getInheritOrientation()
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

        public void scale(float x, float y, float z)
        {
            mScale.x *= x;
            mScale.y *= y;
            mScale.z *= z;
            needUpdate();
        }

        public string getName()
        {
            return mName;
        }

        public void setInitialState()
        {
            mInitialPosition = mPosition;
            mInitialOrientation = mOrientation;
            mInitialScale = mScale;
        }

        public void resetToInitialState()
        {
            mPosition = mInitialPosition;
            mOrientation = mInitialOrientation;
            mScale = mInitialScale;

            needUpdate();
        }

        public MVector3 getInitialPosition()
        {
            return mInitialPosition;
        }

        public MQuaternion getInitialOrientation()
        {
            return mInitialOrientation;

        }

        public MVector3 getInitialScale()
        {
            return mInitialScale;
        }

        public MNode getChild(string name)
        {
            if (!mChildren.ContainsKey(name))
            {
                // error
            }
            return mChildren[name];
        }

        virtual public MNode removeChild(string name)
        {
            if (!mChildren.ContainsKey(name))
            {
                // error
            }

            MNode ret = mChildren[name];
            cancelUpdate(ret);

            mChildren.Remove(name);
            ret.setParent(null);

            return ret;
        }

        public float getSquaredViewDepth(MCamera cam)
        {
            MVector3 diff = _getDerivedPosition() - cam.getDerivedPosition();

            return diff.squaredLength();
        }

        public void needUpdate(bool forceParentUpdate = false)
        {
            mNeedParentUpdate = true;
            mNeedChildUpdate = true;
            mCachedTransformOutOfDate = true;

            if (mParent != null && (!mParentNotified || forceParentUpdate))
            {
                mParent.requestUpdate(this, forceParentUpdate);
                mParentNotified = true;
            }

            mChildrenToUpdate.Clear();
        }

        public void requestUpdate(MNode child, bool forceParentUpdate)
        {
            if (mNeedChildUpdate)
            {
                return;
            }

            // TODO:
            mChildrenToUpdate.Add(child);
            if (mParent != null && (!mParentNotified || forceParentUpdate))
            {
                mParent.requestUpdate(this, forceParentUpdate);
                mParentNotified = true;
            }
        }

        static public void queueNeedUpdate(MNode n)
        {
            if (!n.mQueuedForUpdate)
            {
                n.mQueuedForUpdate = true;
                msQueuedUpdates.Add(n);
            }
        }

        static public void processQueuedUpdates()
        {
            foreach (MNode node in msQueuedUpdates.list())
            {
                MNode n = node;
                n.mQueuedForUpdate = false;
                n.needUpdate(true);
            }
            msQueuedUpdates.Clear();
        }

        public void cancelUpdate(MNode child)
        {
            mChildrenToUpdate.Remove(child);

            if (mChildrenToUpdate.Count == 0 && mParent != null && !mNeedChildUpdate)
            {
                mParent.cancelUpdate(this);
                mParentNotified = false;
            }
        }
    }
}