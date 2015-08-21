using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public enum eRoomIndex
    {
        eStart,
        eA,
        eB,
        eC,
        eTotal
    }

    public class MazeRoom : AuxComponent
    {
        protected int m_fixIdx;     // 固定不变的索引
        protected int m_iTag;
        protected MazeIOControl m_mazeIOControl;
        protected MazeRoomTrackAniControl m_mazeRoomTrackAniControl;
        protected Vector3 m_origPos;
        protected MList<MazePtBase>[] m_ptListArr;

        public MazeRoom(int iTag_)
        {
            m_fixIdx = iTag_;
            m_iTag = iTag_;
            m_mazeIOControl = new MazeIOControl(this);
            m_mazeRoomTrackAniControl = new MazeRoomTrackAniControl(this);
            m_ptListArr = new MList<MazePtBase>[(int)ePathIndex.eTotal];

            for (int pathIdx = 0; pathIdx < (int)ePathIndex.eTotal; ++pathIdx)
            {
                if ((int)ePathIndex.eABC == pathIdx)
                {
                    m_ptListArr[pathIdx] = new MList<MazePtBase>();
                }
                else if ((int)ePathIndex.eACB == pathIdx)
                {
                    if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
                else if ((int)ePathIndex.eBAC == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
                else if ((int)ePathIndex.eBCA == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
                else if ((int)ePathIndex.eCAB == pathIdx)
                {
                    if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
                else if ((int)ePathIndex.eCBA == pathIdx)
                {
                    if ((int)eRoomIndex.eStart != m_fixIdx && (int)eRoomIndex.eA != m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
            }
        }

        public int fixIdx
        {
            get
            {
                return m_fixIdx;
            }
            set
            {
                m_fixIdx = value;
            }
        }

        public int iTag
        {
            get
            {
                return m_iTag;
            }
            set
            {
                m_iTag = value;
            }
        }

        public MazeIOControl mazeIOControl
        {
            get
            {
                return m_mazeIOControl;
            }
            set
            {
                m_mazeIOControl = value;
            }
        }

        public Vector3 origPos
        {
            get
            {
                return m_origPos;
            }
            set
            {
                m_origPos = value;
            }
        }

        public MazeRoomTrackAniControl mazeRoomTrackAniControl
        {
            get
            {
                return m_mazeRoomTrackAniControl;
            }
            set
            {
                m_mazeRoomTrackAniControl = value;
            }
        }

        override protected void onSelfChanged()
        {
            m_origPos = new Vector3(selfGo.transform.localPosition.x, selfGo.transform.localPosition.y, selfGo.transform.localPosition.z);
        }

        public void init()
        {
            for (int pathIdx = 0; pathIdx < (int)ePathIndex.eTotal; ++pathIdx)
            {
                if ((int)ePathIndex.eABC == pathIdx)
                {
                    buildPathPt4f(pathIdx);
                }
                else if ((int)ePathIndex.eACB == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathPt6f(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        buildPathPt4f(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eBAC == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathPt6f(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eBCA == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathPt6f(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eCAB == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathPt6f(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        buildPathPt4f(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eCBA == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathPt6f(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx && (int)eRoomIndex.eA != m_fixIdx)
                    {
                        buildPathPt4f(pathIdx);
                    }
                }
            }
        }

        protected void buildPathPt4f(int pathIdx)
        {
            MazePtBase pt = null;
            string path = "";

            for (int idx = 0; idx < 4; ++idx)
            {
                if (0 == idx)
                {
                    pt = new MazeStartPt();
                }
                else if (3 == idx)
                {
                    pt = new MazeEndPt();
                }
                else
                {
                    pt = new MazeComPt();
                }
                m_ptListArr[pathIdx].Add(pt);

                path = string.Format("WayPt_{0}{1}", pathIdx, idx);
                pt.pos = UtilApi.TransFindChildByPObjAndPath(this.selfGo, path).transform.localPosition;
            }
        }

        protected void buildPathPt6f(int pathIdx)
        {
            MazePtBase pt = null;
            string path = "";

            for (int idx = 0; idx < 6; ++idx)
            {
                if (0 == idx)
                {
                    pt = new MazeStartPt();
                }
                else if (3 == idx)
                {
                    pt = new MazeEndPt();
                }
                else if (4 == idx)
                {
                    pt = new MazeBombPt();
                }
                else if (5 == idx)
                {
                    pt = new MazeDiePt();
                }
                else
                {
                    pt = new MazeComPt();
                }
                m_ptListArr[pathIdx].Add(pt);

                path = string.Format("WayPt_{0}{1}", pathIdx, idx);
                pt.pos = UtilApi.TransFindChildByPObjAndPath(this.selfGo, path).transform.localPosition;
            }
        }

        public void getWayPtList(MList<MazePtBase> ptList_, int pathIdx)
        {
            MazePtBase pt = null;
            MList<MazePtBase> list = null;

            if((int)eRoomIndex.eStart == m_fixIdx)
            {
                list = m_ptListArr[(int)ePathIndex.eABC];
            }
            else
            {
                list = m_ptListArr[pathIdx];
            }

            if (list != null)
            {
                for (int idx = 0; idx < list.Count(); ++idx)
                {
                    pt = list[idx].clone();
                    ptList_.Add(pt);

                    //pt.pos = Ctx.m_instance.m_maze.mazeData.sceneRootGo.transform.TransformPoint(m_ptList[idx].pos);
                    pt.pos = list[idx].pos + selfGo.transform.localPosition;
                }
            }
        }
    }
}