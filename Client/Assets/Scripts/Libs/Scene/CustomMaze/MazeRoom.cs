using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    public class MazeRoom : AuxComponent
    {
        protected int m_iTag;
        protected MazeIOControl m_mazeIOControl;
        protected MazeTrackAniControl m_mazeTrackAniControl;
        protected Vector3 m_origPos;

        public MazeRoom(int iTag_)
        {
            m_iTag = iTag_;
            m_mazeIOControl = new MazeIOControl(this);
            m_mazeTrackAniControl = new MazeTrackAniControl(this);
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
    }
}