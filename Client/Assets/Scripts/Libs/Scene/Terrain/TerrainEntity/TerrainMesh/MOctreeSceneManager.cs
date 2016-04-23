using System.Collections.Generic;

namespace SDK.Lib
{
    public enum Intersection
    {
        OUTSIDE = 0,
        INSIDE = 1,
        INTERSECT = 2
    }

    public class MOctreeSceneManager : MSceneManager
    {
        protected static int intersect_call;
        protected MList<MOctreeNode> mVisible;
        protected MOctree mOctree;
        protected int mNumObjects;
        protected int mMaxDepth;
        protected MAxisAlignedBox mBox;
        protected bool mShowBoxes;
        protected bool mLoose;

        protected float[] mCorners;
        protected MMatrix4 mScaleFactor;
        //protected bool mIsCheckInVisible;   // 是否检查不可见

        public MOctreeSceneManager(string name)
            : base(name)
        {
            MAxisAlignedBox b = new MAxisAlignedBox(-60000, -60000, -60000, 60000, 60000, 60000);
            int depth = 8;
            mOctree = null;
            mVisible = new MList<MOctreeNode>();
            //mIsCheckInVisible = false;
            init(b, depth);
        }

        public MOctreeSceneManager(string name, MAxisAlignedBox box, int max_depth)
            : base(name)
        {
            mOctree = null;
            mVisible = new MList<MOctreeNode>();
            //mIsCheckInVisible = false;
            init(box, max_depth);
        }

        public void init(MAxisAlignedBox box, int depth)
        {
            mOctree = new MOctree(null);

            mMaxDepth = depth;
            mBox = box;

            mOctree.mBox = box;
            MVector3 min = box.getMinimum();
            MVector3 max = box.getMaximum();

            mOctree.mHalfSize = (max - min) / 2;
            mShowBoxes = false;
            mNumObjects = 0;

            MVector3 v = new MVector3(1.5f, 1.5f, 1.5f);
            mScaleFactor = new MMatrix4(0);
            mScaleFactor.setScale(ref v);
        }

        override public MCamera createCamera(string name)
        {
            if (mCameras.ContainsKey(name))
            {
                // Error
            }

            MCamera c = new MOctreeCamera(name, this, null);
            mCameras.Add(name, c);

            return c;
        }

        override public void destroySceneNode(string name)
        {
            MOctreeNode on = (MOctreeNode)(getSceneNode(name));

            if (on != null)
                _removeOctreeNode(on);

            base.destroySceneNode(name);
        }

        public void _updateOctreeNode(MOctreeNode onode)
        {
            MAxisAlignedBox box = onode._getWorldAABB();

            if (box.isNull())
                return;

            if (mOctree == null)
                return;

            if (onode.getOctant() == null)
            {
                if (!onode._isIn(mOctree.mBox))
                    mOctree._addNode(onode);
                else
                    _addOctreeNode(onode, mOctree);
                return;
            }

            if (!onode._isIn(onode.getOctant().mBox))
            {
                _removeOctreeNode(onode);

                if (!onode._isIn(mOctree.mBox))
                    mOctree._addNode(onode);
                else
                    _addOctreeNode(onode, mOctree);
            }
        }

        public void _removeOctreeNode(MOctreeNode n)
        {
            if (mOctree == null)
                return;

            MOctree oct = n.getOctant();

            if (oct != null)
            {
                oct._removeNode(n);
            }

            n.setOctant(null);
        }

