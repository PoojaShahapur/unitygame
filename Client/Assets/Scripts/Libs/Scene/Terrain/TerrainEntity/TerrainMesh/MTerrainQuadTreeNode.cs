using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    public enum QuadTreeChildIndex
    {
        eLEFT_BOTTOM = 0,
        eRIGHT_BOTTOM = 1,
        eLEFT_TOP = 2,
        eRIGHT_TOP = 3,
        eTOTAL,
    }

    public class MTerrainQuadTreeNode : MMovableObject
    {
        protected MTerrain mTerrain;
        protected MTerrainQuadTreeNode mParent;
        protected MTerrainQuadTreeNode[] mChildren;

        protected ushort mOffsetX, mOffsetY;
        protected ushort mBoundaryX, mBoundaryY;

        protected ushort mSize;
        protected ushort mBaseLod;
        protected ushort mDepth;
        protected ushort mQuadrant;
        protected MVector3 mLocalCentre;
        protected MAxisAlignedBox mAABB;
        protected MAxisAlignedBox mWorldAabb;
        protected float mBoundingRadius;
        protected int mCurrentLod;
        protected bool mSelfOrChildRendered;
        protected MVertexDataRecord mVertexDataRecord;
        protected TerrainTileRender mTileRender;

        protected MSceneNode mLocalNode;
        protected int mCurIndexBufferIndex;
        protected bool mIsVertexDataInit;

        protected bool m_bShowBoundBox;         // 显示 BoundBox
        protected MAABBMeshRender m_aabbMeshRender;
        protected MTreeNodeStateNotify mTreeNodeStateNotify;
        protected bool mIsSceneGraphVisible;

        public MTerrainQuadTreeNode(MTerrain terrain,
        MTerrainQuadTreeNode parent, ushort xoff, ushort yoff, ushort size,
        ushort lod, ushort depth, ushort quadrant)
        {
            mTerrain = terrain;
            mParent = parent;
            mOffsetX = xoff;
            mOffsetY = yoff;
            mBoundaryX = (ushort)(xoff + size);
            mBoundaryY = (ushort)(yoff + size);
            mSize = size;
            mBaseLod = lod;
            mDepth = depth;
            mQuadrant = quadrant;
            mBoundingRadius = 0;
            mCurrentLod = -1;
            mSelfOrChildRendered = false;
            mLocalNode = null;
            mCurIndexBufferIndex = 0;

            mAABB = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            mWorldAabb = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
            mIsVertexDataInit = false;
            m_bShowBoundBox = false;
            mTreeNodeStateNotify = new MTreeNodeStateNotify(this);
            mIsSceneGraphVisible = false;

            if (terrain.getMaxBatchSize() < size)
            {
                mChildren = new MTerrainQuadTreeNode[(int)QuadTreeChildIndex.eTOTAL];
                ushort childSize = (ushort)(((size - 1) * 0.5f) + 1);
                ushort childOff = (ushort)(childSize - 1);
                ushort childLod = (ushort)(lod - 1);
                ushort childDepth = (ushort)(depth + 1);

                mChildren[(int)QuadTreeChildIndex.eLEFT_BOTTOM] = new MTerrainQuadTreeNode(terrain, this, xoff, yoff, childSize, childLod, childDepth, 0);
                mChildren[(int)QuadTreeChildIndex.eRIGHT_BOTTOM] = new MTerrainQuadTreeNode(terrain, this, (ushort)(xoff + childOff), yoff, childSize, childLod, childDepth, 1);
                mChildren[(int)QuadTreeChildIndex.eLEFT_TOP] = new MTerrainQuadTreeNode(terrain, this, xoff, (ushort)(yoff + childOff), childSize, childLod, childDepth, 2);
                mChildren[(int)QuadTreeChildIndex.eRIGHT_TOP] = new MTerrainQuadTreeNode(terrain, this, (ushort)(xoff + childOff), (ushort)(yoff + childOff), childSize, childLod, childDepth, 3);
            }
            else
            {
                if(!Ctx.m_instance.m_terrainBufferSys.getAABB(mTerrain.getTerrainId(), getNameStr(), ref mAABB))
                {
                    mAABB.setMinimum(new MVector3(-mTerrain.getMaxBatchWorldSize() / 2, -mTerrain.getMaxBatchWorldSize() / 2, -mTerrain.getMaxBatchWorldSize() / 2));
                    mAABB.setMaximum(new MVector3(mTerrain.getMaxBatchWorldSize() / 2, mTerrain.getMaxBatchWorldSize() / 2, mTerrain.getMaxBatchWorldSize() / 2));
                }

                mBaseLod = 0;
                //mVertexDataRecord = new MVertexDataRecord();
                //mTileRender = new TerrainTileRender(this);
                //mTileRender.pntGo = mTerrain._getRootSceneNode().selfGo;
                //mTileRender.setTmplMaterial(mTerrain.getMatTmpl());
            }

            ushort midoffset = (ushort)((size - 1) / 2);
            ushort midpointx = (ushort)(mOffsetX + midoffset);
            ushort midpointy = (ushort)(mOffsetY + midoffset);

            mTerrain.getPoint(midpointx, midpointy, 0, ref mLocalCentre);

            if (terrain.getMaxBatchSize() == size)
            {
                updateWorldAABB();
            }
        }

        public void serialize(ByteBuffer headerBuffer, ByteBuffer vertexBuffer)
        {
            if(isLeaf())
            {
                SerializeHeader header = mTerrain.mSerializeData.getSerialHeader(getNameStr());
                header.mUniqueId = getNameStr();
                header.mOffset = (int)vertexBuffer.length + mTerrain.mSerializeData.getBasicAndHeaderSize();
                mVertexDataRecord.cpuVertexData.writeVertData(vertexBuffer);
                vertexBuffer.writeAABB(mAABB);
                headerBuffer.writeMultiByte(header.mUniqueId, Encoding.UTF8, mTerrain.mSerializeData.mUniqueIdSize);
                headerBuffer.writeInt32(header.mOffset);
            }
            else
            {
                for(int idx = 0; idx < 4; ++idx)
                {
                    mChildren[idx].serialize(headerBuffer, vertexBuffer);
                }
            }
        }

        public bool isLeaf()
        {
            //return mChildren[0] == null;
            return mChildren == null;
        }

        public bool isRoot()
        {
            return mDepth == 0;
        }

        public MTerrainQuadTreeNode getChild(ushort child)
        {
            if (isLeaf() || child >= 4)
                return null;

            return mChildren[child];
        }

        public MTerrainQuadTreeNode getParent()
        {
            return mParent;
        }

        public MTerrain getTerrain()
        {
            return mTerrain;
        }

        public void prepare()
        {
            if (!isLeaf())
            {
                for (int i = 0; i < 4; ++i)
                    mChildren[i].prepare();
            }
        }

        override public MAxisAlignedBox getBoundingBox()
        {
            return this.mAABB;
        }

        override public MAxisAlignedBox getWorldBoundingBox(bool derive)
        {
            return this.mWorldAabb;
        }

        public MAxisAlignedBox getAABB()
        {
            return mAABB;
        }

        override public float getBoundingRadius()
        {
            return mBoundingRadius;
        }

        public float getMinHeight()
        {
            switch (mTerrain.getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                default:
                    return mAABB.getMinimum().z;
                case Alignment.ALIGN_X_Z:
                    return mAABB.getMinimum().y;
                case Alignment.ALIGN_Y_Z:
                    return mAABB.getMinimum().x;
            };
        }

        public float getMaxHeight()
        {
            switch (mTerrain.getAlignment())
            {
                case Alignment.ALIGN_X_Y:
                default:
                    return mAABB.getMaximum().z;
                case Alignment.ALIGN_X_Z:
                    return mAABB.getMaximum().y;
                case Alignment.ALIGN_Y_Z:
                    return mAABB.getMaximum().x;
            };
        }

        public bool rectContainsNode(ref MTRectI rect)
        {
            return (rect.left <= mOffsetX && rect.right >= mBoundaryX &&
                rect.top <= mOffsetY && rect.bottom >= mBoundaryY);
        }

        public void resetBounds(ref MTRectI rect)
        {
            if (rectContainsNode(ref rect))
            {
                mAABB.setNull();
                mWorldAabb.setNull();
                mBoundingRadius = 0;

                if (!isLeaf())
                {
                    for (int i = 0; i < 4; ++i)
                        mChildren[i].resetBounds(ref rect);
                }
            }
        }

        public void mergeIntoBounds(long x, long y, ref MVector3 pos)
        {
            if (pointIntersectsNode(x, y))
            {
                MVector3 localPos = pos - mLocalCentre;
                mAABB.merge(ref localPos);

                updateWorldAABB();

                mBoundingRadius = UtilMath.max(mBoundingRadius, localPos.length());

                if (!isLeaf())
                {
                    for (int i = 0; i < 4; ++i)
                        mChildren[i].mergeIntoBounds(x, y, ref pos);
                }
            }
        }

        public bool pointIntersectsNode(long x, long y)
        {
            return x >= mOffsetX && x < mBoundaryX &&
                y >= mOffsetY && y < mBoundaryY;
        }

        public void assignVertexData(ushort treeDepthStart, ushort treeDepthEnd, ushort resolution, uint sz)
        {
            //UtilApi.assert(treeDepthStart >= mDepth, "Should not be calling this");

            mIsVertexDataInit = true;

            if (this.isLeaf())
            {
                createCpuVertexData();
            }
            else
            {
                UtilApi.assert(!isLeaf(), "No more levels below this!");

                for (int i = 0; i < 4; ++i)
                    mChildren[i].assignVertexData(treeDepthStart, treeDepthEnd, resolution, sz);
            }
        }

        public void updateVertexData(bool positions, bool deltas, MTRectI rect, bool cpuData)
        {

        }

        public void createCpuVertexData()
        {
            if (!Ctx.m_instance.m_terrainBufferSys.getVertData(mTerrain.getTerrainId(), getNameStr(), ref mVertexDataRecord))
            {
                mVertexDataRecord = new MVertexDataRecord();
                mCurIndexBufferIndex = 0;
                MTRectI updateRect = new MTRectI((int)mOffsetX, (int)mOffsetY, (int)mBoundaryX, (int)mBoundaryY);
                updateVertexBuffer(null, null, ref updateRect);
            }
            else
            {
                mAABB.setNull();
                mWorldAabb.setNull();
                mBoundingRadius = 0;

                Ctx.m_instance.m_terrainBufferSys.getAABB(mTerrain.getTerrainId(), getNameStr(), ref mAABB);
                updateWorldAABB();

                mBoundingRadius = mAABB.getHalfSize().length();
            }
        }

        public void updateVertexBuffer(float[] posbuf, float[] deltabuf, ref MTRectI rect)
        {
            UtilApi.assert(rect.left >= mOffsetX && rect.right <= mBoundaryX &&
                rect.top >= mOffsetY && rect.bottom <= mBoundaryY);

            resetBounds(ref rect);

            float uvScale = 1.0f / (mTerrain.getSize() - 1);
            ushort inc = 1;
            float height = 0;
            //float heightData = 0;
            MVector3 pos = new MVector3(0, 0, 0);
            //Ctx.m_instance.m_terrainBufferSys.openFile();
            for (ushort y = (ushort)rect.top; y < rect.bottom; y += inc)
            {
                for (ushort x = (ushort)rect.left; x < rect.right; x += inc)
                {
                    height = mTerrain.getHeightAtPoint(x, y);
                    //heightData = mTerrain.getOrigHeightData(x, y);
                    mTerrain.getPoint(x, y, height, ref pos);
                    //Ctx.m_instance.m_terrainBufferSys.writeVertex(pos, heightData);
                    mergeIntoBounds(x, y, ref pos);
                    pos -= mLocalCentre;
                    writePosVertex(x, y, ref pos, uvScale);
                }
            }
            //Ctx.m_instance.m_terrainBufferSys.closeFile();
        }

        protected void writePosVertex(ushort x, ushort y, ref MVector3 pos, float uvScale)
        {
            int vertexIndex = (y - mOffsetY) * mTerrain.getMaxBatchSize() + (x - mOffsetX);
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].x = pos.x;
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].y = pos.y;
            mVertexDataRecord.cpuVertexData.m_vertexs[vertexIndex].z = pos.z;

            //mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].x = x * uvScale;
            //mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].y = 1.0f - (y * uvScale);

            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].x = mTerrain.getU(x);
            mVertexDataRecord.cpuVertexData.m_uvs[vertexIndex].y = mTerrain.getV(y);

            if (x != mBoundaryX - 1 && y != mBoundaryY - 1)
            {
                int vertexWidth = mTerrain.getMaxBatchSize();
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 1] = vertexIndex + vertexWidth;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 2] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 3] = vertexIndex;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 4] = vertexIndex + vertexWidth + 1;
                mVertexDataRecord.cpuVertexData.m_indexs[mCurIndexBufferIndex + 5] = vertexIndex + 1;

                mCurIndexBufferIndex += 6;
            }

            mTerrain.getPointNormal(x, y, ref mVertexDataRecord.cpuVertexData.m_vertexNormals[vertexIndex]);
            mTerrain.getPointTangent(x, y, ref mVertexDataRecord.cpuVertexData.m_vertexTangents[vertexIndex]);

            //mVertexDataRecord.cpuVertexData.m_vertexNormals[vertexIndex] = new Vector3(0, 1, 0);
            //mVertexDataRecord.cpuVertexData.m_vertexTangents[vertexIndex] = new Vector3(0, 0, 1);
        }

        public Vector3[] getVertexData()
        {
            if (isLeaf())
            {
                return mVertexDataRecord.cpuVertexData.m_vertexs;
            }

            return null;
        }

        public int getVertexDataCount()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexs.Length;
        }

        public int getTriangleCount()
        {
            return mVertexDataRecord.cpuVertexData.m_indexs.Length / 2;
        }

        public Vector2[] getUVData()
        {
            return mVertexDataRecord.cpuVertexData.m_uvs;
        }

        public Color32[] getVectexColorData()
        {
            return null;
        }

        public Vector3[] getVertexNormalsData()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexNormals;
        }

        public Vector4[] getVertexTangentsData()
        {
            return mVertexDataRecord.cpuVertexData.m_vertexTangents;
        }

        public int[] getIndexData()
        {
            return mVertexDataRecord.cpuVertexData.m_indexs;
        }

        public void clear()
        {

        }

        public MVector3 getLocalCentre()
        {
            return mLocalCentre;
        }

        public MVector3 getWorldPos()
        {
            return mLocalCentre + mTerrain.getWorldPos();
        }

        override public void show(MFrustum frustum)
        {
            if (isLeaf())
            {
                if (!mIsVertexDataInit)
                {
                    if (mLocalNode == null)
                    {
                        // tree Node 不创建 Scene Node，使用 Terrain 的 Scene Node
                        //mLocalNode = mTerrain._getRootSceneNode().createChildSceneNode(mLocalCentre, MQuaternion.IDENTITY);
                        mLocalNode = mTerrain._getRootSceneNode();
                    }

                    if (!this.isAttached())
                    {
                        mLocalNode.attachObject(this);
                    }
                    assignVertexData(0, 0, 0, 0);
                }

                //mTileRender.pntGo = this.mParentNode.selfGo;    // 从移动对象中直接取值
                //mTileRender.show();
                attachRender();
                showBoundBox();

                Ctx.m_instance.m_terrainBufferSys.mTerrainVisibleCheck.addVisibleTreeNode(this);
                mIsVisible = true;
                mTreeNodeStateNotify.onShow();
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    mChildren[i].show(frustum);
                }
            }
        }

        override public void hide(MFrustum frustum)
        {
            if (isLeaf())
            {
                //mTileRender.hide();
                detachRender();
                hideBoundBox();
                mIsVisible = false;
                mTreeNodeStateNotify.onHide();

                //FrustumPlane culledBy = FrustumPlane.FRUSTUM_PLANE_LEFT;
                //if (frustum.isVisible(ref mWorldAabb, ref culledBy))
                //{
                //    Debug.Log("aaaaa");
                //}
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    mChildren[i].hide(frustum);
                }
            }
        }

        public void updateAABB()
        {
            if (!isLeaf())
            {
                for (int index = 0; index < 4; ++index)
                {
                    mChildren[index].updateAABB();

                    MAxisAlignedBox childBox = mChildren[index].getAABB();
                    MVector3 boxoffset = mChildren[index].getLocalCentre() - getLocalCentre();
                    childBox.setMinimum(childBox.getMinimum() + boxoffset);
                    childBox.setMaximum(childBox.getMaximum() + boxoffset);
                    mAABB.merge(childBox);

                    //MMatrix4 worldMat = new MMatrix4();
                    //mWorldAabb.transformAffine(ref worldMat);

                    updateWorldAABB();
                }
            }
            else
            {
                updateWorldAABB();
            }
        }

        public void cullNode(MFrustum frustum)
        {
            FrustumPlane culledBy = FrustumPlane.FRUSTUM_PLANE_LEFT;
            if (frustum.isVisible(ref mWorldAabb, ref culledBy))
            {
                if (isLeaf())
                {
                    show(frustum);
                    //frustum.isVisible(ref mWorldAabb, ref culledBy);
                }
                else
                {
                    for (int index = 0; index < 4; ++index)
                    {
                        mChildren[index].cullNode(frustum);
                    }
                }
            }
            else
            {
                hide(frustum);
            }
        }

        public string getLayerStr()
        {
            return mTerrain.getLayerStr();
        }

        public void updateWorldAABB()
        {
            mWorldAabb.setMinimum(mAABB.getMinimum() + getWorldPos());
            mWorldAabb.setMaximum(mAABB.getMaximum() + getWorldPos());
        }

        protected void showBoundBox()
        {
            if (m_bShowBoundBox)
            {
                if (m_aabbMeshRender == null)
                {
                    m_aabbMeshRender = new MAABBMeshRender();
                }

                m_aabbMeshRender.setName(getNameStr());
                m_aabbMeshRender.addVertex(ref mWorldAabb);
                m_aabbMeshRender.buildIndex();
                m_aabbMeshRender.uploadGeometry();
                m_aabbMeshRender.show();
            }
        }

        protected void hideBoundBox()
        {
            if (m_bShowBoundBox)
            {
                if (m_aabbMeshRender != null)
                {
                    m_aabbMeshRender.hide();
                }
            }
        }

        public string getNameStr()
        {
            if (isRoot())
            {
                return mQuadrant.ToString();
            }
            else
            {
                return mParent.getNameStr() + "_" + mQuadrant.ToString();
            }
        }

        public void attachMO()
        {
            if (isLeaf())
            {
                if (mLocalNode == null)
                {
                    mLocalNode = mTerrain._getRootSceneNode();
                }

                if (!this.isAttached())
                {
                    mLocalNode.attachObject(this);
                }
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    mChildren[i].attachMO();
                }
            }
        }

        public void attachRender()
        {
            if (mTileRender == null)
            {
                if (!Ctx.m_instance.m_terrainBufferSys.getTerrainTileRender(mTerrain.getTerrainId(), this.getNameStr(), ref mTileRender))
                {
                    mTileRender = new TerrainTileRender(this);
                    mTileRender.setTmplMaterial(mTerrain.getMatTmpl());

                    mTileRender.pntGo = Ctx.m_instance.mSceneNodeGraph.mSceneNodes[(int)eSceneNodeId.eSceneTerrainRoot];
                    mTileRender.show();
                }
                else
                {
                    mTileRender.setTreeNode(this);
                    mTileRender.show();
                }
            }
            else
            {
                mTileRender.show();
            }
        }

        public void detachRender()
        {
            if(mTileRender != null)
            {
                mTileRender.hide();
                Ctx.m_instance.m_terrainBufferSys.addTerrainTileRender(mTerrain.getTerrainId(), this.getNameStr(), ref mTileRender);
            }
        }

        // 节点在二维数组中的索引
        public bool getNodeIndex(ref int idx, ref int idz)
        {
            idx = (mOffsetX - 1) / (Ctx.m_instance.mTerrainGlobalOption.mMaxBatchSize - 1);
            idz = (mOffsetY - 1) / (Ctx.m_instance.mTerrainGlobalOption.mMaxBatchSize - 1);
            return true;
        }

        public bool isSceneGraphVisible()
        {
            return mIsSceneGraphVisible;
        }

        // 获取四叉树节点，posX 是图像空间的位置
        public MTerrainQuadTreeNode getTerrainQuadTreeNode(int posX, int posY)
        {
            if (isLeaf())
            {
                return this;
            }
            else
            {
                if(mChildren[(int)QuadTreeChildIndex.eLEFT_BOTTOM].isInBound(posX, posY))
                {
                    return mChildren[(int)QuadTreeChildIndex.eLEFT_BOTTOM].getTerrainQuadTreeNode(posX, posY);
                }
                else if(mChildren[(int)QuadTreeChildIndex.eRIGHT_BOTTOM].isInBound(posX, posY))
                {
                    return mChildren[(int)QuadTreeChildIndex.eRIGHT_BOTTOM].getTerrainQuadTreeNode(posX, posY);
                }
                else if (mChildren[(int)QuadTreeChildIndex.eLEFT_TOP].isInBound(posX, posY))
                {
                    return mChildren[(int)QuadTreeChildIndex.eLEFT_TOP].getTerrainQuadTreeNode(posX, posY);
                }
                else
                {
                    return mChildren[(int)QuadTreeChildIndex.eRIGHT_TOP].getTerrainQuadTreeNode(posX, posY);
                }
            }
        }

        public bool isInBound(int posX, int posY)
        {
            if(mOffsetX <= posX && 
               posX <= mBoundaryX &&
               mOffsetY <= posY &&
               posY <= mBoundaryY)
            {
                return true;
            }

            return false;
        }

        public MTreeNodeStateNotify getTreeNodeStateNotify()
        {
            return mTreeNodeStateNotify;
        }
    }
}