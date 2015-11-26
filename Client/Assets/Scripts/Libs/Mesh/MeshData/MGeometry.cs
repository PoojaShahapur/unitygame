namespace SDK.Lib
{
    /**
     * @brief 基本的 Geometry
     */
    public class MGeometry
    {
        protected MList<MSubGeometryBase> _subGeometries;

        public void addSubGeometry(MSubGeometryBase subGeometry)
        {
            _subGeometries.push(subGeometry);
            subGeometry.setParentGeometry(this);
        }

        public void invalidateBounds(MISubGeometry subGeom)
		{

		}
    }
}