        public void _addOctreeNode(MOctreeNode n, MOctree octant, int depth = 0)
        {
            if (mOctree == null)
                return;

            MAxisAlignedBox bx = n._getWorldAABB();

            if ((depth < mMaxDepth) && octant._isTwiceSize(bx))
            {
                int x = 0, y = 0, z = 0;
                octant._getChildIndexes(bx, ref x, ref y, ref z);

                if (octant.mChildren[x, y, z] == null)
                {
                    octant.mChildren[x, y, z] = new MOctree(octant);
                    MVector3 octantMin = octant.mBox.getMinimum();
                    MVector3 octantMax = octant.mBox.getMaximum();
                    MVector3 min, max;

                    if (x == 0)
                    {
                        min.x = octantMin.x;
                        max.x = (octantMin.x + octantMax.x) / 2;
                    }
                    else
                    {
                        min.x = (octantMin.x + octantMax.x) / 2;
                        max.x = octantMax.x;
                    }

                    if (y == 0)
                    {
                        min.y = octantMin.y;
                        max.y = (octantMin.y + octantMax.y) / 2;
                    }
                    else
                    {
                        min.y = (octantMin.y + octantMax.y) / 2;
                        max.y = octantMax.y;
                    }

                    if (z == 0)
                    {
                        min.z = octantMin.z;
                        max.z = (octantMin.z + octantMax.z) / 2;
                    }
                    else
                    {
                        min.z = (octantMin.z + octantMax.z) / 2;
                        max.z = octantMax.z;
                    }

                    octant.mChildren[x, y, z].mBox.setExtents(ref min, ref max);
                    octant.mChildren[x, y, z].mHalfSize = (max - min) / 2;
                }

                _addOctreeNode(n, octant.mChildren[x, y, z], ++depth);
            }
            else
            {
                octant._addNode(n);
            }
        }

        override public MSceneNode createSceneNodeImpl()
        {
            return new MOctreeNode(this);
        }

        override public MSceneNode createSceneNodeImpl(string name)
        {
            return new MOctreeNode(this, name);
        }

        override public void _updateSceneGraph(MCamera cam)
        {
            base._updateSceneGraph(cam);
            updateEntityWorldBox();
        }

        public void _alertVisibleObjects()
        {
            // Error
        }

        override public void _findVisibleObjects(MCamera cam)
        {
            mVisible.Clear();

            mNumObjects = 0;

            walkOctree((MOctreeCamera)(cam), mOctree);
        }

