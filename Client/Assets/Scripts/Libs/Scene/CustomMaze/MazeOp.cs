namespace SDK.Lib
{
    public class MazeOp
    {
        protected MazeRoom m_curMazeRoom;

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
    }
}