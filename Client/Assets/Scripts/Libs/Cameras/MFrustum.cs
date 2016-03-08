namespace SDK.Lib
{
    public enum OrientationMode
    {
        OR_DEGREE_0 = 0,
        OR_DEGREE_90 = 1,
        OR_DEGREE_180 = 2,
        OR_DEGREE_270 = 3,

        OR_PORTRAIT = OR_DEGREE_0,
        OR_LANDSCAPERIGHT = OR_DEGREE_90,
        OR_LANDSCAPELEFT = OR_DEGREE_270
    };

    public enum ProjectionType
    {
        PT_ORTHOGRAPHIC,
        PT_PERSPECTIVE
    };

    public enum FrustumPlane
    {
        FRUSTUM_PLANE_NEAR = 0,
        FRUSTUM_PLANE_FAR = 1,
        FRUSTUM_PLANE_LEFT = 2,
        FRUSTUM_PLANE_RIGHT = 3,
        FRUSTUM_PLANE_TOP = 4,
        FRUSTUM_PLANE_BOTTOM = 5
    }

    public class MFrustum
        {
        protected ProjectionType mProjType;
    protected float mFOVy;
    protected float mFarDist;
    protected float mNearDist;
    protected float mAspect;
    protected float mOrthoHeight;
    protected MVector2 mFrustumOffset;
    protected float mFocalLength;
    protected MPlane[] mFrustumPlanes;

    protected MQuaternion mLastParentOrientation;
    protected MVector3 mLastParentPosition;

    protected MMatrix4 mProjMatrixRS;
    protected MMatrix4 mProjMatrixRSDepth;
    protected MMatrix4 mProjMatrix;
    protected MMatrix4 mViewMatrix;
    protected bool mRecalcFrustum;
    protected bool mRecalcView;
    protected bool mRecalcFrustumPlanes;
    protected bool mRecalcWorldSpaceCorners;
    protected bool mRecalcVertexData;
    bool mCustomViewMatrix;
    bool mCustomProjMatrix;
    bool mFrustumExtentsManuallySet;
    protected float mLeft, mRight, mTop, mBottom;
    protected OrientationMode mOrientationMode;

    protected MAxisAlignedBox mBoundingBox;
    protected MVector3[] mWorldSpaceCorners;

    protected bool mReflect;
    protected MMatrix4 mReflectMatrix;
    protected MPlane mReflectPlane;
    protected MPlane mLastLinkedReflectionPlane;

    protected bool mObliqueDepthProjection;
    protected MPlane mObliqueProjPlane;
    protected MPlane mLastLinkedObliqueProjPlane;

    protected string msMovableType = "Frustum";
    protected float INFINITE_FAR_PLANE_ADJUST = 0.00001f;
    
    public MFrustum()
    {
        mProjType = ProjectionType.PT_PERSPECTIVE;
        mFOVy = (float)(UtilApi.PI / 4.0f);
        mFarDist = 100000.0f;
        mNearDist = 100.0f;
        mAspect = 1.33333333333333f;
        mOrthoHeight = 1000;
        mFrustumOffset = MVector2.ZERO;
        mFocalLength = 1.0f;
        mLastParentOrientation = MQuaternion.IDENTITY;
        mLastParentPosition = MVector3.ZERO;
        mRecalcFrustum = true;
        mRecalcView = true;
        mRecalcFrustumPlanes = true;
        mRecalcWorldSpaceCorners = true;
        mRecalcVertexData = true;
        mCustomViewMatrix = false;
        mCustomProjMatrix = false;
        mFrustumExtentsManuallySet = false;
        mOrientationMode = OrientationMode.OR_DEGREE_0;
        mReflect = false;
        mObliqueDepthProjection = false;

        mLastLinkedReflectionPlane.normal = MVector3.ZERO;
        mLastLinkedObliqueProjPlane.normal = MVector3.ZERO;

        updateView();
        updateFrustum();
    }

        public void setFOVy(float fov)
    {
        mFOVy = fov;
        invalidateFrustum();
    }

        public float getFOVy()
    {
        return mFOVy;
    }

    public void setFarClipDistance(float farPlane)
    {
        mFarDist = farPlane;
        invalidateFrustum();
    }

    public float getFarClipDistance()
    {
        return mFarDist;
    }

public void setNearClipDistance(float nearPlane)
    {
            UtilApi.assert(nearPlane > 0, "Near clip distance must be greater than zero.");
        mNearDist = nearPlane;
        invalidateFrustum();
    }

public float getNearClipDistance()
    {
        return mNearDist;
    }

    public void setFrustumOffset(ref MVector2 offset)
    {
        mFrustumOffset = offset;
        invalidateFrustum();
    }

public void setFrustumOffset(float horizontal, float vertical)
    {
            MVector2 offsetVec = new MVector2(horizontal, vertical);
        setFrustumOffset(ref offsetVec);
    }

public MVector2 getFrustumOffset()
    {
        return mFrustumOffset;
    }

    public void setFocalLength(float focalLength)
    {
            UtilApi.assert(focalLength > 0, "Focal length must be greater than zero.");

        mFocalLength = focalLength;
        invalidateFrustum();
    }

public float getFocalLength()
    {
        return mFocalLength;
    }

    public MMatrix4 getProjectionMatrix()
    {

        updateFrustum();

        return mProjMatrix;
    }

    public MMatrix4 getProjectionMatrixWithRSDepth()
    {

        updateFrustum();

        return mProjMatrixRSDepth;
    }

    public MMatrix4 getProjectionMatrixRS()
    {

        updateFrustum();

        return mProjMatrixRS;
    }

    public MMatrix4 getViewMatrix()
    {
        updateView();

        return mViewMatrix;

    }

    public MPlane[] getFrustumPlanes()
    {
        updateFrustumPlanes();

        return mFrustumPlanes;
    }

    public MPlane getFrustumPlane(short plane)
    {
        updateFrustumPlanes();

        return mFrustumPlanes[plane];

    }

    public bool isVisible(ref MAxisAlignedBox bound, ref FrustumPlane culledBy)
    {
        if (bound.isNull()) return false;

        if (bound.isInfinite()) return true;

        updateFrustumPlanes();

        MVector3 centre = bound.getCenter();
        MVector3 halfSize = bound.getHalfSize();

        for (int plane = 0; plane < 6; ++plane)
        {
            if (plane == (int)FrustumPlane.FRUSTUM_PLANE_FAR && mFarDist == 0)
                continue;

            MPlane.Side side = mFrustumPlanes[plane].getSide(ref centre, ref halfSize);
            if (side == MPlane.Side.NEGATIVE_SIDE)
            {
                //if (culledBy)
                    culledBy = (FrustumPlane)plane;
                return false;
            }

        }

        return true;
    }

    public bool isVisible(ref MVector3 vert, ref FrustumPlane culledBy)
    {
        updateFrustumPlanes();

        for (int plane = 0; plane < 6; ++plane)
        {
            if (plane == (int)FrustumPlane.FRUSTUM_PLANE_FAR && mFarDist == 0)
                continue;

            if (mFrustumPlanes[plane].getSide(ref vert) == MPlane.Side.NEGATIVE_SIDE)
            {
                //if (culledBy)
                    culledBy = (FrustumPlane)plane;
                return false;
            }

        }

        return true;
    }

    public void calcProjectionParameters(ref float left, ref float right, ref float bottom, ref float top)
    { 
        if (mCustomProjMatrix)
        {
            MMatrix4 invProj = mProjMatrix.inverse();
            MVector3 topLeft = new MVector3(-0.5f, 0.5f, 0.0f);
            MVector3 bottomRight = new MVector3(0.5f, -0.5f, 0.0f);

            topLeft = invProj * topLeft;
            bottomRight = invProj * bottomRight;

            left = topLeft.x;
            top = topLeft.y;
            right = bottomRight.x;
            bottom = bottomRight.y;

        }
        else
        {
            if (mFrustumExtentsManuallySet)
            {
                left = mLeft;
                right = mRight;
                top = mTop;
                bottom = mBottom;
            }
            else if (mProjType == ProjectionType.PT_PERSPECTIVE)
            {
                float thetaY = mFOVy * 0.5f;
                float tanThetaY = UtilApi.Tan(thetaY);
                float tanThetaX = tanThetaY * mAspect;

                float nearFocal = mNearDist / mFocalLength;
                float nearOffsetX = mFrustumOffset.x * nearFocal;
                float nearOffsetY = mFrustumOffset.y * nearFocal;
                float half_w = tanThetaX * mNearDist;
                float half_h = tanThetaY * mNearDist;

                left   = - half_w + nearOffsetX;
                right  = + half_w + nearOffsetX;
                bottom = - half_h + nearOffsetY;
                top    = + half_h + nearOffsetY;

                mLeft = left;
                mRight = right;
                mTop = top;
                mBottom = bottom;
            }
            else
            {
                float half_w = getOrthoWindowWidth() * 0.5f;
                float half_h = getOrthoWindowHeight() * 0.5f;

                left   = - half_w;
                right  = + half_w;
                bottom = - half_h;
                top    = + half_h;

                mLeft = left;
                mRight = right;
                mTop = top;
                mBottom = bottom;
            }

        }
    }

    public void updateFrustumImpl()
    {
        float left = 0, right = 0, bottom = 0, top = 0;

        calcProjectionParameters(ref left, ref right, ref bottom, ref top);

        if (!mCustomProjMatrix)
        {
            float inv_w = 1 / (right - left);
            float inv_h = 1 / (top - bottom);
            float inv_d = 1 / (mFarDist - mNearDist);

            if (mProjType == ProjectionType.PT_PERSPECTIVE)
            {
                float A = 2 * mNearDist * inv_w;
                float B = 2 * mNearDist * inv_h;
                float C = (right + left) * inv_w;
                float D = (top + bottom) * inv_h;
                float q, qn;
                if (mFarDist == 0)
                {
                    q = INFINITE_FAR_PLANE_ADJUST - 1;
                    qn = mNearDist * (INFINITE_FAR_PLANE_ADJUST - 2);
                }
                else
                {
                    q = - (mFarDist + mNearDist) * inv_d;
                    qn = -2 * (mFarDist * mNearDist) * inv_d;
                }

                mProjMatrix = MMatrix4.ZERO;
                mProjMatrix.m[0, 0] = A;
                mProjMatrix.m[0, 2] = C;
                mProjMatrix.m[1, 1] = B;
                mProjMatrix.m[1, 2] = D;
                mProjMatrix.m[2, 2] = q;
                mProjMatrix.m[2, 3] = qn;
                mProjMatrix.m[3, 2] = -1;

                if (mObliqueDepthProjection)
                {
                    updateView();
                    MPlane plane = mViewMatrix * mObliqueProjPlane;

                    MVector4 qVec = new MVector4();
                    qVec.x = (UtilApi.Sign(plane.normal.x) + mProjMatrix[0, 2]) / mProjMatrix[0, 0];
                    qVec.y = (UtilApi.Sign(plane.normal.y) + mProjMatrix[1, 2]) / mProjMatrix[1, 1];
                    qVec.z = -1;
                    qVec.w = (1 + mProjMatrix[2, 2]) / mProjMatrix[2, 3];

                    MVector4 clipPlane4d = new MVector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.d);
                    MVector4 c = clipPlane4d * (2 / (clipPlane4d.dotProduct(qVec)));

                    mProjMatrix.m[2, 0] = c.x;
                    mProjMatrix.m[2, 1] = c.y;
                    mProjMatrix.m[2, 2] = c.z + 1;
                    mProjMatrix.m[2, 3] = c.w; 
                }
            }
            else if (mProjType == ProjectionType.PT_ORTHOGRAPHIC)
            {
                float A = 2 * inv_w;
                float B = 2 * inv_h;
                float C = - (right + left) * inv_w;
                float D = - (top + bottom) * inv_h;
                float q, qn;
                if (mFarDist == 0)
                {
                    q = - INFINITE_FAR_PLANE_ADJUST / mNearDist;
                    qn = - INFINITE_FAR_PLANE_ADJUST - 1;
                }
                else
                {
                    q = - 2 * inv_d;
                    qn = - (mFarDist + mNearDist)  * inv_d;
                }

                mProjMatrix = MMatrix4.ZERO;
                mProjMatrix.m[0, 0] = A;
                mProjMatrix.m[0, 3] = C;
                mProjMatrix.m[1, 1] = B;
                mProjMatrix.m[1, 3] = D;
                mProjMatrix.m[2, 2] = q;
                mProjMatrix.m[2, 3] = qn;
                mProjMatrix.m[3, 3] = 1;
            }
        }

        _convertProjectionMatrix(ref mProjMatrix, ref mProjMatrixRS);
        _convertProjectionMatrix(ref mProjMatrix, ref mProjMatrixRSDepth, true);

        float farDist = (mFarDist == 0) ? 100000 : mFarDist;

        MVector3 min = new MVector3(left, bottom, -farDist);
        MVector3 max = new MVector3(right, top, 0);

        if (mCustomProjMatrix)
        {
            MVector3 tmp = min;
            min.makeFloor(max);
            max.makeCeil(tmp);
        }

        if (mProjType == ProjectionType.PT_PERSPECTIVE)
        {
            float radio = farDist / mNearDist;
            min.makeFloor(new MVector3(left * radio, bottom * radio, -farDist));
            max.makeCeil(new MVector3(right * radio, top * radio, 0));
        }
        mBoundingBox.setExtents(ref min, ref max);

        mRecalcFrustum = false;

        mRecalcFrustumPlanes = true;
    }

    public void updateFrustum()
    {
        if (isFrustumOutOfDate())
        {
            updateFrustumImpl();
        }
    }

    public bool isViewOutOfDate()
    {
        return mRecalcView;
    }

    public bool isFrustumOutOfDate()
    {
        if (mObliqueDepthProjection)
        {
            if (isViewOutOfDate())
            {
                mRecalcFrustum = true;
            }

            if (mObliqueProjPlane != null && !(mLastLinkedObliqueProjPlane == mObliqueProjPlane))
            {
                mLastLinkedObliqueProjPlane = mObliqueProjPlane;
                mRecalcFrustum = true;
            }
        }

        return mRecalcFrustum;
    }

    public void updateViewImpl()
    {
        if (!mCustomViewMatrix)
        {
            MQuaternion orientation = getOrientationForViewUpdate();
            MVector3 position = getPositionForViewUpdate();

            mViewMatrix = UtilApi.makeViewMatrix(ref position, ref orientation, ref mReflectMatrix, mReflect);
        }

        mRecalcView = false;
        mRecalcFrustumPlanes = true;
        mRecalcWorldSpaceCorners = true;
        if (mObliqueDepthProjection)
        {
            mRecalcFrustum = true;
        }
    }

    public void calcViewMatrixRelative(ref MVector3 relPos, ref MMatrix4 matToUpdate)
    {
        MMatrix4 matTrans = MMatrix4.IDENTITY;
        matTrans.setTrans(relPos);
        matToUpdate = getViewMatrix() * matTrans;

    }

    public void updateView()
    {
        if (isViewOutOfDate())
        {
            updateViewImpl();
        }
    }

    public void updateFrustumPlanesImpl()
    {
        MMatrix4 combo = mProjMatrix * mViewMatrix;

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT].normal.x = combo[3, 0] + combo[0, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT].normal.y = combo[3, 1] + combo[0, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT].normal.z = combo[3, 2] + combo[0, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_LEFT].d = combo[3, 3] + combo[0, 3];

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT].normal.x = combo[3, 0] - combo[0, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT].normal.y = combo[3, 1] - combo[0, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT].normal.z = combo[3, 2] - combo[0, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_RIGHT].d = combo[3, 3] - combo[0, 3];

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP].normal.x = combo[3, 0] - combo[1, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP].normal.y = combo[3, 1] - combo[1, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP].normal.z = combo[3, 2] - combo[1, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_TOP].d = combo[3, 3] - combo[1, 3];

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM].normal.x = combo[3, 0] + combo[1, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM].normal.y = combo[3, 1] + combo[1, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM].normal.z = combo[3, 2] + combo[1, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_BOTTOM].d = combo[3, 3] + combo[1, 3];

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR].normal.x = combo[3, 0] + combo[2, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR].normal.y = combo[3, 1] + combo[2, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR].normal.z = combo[3, 2] + combo[2, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_NEAR].d = combo[3, 3] + combo[2, 3];

        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR].normal.x = combo[3, 0] - combo[2, 0];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR].normal.y = combo[3, 1] - combo[2, 1];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR].normal.z = combo[3, 2] - combo[2, 2];
        mFrustumPlanes[(int)FrustumPlane.FRUSTUM_PLANE_FAR].d = combo[3, 3] - combo[2, 3];

        for(int i=0; i<6; i++ ) 
        {
            float length = mFrustumPlanes[i].normal.normalise();
            mFrustumPlanes[i].d /= length;
        }

        mRecalcFrustumPlanes = false;
    }

    public void updateFrustumPlanes()
    {
        updateView();
        updateFrustum();

        if (mRecalcFrustumPlanes)
        {
            updateFrustumPlanesImpl();
        }
    }

    public void updateWorldSpaceCornersImpl()
    {
        MMatrix4 eyeToWorld = mViewMatrix.inverseAffine();

        float nearLeft = 0, nearRight = 0, nearBottom = 0, nearTop = 0;
        calcProjectionParameters(ref nearLeft, ref nearRight, ref nearBottom, ref nearTop);

        float farDist = (mFarDist == 0) ? 100000 : mFarDist;

        float radio = mProjType == ProjectionType.PT_PERSPECTIVE ? farDist / mNearDist : 1;
        float farLeft = nearLeft * radio;
        float farRight = nearRight * radio;
        float farBottom = nearBottom * radio;
        float farTop = nearTop * radio;

        mWorldSpaceCorners[0] = eyeToWorld.transformAffine(new MVector3(nearRight, nearTop,    -mNearDist));
        mWorldSpaceCorners[1] = eyeToWorld.transformAffine(new MVector3(nearLeft,  nearTop,    -mNearDist));
        mWorldSpaceCorners[2] = eyeToWorld.transformAffine(new MVector3(nearLeft,  nearBottom, -mNearDist));
        mWorldSpaceCorners[3] = eyeToWorld.transformAffine(new MVector3(nearRight, nearBottom, -mNearDist));
        // far
        mWorldSpaceCorners[4] = eyeToWorld.transformAffine(new MVector3(farRight,  farTop,     -farDist));
        mWorldSpaceCorners[5] = eyeToWorld.transformAffine(new MVector3(farLeft,   farTop,     -farDist));
        mWorldSpaceCorners[6] = eyeToWorld.transformAffine(new MVector3(farLeft,   farBottom,  -farDist));
        mWorldSpaceCorners[7] = eyeToWorld.transformAffine(new MVector3(farRight,  farBottom,  -farDist));


        mRecalcWorldSpaceCorners = false;
    }

    public void updateWorldSpaceCorners()
    {
        updateView();

        if (mRecalcWorldSpaceCorners)
        {
            updateWorldSpaceCornersImpl();
        }
    }

    public float getAspectRatio()
    {
        return mAspect;
    }

    public void setAspectRatio(float r)
    {
        mAspect = r;
        invalidateFrustum();
    }

