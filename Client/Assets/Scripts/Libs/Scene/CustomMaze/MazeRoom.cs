using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class MazeRoom : AuxComponent
    {
        protected int m_fixIdx;     // 固定不变的索引
        protected int m_iTag;
        protected MazeIOControl m_mazeIOControl;
        protected MazeTrackAniControl m_mazeTrackAniControl;
        protected Vector3 m_origPos;
        protected MList<MazePt> m_ptList;

        public MazeRoom(int iTag_)
        {
            m_fixIdx = iTag_;
            m_iTag = iTag_;
            m_mazeIOControl = new MazeIOControl(this);
            m_mazeTrackAniControl = new MazeTrackAniControl(this);
            m_ptList = new MList<MazePt>();
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

        public MazeTrackAniControl mazeTrackAniControl
        {
            get
            {
                return m_mazeTrackAniControl;
            }
            set
            {
                m_mazeTrackAniControl = value;
            }
        }

        override protected void onSelfChanged()
        {
            m_origPos = new Vector3(selfGo.transform.localPosition.x, selfGo.transform.localPosition.y, selfGo.transform.localPosition.z);
        }

        public void init()
        {
            MazePt pt = null;
            string path = "";

            for(int idx = 0; idx < 4; ++idx)
            {
                if(0 == idx)
                {
                    pt = new MazeStartPt();
                }
                else if(3 == idx)
                {
                    pt = new MazeEndPt();
                }
                else
                {
                    pt = new MazePt();
                }
                m_ptList.Add(pt);

                path = string.Format("WayPt_{0}", idx);
                pt.pos = UtilApi.TransFindChildByPObjAndPath(this.selfGo, path).transform.localPosition;
            }
        }

        public void getWayPtList(MList<MazePt> ptList_)
        {
            MazePt pt = null;

            for(int idx = 0; idx < 4; ++idx)
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
                    pt = new MazePt();
                }
                ptList_.Add(pt);

                pt.pos = m_ptList[idx].pos + selfGo.transform.InverseTransformPoint(m_ptList[idx].pos);
            }
        }
    }
}