        public void walkOctree(MOctreeCamera camera, MOctree octant)
        {
            //Ctx.m_instance.m_logSys.log("walkOctree Enter", LogTypeId.eSceneCull);

            if (octant.numNodes() == 0)
            {
                //Ctx.m_instance.m_logSys.log("walkOctree Exit", LogTypeId.eSceneCull);
                return;
            }

            MOctreeCamera.Visibility v = MOctreeCamera.Visibility.NONE;

            if (octant == mOctree)
            {
                //Ctx.m_instance.m_logSys.log("walkOctree Root Octree", LogTypeId.eSceneCull);

                v = MOctreeCamera.Visibility.PARTIAL;
            }
            else
            {
                //Ctx.m_instance.m_logSys.log("walkOctree Child Octree Check Visible", LogTypeId.eSceneCull);
                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree World Min {0}", octant.mBox.getMinimum()), LogTypeId.eSceneCull);
                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree World Max {0}", octant.mBox.getMaximum()), LogTypeId.eSceneCull);

                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Entity Min {0}", octant.mEntityWorldBox.getMinimum()), LogTypeId.eSceneCull);
                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Entity Max {0}", octant.mEntityWorldBox.getMaximum()), LogTypeId.eSceneCull);

                MAxisAlignedBox box = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
                octant._getCullBounds(ref box);

                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Cull Min {0}", box.getMinimum()), LogTypeId.eSceneCull);
                //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Cull Max {0}", box.getMaximum()), LogTypeId.eSceneCull);

                v = camera.getVisibility(box);
            }

            // 如果可见
            if (v != MOctreeCamera.Visibility.NONE)
            {
                //Ctx.m_instance.m_logSys.log("walkOctree Child Octree Visible", LogTypeId.eSceneCull);

                if (mShowBoxes)
                {
                    //
                }

                bool vis = true;
                /*
                List<MOctreeNode>.Enumerator it = octant.mNodes.list().GetEnumerator();
                while (it.MoveNext())
                {
                    Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node", LogTypeId.eSceneCull);

                    MOctreeNode sn = it.Current;

                    if (v == MOctreeCamera.Visibility.PARTIAL)
                    {
                        Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node Check Visible", LogTypeId.eSceneCull);

                        MAxisAlignedBox tmp = sn._getWorldAABB();
                        FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
                        vis = camera.isVisible(ref tmp, ref plane);
                    }

                    if (vis)
                    {
                        Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node  Visible", LogTypeId.eSceneCull);

                        mNumObjects++;
                        sn._addToRenderQueue(camera, v);
                        mVisible.Add(sn);

                        if (mDisplayNodes)
                        {

                        }

                        if (sn.getShowBoundingBox() || mShowBoundingBoxes)
                        {

                        }
                    }
                    else
                    {
                        if (mIsCheckInVisible)
                        {
                            Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node No Visible", LogTypeId.eSceneCull);

                            sn._removeFromRenderQueue(camera);
                        }
                    }
                }
                */

                int idx = 0;
                int len = octant.mNodes.Count();
                MOctreeNode sn = null;
                while (idx < len)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node", LogTypeId.eSceneCull);

                    sn = octant.mNodes[idx];

                    if (v == MOctreeCamera.Visibility.PARTIAL)
                    {
                        //Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node Check Visible", LogTypeId.eSceneCull);
                        //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Min {0}", sn._getWorldAABB().getMinimum()), LogTypeId.eSceneCull);
                        //Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Max {0}", sn._getWorldAABB().getMaximum()), LogTypeId.eSceneCull);

                        MAxisAlignedBox tmp = sn._getWorldAABB();
                        FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
                        vis = camera.isVisible(ref tmp, ref plane);
                    }

                    if (vis)
                    {
                        //Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node  Visible", LogTypeId.eSceneCull);

                        mNumObjects++;
                        sn._addToRenderQueue(camera, v);
                        mVisible.Add(sn);

                        if (mDisplayNodes)
                        {

                        }

                        if (sn.getShowBoundingBox() || mShowBoundingBoxes)
                        {

                        }
                    }
                    //else
                    //{
                    //    if (mIsCheckInVisible)
                    //    {
                    //        Ctx.m_instance.m_logSys.log("walkOctree Child Octree Node No Visible", LogTypeId.eLogCommon);

                    //        sn._removeFromRenderQueue(camera);
                    //    }
                    //}

                    ++idx;
                }

                MOctree child;
                bool childfoundvisible = (v == MOctreeCamera.Visibility.FULL);
                if ((child = octant.mChildren[0, 0, 0]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 0, 0, 0", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 0, 0, 0", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[1, 0, 0]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 1, 0, 0", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 1, 0, 0", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[0, 1, 0]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 0, 1, 0", LogTypeId.eSceneCull);

                    walkOctree(camera, child);
                }

                if ((child = octant.mChildren[1, 1, 0]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 1, 1, 0", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 1, 1, 0", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[0, 0, 1]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 0, 0, 1", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 0, 0, 1", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[1, 0, 1]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 1, 0, 1", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 1, 0, 1", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[0, 1, 1]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 0, 1, 1", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 0, 1, 1", LogTypeId.eSceneCull);
                }

                if ((child = octant.mChildren[1, 1, 1]) != null)
                {
                    //Ctx.m_instance.m_logSys.log("walkOctree Enter Child 1, 1, 1", LogTypeId.eSceneCull);

                    walkOctree(camera, child);

                    //Ctx.m_instance.m_logSys.log("walkOctree Exit Child 1, 1, 1", LogTypeId.eSceneCull);
                }
            }
            //else
            //{
            //    if (mIsCheckInVisible)
            //    {
            //        walkOctreeHide(camera, octant);
            //    }
            //}

            //Ctx.m_instance.m_logSys.log("walkOctree Exit", LogTypeId.eSceneCull);
        }

        // 遍历隐藏树
        //public void walkOctreeHide(MOctreeCamera camera, MOctree octant)
        //{
        //    // 如果不可见，就隐藏所有不可见的内容
        //    /*
        //    List<MOctreeNode>.Enumerator it = octant.mNodes.list().GetEnumerator();

        //    while (it.MoveNext())
        //    {
        //        MOctreeNode sn = it.Current;
        //        sn._removeFromRenderQueue(camera);
        //    }
        //    */

        //    int idx = 0;
        //    int len = octant.mNodes.Count();
        //    while (idx < len)
        //    {
        //        octant.mNodes[idx]._removeFromRenderQueue(camera);
        //        ++idx;
        //    }

