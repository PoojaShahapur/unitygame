using SDK.Lib;

namespace SDK.Lib
{
    public class Maze
    {
        protected MazeData m_mazeData;

        public Maze()
        {
            m_mazeData = new MazeData();
        }

        public void init()
        {
            m_mazeData.init();
        }

        public MazeData mazeData
        {
            get
            {
                return m_mazeData;
            }
            set
            {
                m_mazeData = value;
            }
        }
    }
}