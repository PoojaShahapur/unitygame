using UnityEngine;

namespace SDK.Lib
{
    public class MCamera : MFrustum
    {
        protected MQuaternion mOrientation;
        protected MVector3 mPosition;

        protected MQuaternion mDerivedOrientation;
        protected MVector3 mDerivedPosition;
        protected MVector3 mDerivedScale;

        protected MQuaternion mRealOrientation;
        protected MVector3 mRealPosition;

        protected bool mYawFixed;
        protected MVector3 mYawFixedAxis;

        protected static string msMovableType = "Camera";

        protected float mWLeft, mWTop, mWRight, mWBottom;
        protected bool mWindowSet;
        protected MList<MPlane> mWindowClipPlanes;
        protected bool mRecalcWindow;
        MFrustum mCullFrustum;
        protected bool mAutoAspectRatio;
        protected Transform mParentNode;

        public MCamera(Transform parentNode)
        {
            mParentNode = parentNode;
            mOrientation = MQuaternion.IDENTITY;
            mPosition = MVector3.ZERO;
            mWindowSet = false;

            mFOVy = (float)(UtilMath.PI / 4.0f);
            mNearDist = 100.0f;
            mFarDist = 100000.0f;
            mAspect = 1.33333333333333f;
            mProjType = ProjectionType.PT_PERSPECTIVE;
            MVector3 dir = new MVector3(0, 0, 0);
            setFixedYawAxis(true, ref dir);

            invalidateFrustum();
            invalidateView();

            mViewMatrix = MMatrix4.ZERO;
            mProjMatrixRS = MMatrix4.ZERO;

            mReflect = false;
        }

        public void setPosition(float x, float y, float z)
        {
            mPosition.x = x;
            mPosition.y = y;
            mPosition.z = z;
            invalidateView();
        }

        public void setPosition(ref MVector3 vec)
        {
            mPosition = vec;
            invalidateView();
        }

        public MVector3 getPosition()
        {
            return mPosition;
        }

        public void move(ref MVector3 vec)
        {
            mPosition = mPosition + vec;
            invalidateView();
        }

        public void moveRelative(ref MVector3 vec)
        {
            MVector3 trans = mOrientation * vec;

            mPosition = mPosition + trans;
            invalidateView();
        }

        public void setDirection(float x, float y, float z)
        {
            MVector3 dir = new MVector3(x, y, z);
            setDirection(ref dir);
        }

        public void setDirection(ref MVector3 vec)
        {
            if (vec == MVector3.ZERO) return;

            MVector3 zAdjustVec = -vec;
            zAdjustVec.normalise();

            MQuaternion targetWorldOrientation = new MQuaternion(1);

            if (mYawFixed)
            {
                MVector3 xVec = mYawFixedAxis.crossProduct(ref zAdjustVec);
                xVec.normalise();

                MVector3 yVec = zAdjustVec.crossProduct(ref xVec);
                yVec.normalise();

                targetWorldOrientation.FromAxes(xVec, yVec, zAdjustVec);
            }
            else
            {

                MVector3[] axes = new MVector3[3];
                updateView();
                mRealOrientation.ToAxes(ref axes);
                MQuaternion rotQuat = new MQuaternion(1);
                if ((axes[2] + zAdjustVec).squaredLength() < 0.00005f)
                {
                    rotQuat.FromAngleAxis(new MRadian(UtilMath.PI), ref axes[1]);
                }
                else
                {
                    MVector3 tmp = new MVector3(0, 0, 0);
                    rotQuat = axes[2].getRotationTo(ref zAdjustVec, ref tmp);
                }
                targetWorldOrientation = rotQuat * mRealOrientation;
            }

            mOrientation = MQuaternion.fromNative(Quaternion.Inverse(mParentNode.rotation) * targetWorldOrientation.toNative());

            invalidateView();
        }

        public MVector3 getDirection()
        {
            return mOrientation * -MVector3.UNIT_Z;
        }

        public MVector3 getUp()
        {
            return mOrientation * MVector3.UNIT_Y;
        }

        public MVector3 getRight()
        {
            return mOrientation * MVector3.UNIT_X;
        }