public MAxisAlignedBox getBoundingBox()
    {
        return mBoundingBox;
    }

    public string getMovableType()
    {
        return msMovableType;
    }

    public void invalidateFrustum()
    {
        mRecalcFrustum = true;
        mRecalcFrustumPlanes = true;
        mRecalcWorldSpaceCorners = true;
        mRecalcVertexData = true;
    }

    public void invalidateView()
    {
        mRecalcView = true;
        mRecalcFrustumPlanes = true;
        mRecalcWorldSpaceCorners = true;
    }

    public MVector3[] getWorldSpaceCorners()
    {
        updateWorldSpaceCorners();

        return mWorldSpaceCorners;
    }

    public void setProjectionType(ProjectionType pt)
    {
        mProjType = pt;
        invalidateFrustum();
    }

public ProjectionType getProjectionType()
    {
        return mProjType;
    }

    public MVector3 getPositionForViewUpdate()
    {
        return mLastParentPosition;
    }

    public MQuaternion getOrientationForViewUpdate()
    {
        return mLastParentOrientation;
    }

    public void enableReflection(ref MPlane p)
    {
        mReflect = true;
        mReflectPlane = p;
        mReflectMatrix = UtilApi.buildReflectionMatrix(ref p);
        invalidateView();

    }

public void disableReflection()
    {
        mReflect = false;
            mLastLinkedReflectionPlane.normal = MVector3.ZERO;
        invalidateView();
    }

