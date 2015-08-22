using SDK.Common;
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
            m_mazeRoomList.Clear();
            m_itemCount = count_;
            MazeRoom mazeRoom = null;
            string path = "";
            for(int idx = 0; idx < m_itemCount; ++idx)
            {
                if (Ctx.m_instance.m_maze.mazeData.bInFisrstScene())
                {
                    mazeRoom = new MazeRoom(idx);
                }
                else
                {
                    mazeRoom = new MazeRoomSecond(idx);
                }
                m_mazeRoomList.Add(mazeRoom);
                path = string.Format("RootGo/Plane_{0}", idx);
                mazeRoom.selfGo = UtilApi.GoFindChildByPObjAndName(path);
                if (0 != idx && 4 != idx)
                {
                    mazeRoom.mazeIOControl.enableDrag();
                }
                mazeRoom.init();
            }

            path = "RootGo/SplitGo";
            m_trans = UtilApi.GoFindChildByPObjAndName(path).transform;

            adjustInitState();
        }

        // 调整最初的状态， B 和 C 交换一下
        public void adjustInitState()
        {
            int srcIdx = 2;
            int destIdx = 3;

            MazeRoom srcMazeRoom = m_mazeRoomList[srcIdx];
            MazeRoom destMazeRoom = m_mazeRoomList[destIdx];

            destMazeRoom.iTag = srcMazeRoom.iTag;
            Vector3 origPos = destMazeRoom.origPos;
            destMazeRoom.origPos = srcMazeRoom.origPos;
            srcMazeRoom.iTag = destIdx;
            srcMazeRoom.origPos = origPos;

            Ctx.m_instance.m_maze.mazeData.roomInfo.updateRoomList();

            srcMazeRoom.mazeIOControl.disableDragTitle();
            destMazeRoom.mazeIOControl.disableDragTitle();
            
            srcMazeRoom.mazeRoomTrackAniControl.goToDestPos();
            destMazeRoom.mazeRoomTrackAniControl.goToDestPos();

            srcMazeRoom.mazeIOControl.enableDragTitle();
            destMazeRoom.mazeIOControl.enableDragTitle();
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

            //if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eFirst)
            //{
                for (int idx = 0; idx < 5; ++idx)
                {
                    m_mazeRoomList[idx].getWayPtList(ptList, pathIdx);
                }
            //}
            //else
            //{
            //    for (int idx = 0; idx < 1; ++idx)
            //    {
            //        m_mazeRoomList[idx].getWayPtList(ptList, pathIdx);
            //    }
            //}
        }

        static public int sortRoom(MazeRoom lh, MazeRoom rh)
        {
            if(lh.iTag < rh.iTag)
            {
                return -1;
            }

            return 1;
        }

        static public int sortFixRoom(MazeRoom lh, MazeRoom rh)
        {
            if (lh.fixIdx < rh.fixIdx)
            {
                return -1;
            }

            return 1;
        }

        public void showLightWin()
        {
            (m_mazeRoomList[4] as MazeRoomSecond).showLightWin();
        }

        public void showDarkWin()
        {
            (m_mazeRoomList[4] as MazeRoomSecond).showDarkWin();
        }

        public void resetInitState()
        {
            m_mazeRoomList.list.Sort(sortFixRoom);      // 排序
            for(int idx = 0; idx < m_mazeRoomList.Count(); ++idx)
            {
                m_mazeRoomList[idx].iTag = idx;

                m_mazeRoomList[idx].mazeIOControl.disableDragTitle();
                m_mazeRoomList[idx].mazeRoomTrackAniControl.goToDestPos();
                m_mazeRoomList[idx].mazeIOControl.enableDragTitle();
            }
            adjustInitState();
        }
    }
}