namespace SDK.Lib
{
    /**
     * @brief 基本的 Geometry
     */
    public class MGeometry
    {
        protected MList<MSubGeometryBase> m_subGeometries;

        public MGeometry()
        {
            m_subGeometries = new MList<MSubGeometryBase>();
        }

        public void addSubGeometry(MSubGeometryBase subGeometry)
        {
            m_subGeometries.push(subGeometry);
            subGeometry.setParentGeometry(this);
        }

        public void invalidateBounds(MISubGeometry subGeom)
		{

		}
    }
}