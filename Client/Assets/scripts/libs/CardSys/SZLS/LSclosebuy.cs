using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class closebuy : InterActiveEntity
    {
        public override void OnMouseUpAsButton()
        {
            //transform.parent.SendMessage("close");
            (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).close();
        }
    }
}