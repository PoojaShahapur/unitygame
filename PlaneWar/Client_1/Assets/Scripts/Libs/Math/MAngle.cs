namespace SDK.Lib
{
    public struct MAngle
    {
        public float mAngle;

        public MAngle(float angle)
        {
            mAngle = angle;
        }

        static public explicit operator MRadian(MAngle lhs)
        {
            return new MRadian(UtilMath.AngleUnitsToRadians(lhs.mAngle));
        }

        static public explicit operator MDegree(MAngle lhs)
        {
            return new MDegree(UtilMath.AngleUnitsToDegrees(lhs.mAngle));
        }
    }
}