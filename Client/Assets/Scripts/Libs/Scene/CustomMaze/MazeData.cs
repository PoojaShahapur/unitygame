﻿namespace SDK.Lib
{
    public class MazeData
    {
        protected RoomInfo m_roomInfo;
        protected MazeIOControl m_mazeIOControl;
        protected MazeOp m_mazeOp;

        public MazeData()
        {
            m_roomInfo = new RoomInfo();
            m_mazeIOControl = new MazeIOControl();
            m_mazeOp = new MazeOp();
        }

        public RoomInfo roomInfo
        {
            get
            {
                return m_roomInfo;
            }
            set
            {
                m_roomInfo = value;
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

        public MazeOp mazeOp
        {
            get
            {
                return m_mazeOp;
            }
            set
            {
                m_mazeOp = value;
            }
        }

        public void init()
        {
            m_roomInfo.initMazeRoomCount(4);
        }
    }
}