using SDK.Common;

namespace SDK.Lib
{
    public class MazeRoom : AuxComponent
    {
        protected int m_iTag;
        protected MazeIOControl m_mazeIOControl;

        public MazeRoom()
        {
            m_mazeIOControl = new MazeIOControl(this);
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
    }
}