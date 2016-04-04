namespace SDK.Lib
{
    public class MMovableObject
    {
        public class MListener
        {
            public MListener()
            {

            }

            public virtual void objectDestroyed(MMovableObject obj)
            {

            }

            public virtual void objectAttached(MMovableObject obj)
            {

            }

            public virtual void objectDetached(MMovableObject obj)
            {

            }

            public virtual void objectMoved(MMovableObject obj)
            {

            }
        }

        protected string mName;

        protected MSceneManager mManager;

        protected MNode mParentNode;
        protected bool mParentIsTagPoint;

        protected bool mVisible;

        protected bool mDebugDisplay;

        protected float mUpperDistance;
        protected float mSquaredUpperDistance;

        protected float mMinPixelSize;

        protected bool mBeyondFarDistance;
        protected MAxisAlignedBox mWorldAABB;

        protected MAxisAlignedBox mWorldDarkCapBounds;
        protected MListener mListener;
        protected static MNameGenerator msMovableNameGenerator = new MNameGenerator("MO_");

        public MMovableObject()
        {
            mManager = null;
            mParentNode = null;
            mParentIsTagPoint = false;
            mVisible = true;
            mDebugDisplay = false;
            mUpperDistance = 0;
            mSquaredUpperDistance = 0;
            mMinPixelSize = 0;
            mBeyondFarDistance = false;

            mName = msMovableNameGenerator.generate();
        }

        public MMovableObject(string name)
        {
            mManager = null;
            mParentNode = null;
            mParentIsTagPoint = false;
            mVisible = true;
            mDebugDisplay = false;
            mUpperDistance = 0;
            mSquaredUpperDistance = 0;
            mMinPixelSize = 0;
            mBeyondFarDistance = false;

            mName = msMovableNameGenerator.generate();
        }

        public void _notifyAttached(MNode parent, bool isTagPoint = false)
        {
            UtilApi.assert(mParentNode == null || parent == null);

            bool different = (parent != mParentNode);

            mParentNode = parent;
            mParentIsTagPoint = isTagPoint;

            if (mListener != null && different)
            {
                if (mParentNode != null)
                    mListener.objectAttached(this);
                else
                    mListener.objectDetached(this);
            }
        }

        public MNode getParentNode()
        {
            return mParentNode;
        }

        public MSceneNode getParentSceneNode()
        {
            return (MSceneNode)(mParentNode);
        }

        public bool isAttached()
        {
            return (mParentNode != null);
        }

        public void detachFromParent()
        {
            if (isAttached())
            {
                MSceneNode sn = (MSceneNode)(mParentNode);
                sn.detachObject(this);
            }
        }

        public bool isInScene()
        {
            if (mParentNode != null)
            {
                MSceneNode sn = (MSceneNode)(mParentNode);
                return sn.isInSceneGraph();
            }
            else
            {
                return false;
            }
        }

        public void _notifyMoved()
        {
            if (mListener != null)
            {
                mListener.objectMoved(this);
            }
        }

        public void setVisible(bool visible)
        {
            mVisible = visible;
        }

        public bool getVisible()
        {
            return mVisible;
        }

        public bool isVisible()
        {
            if (!mVisible || mBeyondFarDistance)
                return false;

            return true;
        }

        public void _notifyCurrentCamera(MCamera cam)
        {
            if (mParentNode != null)
            {
                mBeyondFarDistance = false;

                if (cam.getUseRenderingDistance() && mUpperDistance > 0)
                {
                    float rad = this.getBoundingRadius();
                    float squaredDepth = mParentNode.getSquaredViewDepth(cam.getLodCamera());

                    MVector3 scl = mParentNode._getDerivedScale();
                    float factor = UtilMath.max(UtilMath.max(scl.x, scl.y), scl.z);

                    float maxDist = mUpperDistance + rad * factor;
                    if (squaredDepth > UtilMath.Sqr(maxDist))
                    {
                        mBeyondFarDistance = true;
                    }
                }

                if (!mBeyondFarDistance && cam.getUseMinPixelSize() && mMinPixelSize > 0)
                {
                    float pixelRatio = cam.getPixelDisplayRatio();

                    MVector3 objBound = getBoundingBox().getSize() *
                                getParentNode()._getDerivedScale();

                    objBound.x = UtilMath.Sqr(objBound.x);
                    objBound.y = UtilMath.Sqr(objBound.y);
                    objBound.z = UtilMath.Sqr(objBound.z);
                    float sqrObjMedianSize = UtilMath.max(UtilMath.max(
                                        UtilMath.min(objBound.x, objBound.y),
                                        UtilMath.min(objBound.x, objBound.z)),
                                        UtilMath.min(objBound.y, objBound.z));

                    float sqrDistance = 1;
                    if (cam.getProjectionType() == ProjectionType.PT_PERSPECTIVE)
                    {
                        sqrDistance = mParentNode.getSquaredViewDepth(cam.getLodCamera());
                    }

                    mBeyondFarDistance = sqrObjMedianSize <
                                sqrDistance * UtilMath.Sqr(pixelRatio * mMinPixelSize);
                }
            }
        }

        public MMatrix4 _getParentNodeFullTransform()
        {
            if (mParentNode != null)
            {
                return mParentNode._getFullTransform();
            }

            return MMatrix4.IDENTITY;
        }

        public MAxisAlignedBox getWorldBoundingBox(bool derive)
        {
            if (derive)
            {
                mWorldAABB = this.getBoundingBox();
                MMatrix4 tmp = _getParentNodeFullTransform();
                mWorldAABB.transformAffine(ref tmp);
            }

            return mWorldAABB;
        }

        public virtual void setListener(MListener listener)
        {
            mListener = listener;
        }

        public virtual MListener getListener()
        {
            return mListener;
        }

        public virtual void setRenderingMinPixelSize(float pixelSize)
        {
            mMinPixelSize = pixelSize;
        }

        public virtual float getRenderingMinPixelSize()
        {
            return mMinPixelSize;
        }

        public virtual void _notifyManager(MSceneManager man)
        {
            mManager = man;
        }

        public virtual MSceneManager _getManager()
        {
            return mManager;
        }

        public virtual MAxisAlignedBox getBoundingBox()
        {
            return new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
        }

        public virtual float getBoundingRadius()
        {
            return 0;
        }

        public virtual bool isParentTagPoint()
        {
            return mParentIsTagPoint;
        }

        public virtual string getName()
        {
            return mName;
        }
    }
}