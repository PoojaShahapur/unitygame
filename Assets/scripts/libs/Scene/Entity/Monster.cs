using UnityEngine;

namespace SDK.Lib
{
    class Monster : BeingEntity
    {
        public Monster()
            : base()
        {
            m_skinAniModel.m_modelList = new GameObject[(int)MonstersModelDef.eModelTotal];
        }
    }
}