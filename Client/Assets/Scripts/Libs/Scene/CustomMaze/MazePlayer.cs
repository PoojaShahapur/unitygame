using SDK.Common;

namespace SDK.Lib
{
    public class MazePlayer : AuxComponent
    {
        protected MList<MazePt> m_ptList;

        public MazePlayer()
        {
            m_ptList = new MList<MazePt>();
        }

        public MList<MazePt> ptList
        {
            get
            {
                return m_ptList;
            }
            set
            {
                m_ptList = value;
            }
        }

        public void init()
        {
            string path = "";
            this.selfGo = UtilApi.GoFindChildByPObjAndName(path);
        }

        public void setStartPos()
        {
            this.selfGo.transform.localPosition = m_ptList[0].pos;
        }

        public void startMove()
        {
            
        }
    }
}
