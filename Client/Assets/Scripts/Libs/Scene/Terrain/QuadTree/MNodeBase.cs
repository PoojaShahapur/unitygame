using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 场景结点 Base
     */
    public class MNodeBase
    {
        protected MNodeBase m_parent;               // Parent 结点
		protected MList<MNodeBase> m_childNodes;    // Child 结点列表
		protected int m_numChildNodes;             // Child 结点的数量
		
		protected int m_numEntities;                // Entity 数量
		
		/**
		 * @brief 构造函数
		 */
		public MNodeBase()
        {
            m_childNodes = new MList<MNodeBase>();
        }
		
		/**
		 * @brief 父节点。如果这个 Node ，说明这个 Node 是 Root 结点
		 */
		public MNodeBase getParent()
		{
			return m_parent;
		}

        public void setParent(MNodeBase parent)
        {
            m_parent = parent;
        }

        public int getNumEntities()
        {
            return m_numEntities;
        }

        public void setNumEntities(int value)
        {
            m_numEntities = value;
        }

        public void incNumEntities(int value)
        {
            m_numEntities += value;
        }

        public void decNumEntities(int value)
        {
            m_numEntities -= value;
        }

        /**
		 * @brief 添加一个节点
		 */
        public void addNode(MNodeBase node)
		{
			node.setParent(this);
			m_numEntities += node.getNumEntities();
            m_childNodes[m_numChildNodes++] = node;

            // 更新场景图中 Entity 的数量
            int numEntities = node.getNumEntities();
            node = this;

            do
            {
                node.incNumEntities(numEntities);
            }
            while ((node = node.getParent()) != null);
        }

        /**
         * @brief 移除一个 Node
         */
        public void removeNode(MNodeBase node)
		{
            m_childNodes.Remove(node);

            int numEntities = node.getNumEntities();
			node = this;

            do
            {
                node.decNumEntities(numEntities);
            }
            while ((node = node.getParent()) != null);
        }

        /**
         * @brief 判断 Node 是否在六面体中
         */
        virtual public bool isInFrustum(MList<MPlane3D> planes, int numPlanes)
		{
			return true;
		}
		
		/**
		 * @brief Node 是否与 Ray 相交
		 */
		public bool isIntersectingRay(Vector3 rayPosition, Vector3 rayDirection)
		{
			return true;
		}
		
		/**
		 * @brief 查找包含当前 Entity 的 Node
		 */
		virtual public MNodeBase findPartitionForEntity()
		{
			return this;
        }
		
		protected void updateNumEntities(int value)
		{
            int diff = value - m_numEntities;
            MNodeBase node = this;

            do
            {
                node.incNumEntities(diff);
            }
            while ((node = node.getParent()) != null);
        }
    }
}