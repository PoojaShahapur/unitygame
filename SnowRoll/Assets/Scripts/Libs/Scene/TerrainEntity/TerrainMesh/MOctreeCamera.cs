using UnityEngine;

namespace SDK.Lib
{
    public class MOctreeCamera : MCamera
    {
        public enum Visibility
        {
            NONE,
            PARTIAL,
            FULL
        }

        public MOctreeCamera(string name, MSceneManager sm, Transform trans)
            : base(trans)//: base(name, sm )
        {

        }

        public Visibility getVisibility(MAxisAlignedBox bound)
        {
            if (bound.isNull())
                return Visibility.NONE;

            MVector3 centre = bound.getCenter();
            MVector3 halfSize = bound.getHalfSize();

            bool all_inside = true;
            MPlane.Side side;

            for (int plane = 0; plane < 6; ++plane)
            {
                if (plane == (int)FrustumPlane.FRUSTUM_PLANE_FAR && mFarDist == 0)
                    continue;

                side = getFrustumPlane((short)plane).getSide(ref centre, ref halfSize);
                if (side == MPlane.Side.NEGATIVE_SIDE) return Visibility.NONE;
                if (side == MPlane.Side.BOTH_SIDE)
                    all_inside = false;
            }

            if (all_inside)
                return Visibility.FULL;
            else
                return Visibility.PARTIAL;
        }
    }
}