        public void lookAt(ref MVector3 targetPoint)
        {
            updateView();
            MVector3 dir = targetPoint - mRealPosition;
            this.setDirection(ref dir);
        }

        public void lookAt(float x, float y, float z)
        {
            MVector3 vTemp = new MVector3(x, y, z);
            this.lookAt(ref vTemp);
        }

        public void roll(ref MRadian angle)
        {
            MVector3 zAxis = mOrientation.zAxis();
            rotate(ref zAxis, ref angle);

            invalidateView();
        }

        public void yaw(ref MRadian angle)
        {
            MVector3 yAxis;

            if (mYawFixed)
            {
                yAxis = mYawFixedAxis;
            }
            else
            {
                yAxis = mOrientation.yAxis();
            }

            rotate(ref yAxis, ref angle);

            invalidateView();
        }

        public void pitch(ref MRadian angle)
        {
            MVector3 xAxis = mOrientation.xAxis();
            rotate(ref xAxis, ref angle);

            invalidateView();
        }

        public void rotate(ref MVector3 axis, ref MRadian angle)
        {
            MQuaternion q = new MQuaternion(1);
            q.FromAngleAxis(angle, ref axis);
            rotate(ref q);
        }

        public void rotate(ref MQuaternion q)
        {
            MQuaternion qnorm = q;
            qnorm.normalise();
            mOrientation = qnorm * mOrientation;

            invalidateView();
        }

        override public bool isViewOutOfDate()
        {
            MQuaternion derivedOrient = MQuaternion.fromNative(mParentNode.rotation);
            MVector3 derivedPos = MVector3.fromNative(mParentNode.position);

            if (mRecalcView ||
                derivedOrient != mLastParentOrientation ||
                derivedPos != mLastParentPosition)
            {
                mLastParentOrientation = derivedOrient;
                mLastParentPosition = derivedPos;
                mRealOrientation = mLastParentOrientation * mOrientation;
                mRealPosition = (mLastParentOrientation * mPosition) + mLastParentPosition;
                mRecalcView = true;
                mRecalcWindow = true;
            }

            if (mReflect && mReflectPlane != null &&
                !(mLastLinkedReflectionPlane == mReflectPlane))
            {
                mReflectMatrix = UtilMath.buildReflectionMatrix(ref mReflectPlane);
                mLastLinkedReflectionPlane = mReflectPlane;
                mRecalcView = true;
                mRecalcWindow = true;
            }

            if (mRecalcView)
            {
                if (mReflect)
                {
                    MVector3 dir = mRealOrientation * MVector3.NEGATIVE_UNIT_Z;
                    MVector3 rdir = dir.reflect(ref mReflectPlane.normal);
                    MVector3 up = mRealOrientation * MVector3.UNIT_Y;
                    mDerivedOrientation = dir.getRotationTo(ref rdir, ref up) * mRealOrientation;

                    mDerivedPosition = mReflectMatrix.transformAffine(mRealPosition);
                }
                else
                {
                    mDerivedOrientation = mRealOrientation;
                    mDerivedPosition = mRealPosition;
                }
            }

            return mRecalcView;
        }

        override public void invalidateView()
        {
            mRecalcWindow = true;
            base.invalidateView();
        }

        override public void invalidateFrustum()
        {
            mRecalcWindow = true;
            base.invalidateFrustum();
        }

