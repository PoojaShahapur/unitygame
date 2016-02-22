using UnityEngine;

namespace SDK.Lib
{
    /**
     * @brief 坐标系
     */
    public class Coordinate
    {
        virtual public float getX()
        {
            return 0;
        }

        virtual public float getY()
        {
            return 0;
        }

        virtual public float getZ()
        {
            return 0;   
        }

        // 增加 theta
        virtual public void incTheta(float deltaDegree)
        {
            
        }

        // 减少 theta
        virtual public void decTheta(float deltaDegree)
        {
            
        }

        virtual public void setParam(float radius, float theta, float fai)
        {

        }

        virtual public void convCartesian2Spherical()
        {

        }

        virtual public void convSpherical2Cartesian()
        {

        }

        virtual public void syncTrans(Transform trans)
        {
            
        }

        virtual public void updateCoord()
        {

        }
    }
}