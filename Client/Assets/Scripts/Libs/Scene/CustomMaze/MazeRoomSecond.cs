using SDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace SDK.Lib
{
    public class MazeRoomSecond : MazeRoom
    {
        public MazeRoomSecond(int iTag_)
            : base(iTag_)
        {
            
        }

        override public void initWayPtList()
        {
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
                    if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        m_ptListArr[pathIdx] = new MList<MazePtBase>();
                    }
                }
            }
        }

        override public void init()
        {
            for (int pathIdx = 0; pathIdx < (int)ePathIndex.eTotal; ++pathIdx)
            {
                if ((int)ePathIndex.eABC == pathIdx)
                {
                    buildPathSecond(pathIdx);
                }
                else if ((int)ePathIndex.eACB == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eBAC == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eBCA == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eCAB == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                }
                else if ((int)ePathIndex.eCBA == pathIdx)
                {
                    if ((int)eRoomIndex.eB == m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                    else if ((int)eRoomIndex.eStart != m_fixIdx)
                    {
                        buildPathSecond(pathIdx);
                    }
                }
            }
        }

        protected void buildPathSecond(int pathIdx)
        {
            MazePtBase pt = null;
            int emptyCount = 0;
            List<string> wayPtSuffix = new List<string>();
            wayPtSuffix.Add("");
            wayPtSuffix.Add("_Start");
            wayPtSuffix.Add("_Start_Jump");
            wayPtSuffix.Add("_Start_Show");
            wayPtSuffix.Add("_Start_Door");
            wayPtSuffix.Add("_End_Jump");
            wayPtSuffix.Add("_End_Hide");
            wayPtSuffix.Add("_End_Door");
            wayPtSuffix.Add("_End_Die");

            // 最多 10 个点
            for (int idx = 0; idx < 10; ++idx)
            {
                emptyCount = 0;

                for (int suffixIdx = 0; suffixIdx < wayPtSuffix.Count; ++suffixIdx)
                {
                    pt = createWayPtByGo(pathIdx, idx, wayPtSuffix[suffixIdx]);
                    if(null == pt)
                    {
                        ++emptyCount;
                    }
                    else
                    {
                        m_ptListArr[pathIdx].Add(pt);
                        break;
                    }
                }

                if(emptyCount == wayPtSuffix.Count)
                {
                    break;
                }
            }
        }

        protected MazePtBase createWayPtByGo(int pathIdx, int idx, string suffix)
        {
            MazePtBase pt = null;
            string path = "";
            GameObject go_ = null;

            path = string.Format("WayPt_{0}{1}{2}", pathIdx, idx, suffix);
            go_ = UtilApi.TransFindChildByPObjAndPath(this.selfGo, path);
            if (null != go_)
            {
                if ("_Start" == suffix)
                {
                    pt = new MazeStartPt();
                }
                else if ("_Start_Jump" == suffix)
                {
                    pt = new MazeStartJumpPt();
                }
                else if ("_Start_Show" == suffix)
                {
                    pt = new MazeStartShowPt();
                }
                else if ("_Start_Door" == suffix)
                {
                    pt = new MazeStartDoorPt();
                }
                else if ("_End_Jump" == suffix)
                {
                    pt = new MazeEndJumpPt();
                }
                else if ("_End_Hide" == suffix)
                {
                    pt = new MazeEndHidePt();
                }
                else if ("_End_Door" == suffix)
                {
                    pt = new MazeEndDoorPt();
                }
                else if ("_End_Die" == suffix)
                {
                    pt = new MazeEndDiePt();
                }
                else
                {
                    pt = new MazeComPt();
                }

                pt.pos = go_.transform.localPosition;
            }

            return pt;
        }
    }
}