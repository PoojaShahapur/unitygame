namespace SDK.Lib
{
    /**
     * @brief 基本的 Geometry
     */
    public class MTestGeometry
    {
        protected MList<MTestSubGeometryBase> m_subGeometries;

        public MTestGeometry()
        {
            m_subGeometries = new MList<MTestSubGeometryBase>();
        }

        public void addSubGeometry(MTestSubGeometryBase subGeometry)
        {
            m_subGeometries.push(subGeometry);
            subGeometry.setParentGeometry(this);
        }

        public void invalidateBounds(MITestSubGeometry subGeom)
		{

		}
    }
}