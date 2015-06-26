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
            if (capacity > needSize)        // 使用 > ，不适用 >= ，浪费一个自己，方便判断
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

                if(ret < needSize)      // 分配失败
                {
                    Ctx.m_instance.m_logSys.error(string.Format("分配字节缓冲区失败，不能分配 {0} 自己的缓冲区", needSize));
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
        static protected void splitPos(int up, Transform trans, float inter, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            if (splitCnt > 3)        // 只有大于 3 个的时候才分割
            {
                if (up == 0)
                {
                    upHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                }
                else
                {
                    downHemisphereSplit(trans, radius, splitCnt, ref posList, ref rotList);
                }
            }
            else
            {
                rectSplit(trans, inter, splitCnt, ref posList, ref rotList);
            }
        }

        static protected void rectSplit(Transform trans, float inter, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            float totalLen = splitCnt * inter;
            float startPos = -totalLen / 2;

            Vector3 pos;
            Quaternion rot;

            int listIdx = 0;
            while(listIdx < splitCnt)
            {
                pos.x = trans.localPosition.x + startPos + inter * listIdx;
                pos.y = trans.localPosition.y;
                pos.z = trans.localPosition.z;
                posList.Add(pos);

                rot = trans.localRotation;
                rotList.Add(rot);

                ++listIdx;
            }
        }

        // 180 - 360 度区间
        static protected void upHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            //float radianSector = 0;         // 每一个弧形的弧度
            float degSector = 0;            // 度
            //float curRadian = 0;
            float curDeg = 0;

            //float startRadian = 0;          // 开始的弧度
            float startDeg = 0;             // 开始的角度

            float yDelta = 0.4f;

            Vector3 pos;
            Quaternion rot;

            // 总共 10 张牌
            //radianSector = Mathf.PI / 11;           // 这个地方需要加 1 
            degSector = 180 / 11;

            //startRadian = Mathf.PI + radianSector;
            startDeg = 180 + degSector;

            Vector3 orign = new Vector3(radius, 0, 0);

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                //curRadian = startRadian + radianSector * listIdx;
                curDeg = startDeg + degSector * listIdx;

                pos = new Vector3();
                pos = Quaternion.AngleAxis(curDeg, Vector3.up) * orign;
                pos += trans.localPosition;
                pos.y += listIdx * yDelta;

                posList.Add(pos);

                rot = Quaternion.Euler(0, curDeg + trans.eulerAngles.y + 90, 0);            // +90 就是为了是竖直的变成水平的，起始都偏移 90 度，这样就可以认为是 0 度了
                rotList.Add(rot);

                ++listIdx;
            }
        }

        // 0 - 180 度区间
        static protected void downHemisphereSplit(Transform trans, float radius, int splitCnt, ref List<Vector3> posList, ref List<Quaternion> rotList)
        {
            //float radianSector = 0;         // 每一个弧形的弧度
            float degSector = 0;            // 度
            //float curRadian = 0;
            float curDeg = 0;

            //float startRadian = 0;          // 开始的弧度
            float startDeg = 0;             // 开始的角度

            float yDelta = 0.4f;

            Vector3 pos;
            Quaternion rot;

            // 总共 10 张牌
            //radianSector = Mathf.PI / 11;           // 这个地方需要加 1 
            degSector = 180 / 11;

            //startRadian = Mathf.PI - radianSector;
            startDeg = 180 - degSector;

            Vector3 orign = new Vector3(radius, 0, 0);

            int listIdx = 0;
            while (listIdx < splitCnt)
            {
                //curRadian = startRadian - radianSector * listIdx;
                curDeg = startDeg - degSector * listIdx;

                pos = new Vector3();
                pos = Quaternion.AngleAxis(curDeg, Vector3.up) * orign;
                pos += trans.localPosition;
                pos.y += listIdx * yDelta;

                posList.Add(pos);

                rot = Quaternion.Euler(0, curDeg, 0);            // +90 就是为了是竖直的变成水平的，起始都偏移 90 度，这样就可以认为是 0 度了
                rotList.Add(rot);

                ++listIdx;
            }
        }

        static public float xzDis(Vector3 aPos, Vector3 bPos)
        {
            return Mathf.Sqrt((aPos.x - bPos.x) * (aPos.x - bPos.x) + (aPos.z - bPos.z) * (aPos.z - bPos.z));
        }

        /**
         * @param trans 起始位置
         * @param unitWidth 单元宽度
         * @param areaWidth 区域宽度
         * @param splitCnt 划分数量
         * @param posList 返回的位置列表
         */
        static public void newRectSplit(Transform trans, float unitWidth, float areaRadius, float fYDelta, int splitCnt, ref List<Vector3> posList)
        {
            Vector3 pos;
            int listIdx = 0;
            float halfUnitWidth = unitWidth / 2;
            if (unitWidth * splitCnt > 2 * areaRadius)       // 如果当前区域不能完整放下所有的单元
            {
                float plusOneWidth = (areaRadius * 2) - unitWidth;          // 最后一个必然要放在最后一个，并且不能超出边界
                float splitCellWidth = plusOneWidth / (splitCnt - 1);
                while (listIdx < splitCnt - 1)  // 最后一个位置左边界就是 plusOneWidth ，已经计算好了
                {
                    pos.x = trans.localPosition.x + splitCellWidth * listIdx - areaRadius;  // 这个是左边的位置
                    pos.x += halfUnitWidth;           // 调整中心点的位置
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }

                // 计算最后一个位置
                pos.x = trans.localPosition.x + plusOneWidth - areaRadius;  // 这个是左边的位置
                pos.x += halfUnitWidth;           // 调整中心点的位置
                pos.y = trans.localPosition.y + fYDelta * listIdx;
                pos.z = trans.localPosition.z;
                posList.Add(pos);
            }
            else            // 全部能放下，就居中显示
            {
                float halfWidth = (float)((unitWidth * splitCnt) * 0.5);        // 占用的区域的一半宽度
                while (listIdx < splitCnt)
                {
                    pos.x = trans.localPosition.x + unitWidth * listIdx - halfWidth;    // 这个是左边的位置
                    pos.x += halfUnitWidth;           // 调整中心点的位置
                    pos.y = trans.localPosition.y + fYDelta * listIdx;
                    pos.z = trans.localPosition.z;
                    posList.Add(pos);

                    ++listIdx;
                }
            }
        }

        // 设置状态为
        static public void setState(StateID stateID, params byte[] stateBytes)
        {
            if((int)stateID/8 < stateBytes.Length)
            {
                stateBytes[(int)stateID/8] |= ((byte)(1 << ((int)stateID % 8)));
            }
        }

        static public void clearState(StateID stateID, params byte[] stateBytes)
        {
            if ((int)stateID/8 < stateBytes.Length)
            {
                stateBytes[(int)stateID / 8] &= (byte)(~((byte)(1 << ((int)stateID % 8))));
            }
        }

        static public bool checkState(StateID stateID, params byte[] stateBytes)
        {
            if((int)stateID/8 < stateBytes.Length)
            {
                return ((stateBytes[(int)stateID / 8] & ((byte)(1 << ((int)stateID % 8)))) > 0);
            }

            return false;
        }

        static public void clearState(AttackTarget stateID, uint state)
        {
            if ((uint)stateID < uint.MaxValue)
            {
                state &= (byte)(~((byte)(1 << ((int)stateID % 8))));
            }
        }

        static public bool checkAttackState(AttackTarget stateID, uint state)
        {
            if ((uint)stateID < uint.MaxValue)
            {
                return ((state & (uint)stateID) > 0);
            }

            return false;
        }

        static public void setState(int idx, ref byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                stateData |= ((byte)(1 << idx));
            }
        }

        static public void clearState(int idx, ref byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                stateData &= ((byte)(~(1 << idx)));
            }
        }

        static public bool checkState(int idx, byte stateData)
        {
            if (idx < sizeof(byte) * 8)
            {
                return ((stateData & (1 << idx)) > 0);
            }

            return false;
        }
    }
}