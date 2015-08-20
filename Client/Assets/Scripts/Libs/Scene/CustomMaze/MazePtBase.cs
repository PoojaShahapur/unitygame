using UnityEngine;

namespace SDK.Lib
{
    public enum eMazePtType
    {
        eCom,
        eStart,
        eEnd,
        eDie,
        eTotal
    }

    public class MazePtBase
    {
        protected eMazePtType m_ptType;
        protected Vector3 m_pos;

        public MazePtBase(eMazePtType type_)
        {
            m_ptType = type_;
        }

        public eMazePtType ptType
        {
            get
            {
                return m_ptType;
            }
            set
            {
                m_ptType = value;
            }
        }

        public Vector3 pos
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        virtual public void moveToDestPos(MazePlayer mazePlayer_)
        {
            
        }
    }
}