        public string ToString(ref MCamera c)
        {
            string o = "";
            o += ("Camera(" + " pos=" + c.mPosition);
            MVector3 dir = new MVector3(c.mOrientation * new MVector3(0, 0, -1));
            o += (", direction=" + dir + ",near=" + c.mNearDist);
            o += (", far=" + c.mFarDist + ", FOVy=" + c.mFOVy);
            o += (", aspect=" + c.mAspect + ", ");
            o += (", xoffset=" + c.mFrustumOffset.x + ", yoffset=" + c.mFrustumOffset.y);
            o += (", focalLength=" + c.mFocalLength + ", ");
            o += ("NearFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR] + ", ");
            o += ("FarFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR] + ", ");
            o += ("LeftFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT] + ", ");
            o += ("RightFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT] + ", ");
            o += ("TopFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP] + ", ");
            o += ("BottomFrustumPlane=" + c.mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM]);
            o += (")");

            return o;
        }

        public void setFixedYawAxis(bool useFixed, ref MVector3 fixedAxis)
        {
            mYawFixed = useFixed;
            mYawFixedAxis = fixedAxis;
        }

        public MQuaternion getOrientation()
        {
            return mOrientation;
        }

        public void setOrientation(ref MQuaternion q)
        {
            mOrientation = q;
            mOrientation.normalise();
            invalidateView();
        }

        public MQuaternion getDerivedOrientation()
        {
            updateView();
            return mDerivedOrientation;
        }

        public MVector3 getDerivedPosition()
        {
            updateView();
            return mDerivedPosition;
        }

        public MVector3 getDerivedDirection()
        {
            updateView();
            return mDerivedOrientation * MVector3.NEGATIVE_UNIT_Z;
        }

        public MVector3 getDerivedUp()
        {
            updateView();
            return mDerivedOrientation * MVector3.UNIT_Y;
        }

        public MVector3 getDerivedRight()
        {
            updateView();
            return mDerivedOrientation * MVector3.UNIT_X;
        }

        public MQuaternion getRealOrientation()
        {
            updateView();
            return mRealOrientation;
        }

        public MVector3 getRealPosition()
        {
            updateView();
            return mRealPosition;
        }

        public MVector3 getRealDirection()
        {
            updateView();
            return mRealOrientation * MVector3.NEGATIVE_UNIT_Z;
        }

        public MVector3 getRealUp()
        {
            updateView();
            return mRealOrientation * MVector3.UNIT_Y;
        }

        public MVector3 getRealRight()
        {
            updateView();
            return mRealOrientation * MVector3.UNIT_X;
        }

        public void getWorldTransforms(ref MMatrix4 mat)
        {
            updateView();

            MVector3 scale = MVector3.fromNative(mParentNode.localScale);
            mat.makeTransform(
                    ref mDerivedPosition,
                    ref scale,
                    ref mDerivedOrientation);
        }

        override public string getMovableType()
        {
            return msMovableType;
        }

        public MRay getCameraToViewportRay(float screenX, float screenY)
        {
            MRay ret = new MRay();
            getCameraToViewportRay(screenX, screenY, ref ret);
            return ret;
        }

        public void getCameraToViewportRay(float screenX, float screenY, ref MRay outRay)
        {
            MMatrix4 inverseVP = (getProjectionMatrix() * getViewMatrix(true)).inverse();

            float nx = (2.0f * screenX) - 1.0f;
            float ny = 1.0f - (2.0f * screenY);
            MVector3 nearPoint = new MVector3(nx, ny, -1.0f);
            MVector3 midPoint = new MVector3(nx, ny, 0.0f);

            MVector3 rayOrigin, rayTarget;

            rayOrigin = inverseVP * nearPoint;
            rayTarget = inverseVP * midPoint;

            MVector3 rayDirection = rayTarget - rayOrigin;
            rayDirection.normalise();

            outRay.setOrigin(ref rayOrigin);
            outRay.setDirection(ref rayDirection);
        }

        public void setWindow(float Left, float Top, float Right, float Bottom)
        {
            mWLeft = Left;
            mWTop = Top;
            mWRight = Right;
            mWBottom = Bottom;

            mWindowSet = true;
            mRecalcWindow = true;
        }

        public void resetWindow()
        {
            mWindowSet = false;
        }

        public void setWindowImpl()
        {
            if (!mWindowSet || !mRecalcWindow)
                return;

            float vpLeft = 0, vpRight = 0, vpBottom = 0, vpTop = 0;
            calcProjectionParameters(ref vpLeft, ref vpRight, ref vpBottom, ref vpTop);

            float vpWidth = vpRight - vpLeft;
            float vpHeight = vpTop - vpBottom;

            float wvpLeft = vpLeft + mWLeft * vpWidth;
            float wvpRight = vpLeft + mWRight * vpWidth;
            float wvpTop = vpTop - mWTop * vpHeight;
            float wvpBottom = vpTop - mWBottom * vpHeight;

            MVector3 vp_ul = new MVector3(wvpLeft, wvpTop, -mNearDist);
            MVector3 vp_ur = new MVector3(wvpRight, wvpTop, -mNearDist);
            MVector3 vp_bl = new MVector3(wvpLeft, wvpBottom, -mNearDist);
            MVector3 vp_br = new MVector3(wvpRight, wvpBottom, -mNearDist);

            MMatrix4 inv = mViewMatrix.inverseAffine();

            MVector3 vw_ul = inv.transformAffine(vp_ul);
            MVector3 vw_ur = inv.transformAffine(vp_ur);
            MVector3 vw_bl = inv.transformAffine(vp_bl);
            MVector3 vw_br = inv.transformAffine(vp_br);

            mWindowClipPlanes.Clear();
            if (mProjType == ProjectionType.PT_PERSPECTIVE)
            {
                MVector3 position = getPositionForViewUpdate();
                mWindowClipPlanes.Add(new MPlane(ref position, ref vw_bl, ref vw_ul));
                mWindowClipPlanes.Add(new MPlane(ref position, ref vw_ul, ref vw_ur));
                mWindowClipPlanes.Add(new MPlane(ref position, ref vw_ur, ref vw_br));
                mWindowClipPlanes.Add(new MPlane(ref position, ref vw_br, ref vw_bl));
            }
            else
            {
                MVector3 x_axis = new MVector3(inv[0, 0], inv[0, 1], inv[0, 2]);
                MVector3 y_axis = new MVector3(inv[1, 0], inv[1, 1], inv[1, 2]);
                x_axis.normalise();
                y_axis.normalise();
                mWindowClipPlanes.Add(new MPlane(ref x_axis, ref vw_bl));
                MVector3 tmp = -x_axis;
                mWindowClipPlanes.Add(new MPlane(ref tmp, ref vw_ur));
                mWindowClipPlanes.Add(new MPlane(ref y_axis, ref vw_bl));
                tmp = -y_axis;
                mWindowClipPlanes.Add(new MPlane(ref tmp, ref vw_ur));
            }

            mRecalcWindow = false;
        }

        public MList<MPlane> getWindowPlanes()
        {
            updateView();
            setWindowImpl();
            return mWindowClipPlanes;
        }

        override public MVector3 getPositionForViewUpdate()
        {
            return mRealPosition;
        }

        override public MQuaternion getOrientationForViewUpdate()
        {
            return mRealOrientation;
        }

        public bool getAutoAspectRatio()
        {
            return mAutoAspectRatio;
        }

        public void setAutoAspectRatio(bool autoratio)
        {
            mAutoAspectRatio = autoratio;
        }

        override public bool isVisible(ref MAxisAlignedBox bound, ref FrustumPlane culledBy)
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.isVisible(ref bound, ref culledBy);
            }
            else
            {
                return base.isVisible(ref bound, ref culledBy);
            }
        }

        override public bool isVisible(ref MVector3 vert, ref FrustumPlane culledBy)
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.isVisible(ref vert, ref culledBy);
            }
            else
            {
                return base.isVisible(ref vert, ref culledBy);
            }
        }

        override public MVector3[] getWorldSpaceCorners()
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.getWorldSpaceCorners();
            }
            else
            {
                return base.getWorldSpaceCorners();
            }
        }

        override public MPlane getFrustumPlane(short plane)
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.getFrustumPlane(plane);
            }
            else
            {
                return base.getFrustumPlane(plane);
            }
        }

        override public float getNearClipDistance()
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.getNearClipDistance();
            }
            else
            {
                return base.getNearClipDistance();
            }
        }

        override public float getFarClipDistance()
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.getFarClipDistance();
            }
            else
            {
                return base.getFarClipDistance();
            }
        }

        override public MMatrix4 getViewMatrix()
        {
            if (mCullFrustum != null)
            {
                return mCullFrustum.getViewMatrix();
            }
            else
            {
                return base.getViewMatrix();
            }
        }

        public MMatrix4 getViewMatrix(bool ownFrustumOnly)
        {
            if (ownFrustumOnly)
            {
                return base.getViewMatrix();
            }
            else
            {
                return getViewMatrix();
            }
        }
    }
}