namespace SDK.Lib
{
    public class MazeOp
    {
        protected MazeRoom m_curMazeRoom;
        protected bool m_bStart;

        public MazeOp()
        {
            m_curMazeRoom = null;
            m_bStart = false;
        }

        public MazeRoom curMazeRoom
        {
            get
            {
                return m_curMazeRoom;
            }
            set
            {
                m_curMazeRoom = value;
            }
        }

        public bool bStart
        {
            get
            {
                return m_bStart;
            }
            set
            {
                m_bStart = value;
            }
        }
    }
}