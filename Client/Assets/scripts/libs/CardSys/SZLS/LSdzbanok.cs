using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    public class dzbanok : InterActiveEntity
    {
        void OnMouseUpAsButton()
        {
            //Camera.main.SendMessage("replace");
            Ctx.m_instance.m_camSys.m_dzcam.replace();
            transform.parent.gameObject.SetActive(false);
        }
    }
}