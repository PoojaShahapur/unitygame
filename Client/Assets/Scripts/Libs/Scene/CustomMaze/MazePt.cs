using UnityEngine;

namespace SDK.Lib
{
    public enum eMazePtType
    {
        eCom,
        eStart,
        eEnd
    }

    public class MazePt
    {
        protected eMazePtType m_ptType;
        protected Vector3 m_pos;

        public MazePt(eMazePtType type_ = eMazePtType.eCom)
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
    }
}