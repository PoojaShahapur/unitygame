using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 四叉树结点
     */
    public class MQuadTreeNode : MNodeBase
    {
        protected float m_centerX;
        protected float m_centerZ;
        protected float m_depth;
        protected bool m_leaf;
        protected float m_height;

        protected MQuadTreeNode m_rightFar;
        protected MQuadTreeNode m_leftFar;
        protected MQuadTreeNode m_rightNear;
        protected MQuadTreeNode m_leftNear;

        protected float m_halfExtentXZ;
        protected float m_halfExtentY;

        public MQuadTreeNode(int maxDepth = 5, float size = 10000, float height = 1000000, float centerX = 0, float centerZ = 0, int depth = 0)
        {
            float hs = size * 0.5f;

            m_centerX = centerX;
            m_centerZ = centerZ;
            m_height = height;
            m_depth = depth;
            m_halfExtentXZ = size * 0.5f;
            m_halfExtentY = height * 0.5f;

            m_leaf = depth == maxDepth;

            if (!m_leaf)
            {
                float hhs = hs * 0.5f;
                addNode(m_leftNear = new MQuadTreeNode(maxDepth, hs, height, centerX - hhs, centerZ - hhs, depth + 1));
                addNode(m_rightNear = new MQuadTreeNode(maxDepth, hs, height, centerX + hhs, centerZ - hhs, depth + 1));
                addNode(m_leftFar = new MQuadTreeNode(maxDepth, hs, height, centerX - hhs, centerZ + hhs, depth + 1));
                addNode(m_rightFar = new MQuadTreeNode(maxDepth, hs, height, centerX + hhs, centerZ + hhs, depth + 1));
            }
        }

        // todo: fix to infinite height so that height needn't be passed in constructor
        override public bool isInFrustum(MList<MPlane3D> planes, int numPlanes)
		{
			for (int i = 0; i < numPlanes; ++i) 
            {
                MPlane3D plane = planes[i];
				float flippedExtentX = plane.m_a< 0? - m_halfExtentXZ : m_halfExtentXZ;
                float flippedExtentY = plane.m_b < 0? - m_halfExtentY : m_halfExtentY;
                float flippedExtentZ = plane.m_c < 0? - m_halfExtentXZ : m_halfExtentXZ;
                float projDist = plane.m_a * (m_centerX + flippedExtentX) + plane.m_b * flippedExtentY + plane.m_c * (m_centerZ + flippedExtentZ) - plane.m_d;
                if (projDist < 0)
                {
                    return false;
                }
			}
			
			return true;
		}

        override public MNodeBase findPartitionForEntity()
        {
            MBoundingVolumeBase bounds = null;
            Vector3 min = bounds.getMin();
            Vector3 max = bounds.getMax();
            return findPartitionForBounds(min.x, min.z, max.x, max.z);
        }

        private MQuadTreeNode findPartitionForBounds(float minX, float minZ, float maxX, float maxZ)
		{
			bool left, right;
            bool far, near;

            if (m_leaf)
            {
                return this;
            }
			
			right = maxX > m_centerX;
            left = minX < m_centerX;
            far = maxZ > m_centerZ;
            near = minZ < m_centerZ;

            if (left && right)
            {
                return this;
            }
			
			if (near)
            {
                if (far)
                {
                    return this;
                }

                if (left)
                {
                    return m_leftNear.findPartitionForBounds(minX, minZ, maxX, maxZ);
                }
                else
                {
                    return m_rightNear.findPartitionForBounds(minX, minZ, maxX, maxZ);
                }
            }
            else
            {
                if (left)
                {
                    return m_leftFar.findPartitionForBounds(minX, minZ, maxX, maxZ);
                }
                else
                {
                    return m_rightFar.findPartitionForBounds(minX, minZ, maxX, maxZ);
                }
            }
        }
    }
}