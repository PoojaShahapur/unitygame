﻿using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public enum ePathIndex
    {
        eABC,
        eACB,
        eBAC,
        eBCA,
        eCAB,
        eCBA,
        eTotal
    }

    public class RoomInfo
    {
        protected MList<MazeRoom> m_mazeRoomList;   // 所有迷宫房间
        protected int m_itemCount;              // 迷宫中房间数量
        protected Transform m_trans;

        public RoomInfo()
        {
            m_mazeRoomList = new MList<MazeRoom>();
        }

        public MList<MazeRoom> mazeRoomList
        {
            get
            {
                return m_mazeRoomList;
            }
            set
            {
                m_mazeRoomList = value;
            }
        }

        public int itemCount
        {
            get
            {
                return m_itemCount;
            }
            set
            {
                m_itemCount = value;
            }
        }

        public void initMazeRoomCount(int count_)
        {
            m_itemCount = count_;
            MazeRoom mazeRoom = null;
            string path = "";
            for(int idx = 0; idx < m_itemCount; ++idx)
            {
                mazeRoom = new MazeRoom(idx);
                m_mazeRoomList.Add(mazeRoom);
                path = string.Format("RootGo/Plane_{0}", idx);
                mazeRoom.selfGo = UtilApi.GoFindChildByPObjAndName(path);
                if (0 != idx)
                {
                    mazeRoom.mazeIOControl.enableDrag();
                }
                mazeRoom.init();
            }

            path = "RootGo/SplitGo";
            m_trans = UtilApi.GoFindChildByPObjAndName(path).transform;
        }

        public int getRoomIdx(MazeRoom mazeRoom)
        {
            if (mazeRoom.selfGo.transform.localPosition.x <= m_trans.localPosition.x)     // 如果在左边
            {
                if (mazeRoom.selfGo.transform.localPosition.z >= m_trans.localPosition.z)    // 如果在上面
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            else                // 如果在右边
            {
                if (mazeRoom.selfGo.transform.localPosition.z >= m_trans.localPosition.z)    // 如果在上面
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
        }

        public MazeRoom getMazeRoom(int idx)
        {
            return m_mazeRoomList[idx];
        }

        public void updateRoomList()
        {
            m_mazeRoomList.list.Sort(sortRoom);
        }

        public void getWayPtList(MList<MazePtBase> ptList)
        {
            int pathIdx = 0;
            if ((int)eRoomIndex.eA == m_mazeRoomList[1].fixIdx)
            {
                if ((int)eRoomIndex.eB == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eABC;
                }
                else if((int)eRoomIndex.eC == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eACB;
                }
            }
            else if ((int)eRoomIndex.eB == m_mazeRoomList[1].fixIdx)
            {
                if ((int)eRoomIndex.eA == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eBAC;
                }
                else if ((int)eRoomIndex.eC == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eBCA;
                }
            }
            else if ((int)eRoomIndex.eC == m_mazeRoomList[1].fixIdx)
            {
                if ((int)eRoomIndex.eB == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eCBA;
                }
                else if ((int)eRoomIndex.eA == m_mazeRoomList[2].fixIdx)
                {
                    pathIdx = (int)ePathIndex.eCAB;
                }
            }

            for(int idx = 0; idx < 4; ++idx)
            {
                m_mazeRoomList[idx].getWayPtList(ptList, pathIdx);
            }
        }

        static public int sortRoom(MazeRoom lh, MazeRoom rh)
        {
            if(lh.iTag < rh.iTag)
            {
                return -1;
            }

            return 1;
        }
    }
}