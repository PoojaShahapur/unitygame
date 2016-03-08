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

    public class Frustum
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
    
    public Frustum()
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

        public const Radian& getFOVy()
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
        setFrustumOffset(new MVector2(horizontal, vertical));
    }

public ref MVector2 getFrustumOffset()
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

    public MPlane getFrustumPlanes()
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
                if (culledBy)
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
                if (culledBy)
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

                    MVector4 qVec = new MVector4;
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

        _convertProjectionMatrix(mProjMatrix, mProjMatrixRS);
        _convertProjectionMatrix(mProjMatrix, mProjMatrixRSDepth, true);

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

    public void updateVertexData()
    {
        if (mRecalcVertexData)
        {
            if (mVertexData.vertexBufferBinding->getBufferCount() <= 0)
            {
                // Initialise vertex & index data
                mVertexData.vertexDeclaration->addElement(0, 0, VET_FLOAT3, VES_POSITION);
                mVertexData.vertexCount = 32;
                mVertexData.vertexStart = 0;
                mVertexData.vertexBufferBinding->setBinding( 0,
                    v1::HardwareBufferManager::getSingleton().createVertexBuffer(
                        sizeof(float)*3, 32, v1::HardwareBuffer::HBU_DYNAMIC_WRITE_ONLY) );
            }

            // Note: Even though we can dealing with general projection matrix here,
            //       but because it's incompatibly with infinite far plane, thus, we
            //       still need to working with projection parameters.

            // Calc near plane corners
            Real vpLeft, vpRight, vpBottom, vpTop;
            calcProjectionParameters(vpLeft, vpRight, vpBottom, vpTop);

            // Treat infinite fardist as some arbitrary far value
            Real farDist = (mFarDist == 0) ? 100000 : mFarDist;

            // Calc far plane corners
            Real radio = mProjType == PT_PERSPECTIVE ? farDist / mNearDist : 1;
            Real farLeft = vpLeft * radio;
            Real farRight = vpRight * radio;
            Real farBottom = vpBottom * radio;
            Real farTop = vpTop * radio;

            // Calculate vertex positions (local)
            // 0 is the origin
            // 1, 2, 3, 4 are the points on the near plane, top left first, clockwise
            // 5, 6, 7, 8 are the points on the far plane, top left first, clockwise
            v1::HardwareVertexBufferSharedPtr vbuf = mVertexData.vertexBufferBinding->getBuffer(0);
            float* pFloat = static_cast<float*>(vbuf->lock(v1::HardwareBuffer::HBL_DISCARD));

            // near plane (remember frustum is going in -Z direction)
            *pFloat++ = vpLeft;  *pFloat++ = vpTop;    *pFloat++ = -mNearDist;
            *pFloat++ = vpRight; *pFloat++ = vpTop;    *pFloat++ = -mNearDist;

            *pFloat++ = vpRight; *pFloat++ = vpTop;    *pFloat++ = -mNearDist;
            *pFloat++ = vpRight; *pFloat++ = vpBottom; *pFloat++ = -mNearDist;

            *pFloat++ = vpRight; *pFloat++ = vpBottom; *pFloat++ = -mNearDist;
            *pFloat++ = vpLeft;  *pFloat++ = vpBottom; *pFloat++ = -mNearDist;

            *pFloat++ = vpLeft;  *pFloat++ = vpBottom; *pFloat++ = -mNearDist;
            *pFloat++ = vpLeft;  *pFloat++ = vpTop;    *pFloat++ = -mNearDist;

            // far plane (remember frustum is going in -Z direction)
            *pFloat++ = farLeft;  *pFloat++ = farTop;    *pFloat++ = -farDist;
            *pFloat++ = farRight; *pFloat++ = farTop;    *pFloat++ = -farDist;

            *pFloat++ = farRight; *pFloat++ = farTop;    *pFloat++ = -farDist;
            *pFloat++ = farRight; *pFloat++ = farBottom; *pFloat++ = -farDist;

            *pFloat++ = farRight; *pFloat++ = farBottom; *pFloat++ = -farDist;
            *pFloat++ = farLeft;  *pFloat++ = farBottom; *pFloat++ = -farDist;

            *pFloat++ = farLeft;  *pFloat++ = farBottom; *pFloat++ = -farDist;
            *pFloat++ = farLeft;  *pFloat++ = farTop;    *pFloat++ = -farDist;

            // Sides of the pyramid
            *pFloat++ = 0.0f;    *pFloat++ = 0.0f;   *pFloat++ = 0.0f;
            *pFloat++ = vpLeft;  *pFloat++ = vpTop;  *pFloat++ = -mNearDist;

            *pFloat++ = 0.0f;    *pFloat++ = 0.0f;   *pFloat++ = 0.0f;
            *pFloat++ = vpRight; *pFloat++ = vpTop;    *pFloat++ = -mNearDist;

            *pFloat++ = 0.0f;    *pFloat++ = 0.0f;   *pFloat++ = 0.0f;
            *pFloat++ = vpRight; *pFloat++ = vpBottom; *pFloat++ = -mNearDist;

            *pFloat++ = 0.0f;    *pFloat++ = 0.0f;   *pFloat++ = 0.0f;
            *pFloat++ = vpLeft;  *pFloat++ = vpBottom; *pFloat++ = -mNearDist;

            // Sides of the box

            *pFloat++ = vpLeft;  *pFloat++ = vpTop;  *pFloat++ = -mNearDist;
            *pFloat++ = farLeft;  *pFloat++ = farTop;  *pFloat++ = -farDist;

            *pFloat++ = vpRight; *pFloat++ = vpTop;    *pFloat++ = -mNearDist;
            *pFloat++ = farRight; *pFloat++ = farTop;    *pFloat++ = -farDist;

            *pFloat++ = vpRight; *pFloat++ = vpBottom; *pFloat++ = -mNearDist;
            *pFloat++ = farRight; *pFloat++ = farBottom; *pFloat++ = -farDist;

            *pFloat++ = vpLeft;  *pFloat++ = vpBottom; *pFloat++ = -mNearDist;
            *pFloat++ = farLeft;  *pFloat++ = farBottom; *pFloat++ = -farDist;


            vbuf->unlock();

            mRecalcVertexData = false;
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

            if (mLinkedObliqueProjPlane && 
                !(mLastLinkedObliqueProjPlane == mLinkedObliqueProjPlane->_getDerivedPlane()))
            {
                mObliqueProjPlane = mLinkedObliqueProjPlane->_getDerivedPlane();
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
            MMatrix3 rot = new MMatrix3();
            MQuaternion orientation = getOrientationForViewUpdate();
            MVector3 position = getPositionForViewUpdate();

            mViewMatrix = Math::makeViewMatrix(position, orientation, mReflect? &mReflectMatrix : 0);
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

    public void getCustomWorldSpaceCorners(
                ArrayVector3 outCorners[(8 + ARRAY_PACKED_REALS - 1) / ARRAY_PACKED_REALS],
                Real customFarPlane )
    {
        updateView();

        ArrayMatrixAf4x3 eyeToWorld;
        eyeToWorld.setAll( mViewMatrix.inverseAffine() );

        Real nearLeft, nearRight, nearBottom, nearTop;
        calcProjectionParameters(nearLeft, nearRight, nearBottom, nearTop);

        Real farDist = (customFarPlane == 0) ? 100000 : customFarPlane;

        Real radio = mProjType == PT_PERSPECTIVE ? farDist / mNearDist : 1;
        Real farLeft = nearLeft * radio;
        Real farRight = nearRight * radio;
        Real farBottom = nearBottom * radio;
        Real farTop = nearTop * radio;

        ArrayVector3 corners[(8 + ARRAY_PACKED_REALS - 1) / ARRAY_PACKED_REALS];

        OGRE_ALIGNED_DECL( Real, scalarCorners[8 * 4 * (ARRAY_PACKED_REALS <= 8 ?
                                               1 : (ARRAY_PACKED_REALS / 8))],
                           OGRE_SIMD_ALIGNMENT );
        memset( scalarCorners, 0, sizeof( scalarCorners ) );

        scalarCorners[0] = nearRight;   scalarCorners[1] = nearTop;     scalarCorners[2] = -mNearDist;
        scalarCorners[4] = nearLeft;    scalarCorners[5] = nearTop;     scalarCorners[6] = -mNearDist;
        scalarCorners[8] = nearLeft;    scalarCorners[9] = nearBottom;  scalarCorners[10]= -mNearDist;
        scalarCorners[12]= nearRight;   scalarCorners[13]= nearBottom;  scalarCorners[14]= -mNearDist;
        scalarCorners[3] = scalarCorners[7] = scalarCorners[11] = scalarCorners[15] = 0;

        scalarCorners[16]= farRight;    scalarCorners[17]= farTop;      scalarCorners[18]= -farDist;
        scalarCorners[20]= farLeft;     scalarCorners[21]= farTop;      scalarCorners[22]= -farDist;
        scalarCorners[24]= farLeft;     scalarCorners[25]= farBottom;   scalarCorners[26]= -farDist;
        scalarCorners[28]= farRight;    scalarCorners[29]= farBottom;   scalarCorners[30]= -farDist;
        scalarCorners[19] = scalarCorners[23] = scalarCorners[27] = scalarCorners[31] = 0;

        for( size_t i=32; i<ARRAY_PACKED_REALS * 8; i += 4 )
        {
            scalarCorners[i+0] = farRight;
            scalarCorners[i+1] = farBottom;
            scalarCorners[i+2] = -farDist;
            scalarCorners[i+3] = 0;
        }

        for( size_t i=0; i<(8 + ARRAY_PACKED_REALS - 1) / ARRAY_PACKED_REALS; ++i )
        {
            corners[i].loadFromAoS( scalarCorners + i * ARRAY_PACKED_REALS * 4 );
            outCorners[i] = eyeToWorld * corners[i];
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

    public MVector3 getWorldSpaceCorners()
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
        mLinkedReflectPlane = 0;
        mReflectMatrix = Math::buildReflectionMatrix(p);
        invalidateView();

    }

public void enableReflection( MovablePlane p)
    {
        mReflect = true;
        mLinkedReflectPlane = p;
        mReflectPlane = mLinkedReflectPlane->_getDerivedPlane();
        mReflectMatrix = Math::buildReflectionMatrix(mReflectPlane);
        mLastLinkedReflectionPlane = mLinkedReflectPlane->_getDerivedPlane();
        invalidateView();
    }

public void disableReflection()
    {
        mReflect = false;
        mLinkedReflectPlane = 0;
        mLastLinkedReflectionPlane.normal = Vector3::ZERO;
        invalidateView();
    }

    public void enableCustomNearClipPlane(MovablePlane* plane)
    {
        mObliqueDepthProjection = true;
        mLinkedObliqueProjPlane = plane;
        mObliqueProjPlane = plane->_getDerivedPlane();
        invalidateFrustum();
    }

public void enableCustomNearClipPlane(ref MPlane plane)
    {
        mObliqueDepthProjection = true;
        mLinkedObliqueProjPlane = 0;
        mObliqueProjPlane = plane;
        invalidateFrustum();
    }

public void disableCustomNearClipPlane()
    {
        mObliqueDepthProjection = false;
        mLinkedObliqueProjPlane = 0;
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
    ref MMatrix4 dest, bool forGpuProgram)
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