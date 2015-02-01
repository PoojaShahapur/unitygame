using SDK.Lib;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Common
{
    public class UtilMath
    {
        static public bool isVisible(Camera camera, RectangleF box)
        {
            return false;
        }

        // 获取一个最近的大小
        static public uint getCloseSize(uint needSize, uint capacity, uint maxCapacity)
        {
            uint ret = 0;
            if (capacity >= needSize)
            {
                ret = capacity;
            }
            else
            {
                ret = 2 * capacity;
                while (ret < needSize && ret < maxCapacity)
                {
                    ret *= 2;
                }

                if (ret > maxCapacity)
                {
                    ret = maxCapacity;
                }
            }

            return ret;
        }

        /* 
         * @brief 上半球分割
         * @param inter 矩形分割的时候表示间隔
         * @param radius 半径
         * @param splitCnt 分裂多少分
         * @param posList 返回的位置列表
         * @param rotList 返回的旋转列表
         */
        static public void splitPos(int up, Transform trans, float inter, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            if (splitCnt > 3)        // 只有大于 3 个的时候才分割
            {
                //if (up == 0)
                //{
                    upHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                //}
                //else
                //{
                //    downHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                //}
            }
            else
            {
                rectSplit(trans, inter, splitCnt, ref posList, ref rotList);
            }
        }

        static public void rectSplit(Transform trans, float inter, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            float totalLen = splitCnt * inter;
            float startPos = -totalLen / 2;

            Vector3 pos;
            Quaternion rot;

            int listIdx = 0;
            while(listIdx < splitCnt)
            {
                pos = new Vector3();
                pos.x = trans.localPosition.x + startPos + inter * listIdx;
                pos.y = trans.localPosition.y;
                pos.z = trans.localPosition.z;
                posList.Add(pos);

                rot = trans.localRotation;
                rotList.Add(rot);

                ++listIdx;
            }
        }

        static public void upHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            float radianSector = 0;         // 每一个弧形的弧度
            float degSector = 0;            // 度
            float curRadian = 0;
            float curDeg = 0;

            float startRadian = 0;          // 开始的弧度
            float startDeg = 0;             // 开始的角度

            Vector3 pos;
            Quaternion rot;

            radianSector = (Mathf.PI / 2) / (splitCnt + 1);           // 这个地方需要加 1 
            degSector = 180 / (splitCnt + 1);

            startRadian = (float)(-radianSector * (splitCnt / 2 - 1 + 0.5));
            startDeg = (float)(-degSector * (splitCnt / 2 - 1 + 0.5));

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                curRadian = startRadian + radianSector * listIdx + (- Mathf.PI / 4);
                curDeg = startDeg + degSector * listIdx;

                pos = new Vector3();
                pos.x = trans.localPosition.x + radius * Mathf.Cos(curRadian);
                pos.y = trans.localPosition.y + radius * Mathf.Sin(curRadian);
                pos.z = trans.localPosition.z;
                posList.Add(pos);

                rot = Quaternion.Euler(0, curDeg + trans.eulerAngles.y, 0);
                rotList.Add(rot);

                ++listIdx;
            }
        }

        //// 下半球分割
        //static public void downHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        //{
        //    float radianSector = 0;         // 每一个弧形的弧度
        //    float degSector = 0;            // 度
        //    float curRadian = 0;
        //    float curDeg = 0;

        //    float startRadian = 0;          // 开始的弧度
        //    float startDeg = 0;             // 开始的角度

        //    Vector3 pos;
        //    Quaternion rot;

        //    radianSector = (Mathf.PI / 2) / (splitCnt + 1);           // 这个地方需要加 1 
        //    degSector = 180 / (splitCnt + 1);

        //    startRadian = (float)(-radianSector * (splitCnt / 2 - 1 + 0.5));
        //    startDeg = (float)(-degSector * (splitCnt / 2 - 1 + 0.5));

        //    int listIdx = 0;
        //    while (listIdx < splitCnt)
        //    {
        //        curRadian = startRadian + radianSector * listIdx + (-Mathf.PI / 4);
        //        curDeg = startDeg + degSector * listIdx;

        //        pos = new Vector3();
        //        pos.x = trans.localPosition.x + radius * Mathf.Cos(curRadian);
        //        pos.y = trans.localPosition.y + radius * Mathf.Sin(curRadian);
        //        pos.z = trans.localPosition.z;
        //        posList.Add(pos);

        //        rot = Quaternion.Euler(0, curDeg + 180, 0);
        //        rotList.Add(rot);

        //        ++listIdx;
        //    }
        //}

        static public float xzDis(Vector3 aPos, Vector3 bPos)
        {
            return Mathf.Sqrt((aPos.x - bPos.x) * (aPos.x - bPos.x) + (aPos.z - bPos.z) * (aPos.z - bPos.z));
        }
    }
}