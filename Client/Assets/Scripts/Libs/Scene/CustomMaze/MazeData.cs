namespace SDK.Lib
{
    public class MazeData
    {
        protected RoomInfo m_roomInfo;
        protected MazeOp m_mazeOp;
        protected MazePlayer m_mazePlayer;

        public MazeData()
        {
            m_roomInfo = new RoomInfo();
            m_mazeOp = new MazeOp();
            m_mazePlayer = new MazePlayer();
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
            m_mazePlayer.init();
        }

        public void getWayPtList()
        {
            m_mazePlayer.ptList.Clear();
            m_roomInfo.getWayPtList(m_mazePlayer.ptList);
        }

        public void setStartPos()
        {
            m_mazePlayer.setStartPos();
        }

        public void startMove()
        {
            m_mazePlayer.startMove();
        }
    }
}