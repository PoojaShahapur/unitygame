using UnityEngine;

namespace SDK.Lib
{
    public enum eMazePtType
    {
        eCom,
        eStart,
        eEnd,
        eBomb,
        eDie,
        eStart_Jump,
        eStart_Show,
        eStart_Door,
        eEnd_Jump,
        eEnd_Hide,
        eEnd_Door,
        eEnd_Die,
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

        virtual public MazePtBase clone()
        {
            return null;
        }

        virtual public void copyFrom(MazePtBase rh)
        {
            this.m_ptType = rh.m_ptType;
            this.m_pos = rh.m_pos;
        }
    }
}