public void enableCustomNearClipPlane(ref MPlane plane)
    {
        mObliqueDepthProjection = true;
        mObliqueProjPlane = plane;
        invalidateFrustum();
    }

public void disableCustomNearClipPlane()
    {
        mObliqueDepthProjection = false;
        invalidateFrustum();
    }

public void setCustomViewMatrix(bool enable, ref MMatrix4 viewMatrix)
    {
        mCustomViewMatrix = enable;
        if (enable)
        {
            UtilApi.assert(viewMatrix.isAffine());
            mViewMatrix = viewMatrix;
        }
        invalidateView();
    }

public void setCustomProjectionMatrix(bool enable, ref MMatrix4 projMatrix)
    {
        mCustomProjMatrix = enable;
        if (enable)
        {
            mProjMatrix = projMatrix;
        }
        invalidateFrustum();
    }

public void setOrthoWindow(float w, float h)
    {
        mOrthoHeight = h;
        mAspect = w / h;
        invalidateFrustum();
    }

public void setOrthoWindowHeight(float h)
    {
        mOrthoHeight = h;
        invalidateFrustum();
    }

public void setOrthoWindowWidth(float w)
    {
        mOrthoHeight = w / mAspect;
        invalidateFrustum();
    }

public float getOrthoWindowHeight()
    {
        return mOrthoHeight;
    }

    public float getOrthoWindowWidth()
    {
        return mOrthoHeight * mAspect;  
    }

