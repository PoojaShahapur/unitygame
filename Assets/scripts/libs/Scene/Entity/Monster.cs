using SDK.Common;
using UnityEngine;

namespace SDK.Lib
{
    class Monster : BeingEntity
    {
        public Monster()
            : base()
        {
            m_skinAniModel.m_modelList = new PartInfo[(int)MonstersModelDef.eModelTotal];
        }
    }
}