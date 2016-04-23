using System.Collections.Generic;

namespace SDK.Lib
{
    public class MOctreeNode : MSceneNode
    {
        protected MAxisAlignedBox mLocalAABB;
        protected MOctree mOctant;
        protected float[] mCorners;

        public MOctreeNode(MSceneManager creator)
            : base(creator)
        {
            mOctant = null;
        }

        public MOctreeNode(MSceneManager creator, string name)
            : base(creator, name)
        {
            mOctant = null;
        }

        public void _removeNodeAndChildren()
        {
            ((MOctreeSceneManager)(mCreator))._removeOctreeNode(this);
            Dictionary<string, MNode>.Enumerator it = mChildren.GetEnumerator();
            while (it.MoveNext())
            {
                ((MOctreeNode)(it.Current.Value))._removeNodeAndChildren();
            }
        }

        override public MNode removeChild(ushort index)
        {
            MOctreeNode on = (MOctreeNode)(base.removeChild(index));
            on._removeNodeAndChildren();
            return on;
        }

        override public MNode removeChild(MNode child)
        {
            MOctreeNode on = (MOctreeNode)(base.removeChild(child));
            on._removeNodeAndChildren();
            return on;
        }

        override public void removeAllChildren()
        {
            foreach (KeyValuePair<string, MNode> i in mChildren)
            {
                MOctreeNode on = (MOctreeNode)(i.Value);
                on.setParent(null);
                on._removeNodeAndChildren();
            }
            mChildren.Clear();
            mChildrenToUpdate.Clear();
        }

        override public MNode removeChild(string name)
        {
            MOctreeNode on = (MOctreeNode)(base.removeChild(name));
            on._removeNodeAndChildren();
            return on;
        }

        override public void _updateBounds()
        {
            mWorldAABB.setNull();
            mLocalAABB.setNull();

            Dictionary<string, MMovableObject>.Enumerator i = mObjectsByName.GetEnumerator();
            MAxisAlignedBox bx = new MAxisAlignedBox(MAxisAlignedBox.Extent.EXTENT_FINITE);

            while (i.MoveNext())
            {
                bx = i.Current.Value.getBoundingBox();

                mLocalAABB.merge(bx);

                mWorldAABB.merge(i.Current.Value.getWorldBoundingBox(true));
            }

            if (!mWorldAABB.isNull() && mIsInSceneGraph)
            {
                ((MOctreeSceneManager)(mCreator))._updateOctreeNode(this);
            }
        }

        public bool _isIn(MAxisAlignedBox box)
        {
            if (!mIsInSceneGraph || box.isNull()) return false;

            if (box.isInfinite())
                return true;

            MVector3 min = mWorldAABB.getMinimum();
            MVector3 center = mWorldAABB.getMaximum().midPoint(ref min);

            MVector3 bmin = box.getMinimum();
            MVector3 bmax = box.getMaximum();

            bool centre = (bmax > center && bmin < center);
            if (!centre)
                return false;

            MVector3 octreeSize = bmax - bmin;
            MVector3 nodeSize = mWorldAABB.getMaximum() - mWorldAABB.getMinimum();
            return nodeSize < octreeSize;
        }

        public void _addToRenderQueue(MCamera cam, MOctreeCamera.Visibility visibleLevel)
        {
            MAxisAlignedBox tmp;
            FrustumPlane plane;

            /*
            foreach (MMovableObject mo in mObjectsByName.Values)
            {
                Ctx.m_instance.m_logSys.log("_addToRenderQueue", LogTypeId.eLogCommon);

                bool vis = true;
                tmp = mo.getWorldBoundingBox(false);
                plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
                if (visibleLevel == MOctreeCamera.Visibility.PARTIAL)
                {
                    Ctx.m_instance.m_logSys.log("_addToRenderQueue Check Visible", LogTypeId.eLogCommon);

                    Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Min {0}", tmp.getMinimum()), LogTypeId.eLogCommon);
                    Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Max {0}", tmp.getMaximum()), LogTypeId.eLogCommon);

                    vis = cam.isVisible(ref tmp, ref plane);
                }
                if(vis)
                {
                    Ctx.m_instance.m_logSys.log("_addToRenderQueue Show", LogTypeId.eLogCommon);

                    mo.show(cam);
                }
                //else
                //{
                //    Ctx.m_instance.m_logSys.log("_addToRenderQueue Hide", LogTypeId.eLogCommon);
                //    mo.hide(cam);
                //}
            }
            */

            int idx = 0;
            int len = mObjectsList.Count();
            MMovableObject mo;

            while (idx < len)
            {
                mo = mObjectsList[idx];
                Ctx.m_instance.m_logSys.log("_addToRenderQueue", LogTypeId.eLogCommon);

                bool vis = true;
                tmp = mo.getWorldBoundingBox(false);
                plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
                if (visibleLevel == MOctreeCamera.Visibility.PARTIAL)
                {
                    Ctx.m_instance.m_logSys.log("_addToRenderQueue Check Visible", LogTypeId.eLogCommon);

                    Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Min {0}", tmp.getMinimum()), LogTypeId.eLogCommon);
                    Ctx.m_instance.m_logSys.log(string.Format("walkOctree Child Octree Node Max {0}", tmp.getMaximum()), LogTypeId.eLogCommon);

                    vis = cam.isVisible(ref tmp, ref plane);
                }
                if (vis)
                {
                    Ctx.m_instance.m_logSys.log("_addToRenderQueue Show", LogTypeId.eLogCommon);

                    mo.show(cam);
                }
                //else
                //{
                //    Ctx.m_instance.m_logSys.log("_addToRenderQueue Hide", LogTypeId.eLogCommon);
                //    mo.hide(cam);
                //}

                ++idx;
            }
        }

        public void _removeFromRenderQueue(MCamera cam)
        {
            foreach (MMovableObject mo in mObjectsByName.Values)
            {
                //bool vis = false;
                //MAxisAlignedBox tmp = mo.getWorldBoundingBox(false);
                //FrustumPlane plane = FrustumPlane.FRUSTUM_PLANE_BOTTOM;
                //vis = cam.isVisible(ref tmp, ref plane);
                //if (vis)
                //{
                //    mo.show(cam);
                //}
                //else
                //{
                Ctx.m_instance.m_logSys.log("_removeFromRenderQueue", LogTypeId.eLogCommon);
                mo.hide(cam);
                //}
            }
        }

        public MOctree getOctant()
        {
            return mOctant;
        }

        public void setOctant(MOctree o)
        {
            mOctant = o;
        }

        public MAxisAlignedBox _getLocalAABB()
        {
            return mLocalAABB;
        }
    }
}