public void setFrustumExtents(float left, float right, float top, float bottom)
    {
        mFrustumExtentsManuallySet = true;
        mLeft = left;
        mRight = right;
        mTop = top;
        mBottom = bottom;

        invalidateFrustum();
    }

public void resetFrustumExtents()
    {
        mFrustumExtentsManuallySet = false;
        invalidateFrustum();
    }

public void getFrustumExtents(ref float outleft, ref float outright, ref float outtop, ref float outbottom)
    {
        updateFrustum();
        outleft = mLeft;
        outright = mRight;
        outtop = mTop;
        outbottom = mBottom;
    }

public void setOrientationMode(OrientationMode orientationMode)
    {
        mOrientationMode = orientationMode;
        invalidateFrustum();
    }

public OrientationMode getOrientationMode()
    {
        return mOrientationMode;
    }

protected void _convertProjectionMatrix(ref MMatrix4 matrix,
    ref MMatrix4 dest, bool forGpuProgram = false)
{
    bool idDX = true;
    if (idDX)
    {
        dest = matrix;

        dest.m[2, 0] = (dest[2, 0] + dest[3, 0]) / 2;
        dest.m[2, 1] = (dest[2, 1] + dest[3, 1]) / 2;
        dest.m[2, 2] = (dest[2, 2] + dest[3, 2]) / 2;
        dest.m[2, 3] = (dest[2, 3] + dest[3, 3]) / 2;

        if (!forGpuProgram)
        {
            dest.m[0, 2] = -dest[0, 2];
            dest.m[1, 2] = -dest[1, 2];
            dest.m[2, 2] = -dest[2, 2];
            dest.m[3, 2] = -dest[3, 2];
        }
    }
    else
    {
        dest = matrix;
    }
}
    }
}