        //    MOctree child;
        //    if ((child = octant.mChildren[0, 0, 0]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[1, 0, 0]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[0, 1, 0]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[1, 1, 0]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[0, 0, 1]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[1, 0, 1]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[0, 1, 1]) != null)
        //        walkOctreeHide(camera, child);

        //    if ((child = octant.mChildren[1, 1, 1]) != null)
        //        walkOctreeHide(camera, child);
        //}

        public void _findNodes(MAxisAlignedBox t, ref MList<MSceneNode> list, MSceneNode exclude, bool full, MOctree octant)
        {
            if (!full)
            {
                MAxisAlignedBox obox = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);
                octant._getCullBounds(ref obox);

                Intersection isect = intersect(t, obox);

                if (isect == Intersection.OUTSIDE)
                    return;

                full = (isect == Intersection.INSIDE);
            }

            List<MOctreeNode>.Enumerator it = octant.mNodes.list().GetEnumerator();

            while (it.MoveNext())
            {
                MOctreeNode on = it.Current;

                if (on != exclude)
                {
                    if (full)
                    {
                        list.Add(on);
                    }

                    else
                    {
                        Intersection nsect = intersect(t, on._getWorldAABB());

                        if (nsect != Intersection.OUTSIDE)
                        {
                            list.Add(on);
                        }
                    }
                }
            }

            MOctree child = null;

            if ((child = octant.mChildren[0, 0, 0]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[1, 0, 0]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[0, 1, 0]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[1, 1, 0]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[0, 0, 1]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[1, 0, 1]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[0, 1, 1]) != null)
                _findNodes(t, ref list, exclude, full, child);

            if ((child = octant.mChildren[1, 1, 1]) != null)
                _findNodes(t, ref list, exclude, full, child);
        }

        public void findNodesIn(MAxisAlignedBox box, ref MList<MSceneNode> list, MSceneNode exclude)
        {
            _findNodes(box, ref list, exclude, false, mOctree);
        }

        public void resize(MAxisAlignedBox box)
        {
            MList<MSceneNode> nodes = new MList<MSceneNode>();
            List<MSceneNode>.Enumerator it;

            _findNodes(mOctree.mBox, ref nodes, null, true, mOctree);

            mOctree = new MOctree(null);
            mOctree.mBox = box;

            MVector3 min = box.getMinimum();
            MVector3 max = box.getMaximum();
            mOctree.mHalfSize = (max - min) * 0.5f;

            it = nodes.list().GetEnumerator();

            while (it.MoveNext())
            {
                MOctreeNode on = (MOctreeNode)(it.Current);
                on.setOctant(null);
                _updateOctreeNode(on);
            }
        }

        override public void clearScene()
        {
            base.clearScene();
            init(mBox, mMaxDepth);
        }

        public Intersection intersect(MAxisAlignedBox one, MAxisAlignedBox two)
        {
            MOctreeSceneManager.intersect_call++;

            if (one.isNull() || two.isNull()) return Intersection.OUTSIDE;
            if (one.isInfinite()) return Intersection.INSIDE;
            if (two.isInfinite()) return Intersection.INTERSECT;


            MVector3 insideMin = two.getMinimum();
            MVector3 insideMax = two.getMaximum();

            MVector3 outsideMin = one.getMinimum();
            MVector3 outsideMax = one.getMaximum();

            if (insideMax.x < outsideMin.x ||
                    insideMax.y < outsideMin.y ||
                    insideMax.z < outsideMin.z ||
                    insideMin.x > outsideMax.x ||
                    insideMin.y > outsideMax.y ||
                    insideMin.z > outsideMax.z)
            {
                return Intersection.OUTSIDE;
            }

            bool full = (insideMin.x > outsideMin.x &&
                          insideMin.y > outsideMin.y &&
                          insideMin.z > outsideMin.z &&
                          insideMax.x < outsideMax.x &&
                          insideMax.y < outsideMax.y &&
                          insideMax.z < outsideMax.z);

            if (full)
                return Intersection.INSIDE;
            else
                return Intersection.INTERSECT;
        }

        public void updateEntityWorldBox()
        {
            if(mOctree.getEntityWorldBoxNeedUpdate())
            {
                mOctree.updateEntityWorldBox();
            }
        }
    }
}