using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.Msg;

namespace SDK.Lib
{
    /// <summary>
    /// 所有界面按钮的行为
    /// </summary>
    public class SceneBtnBase : InterActiveEntity
    {
        public bool m_bDisable = false;
        GameObject m_light;

        // Use this for initialization
        public override void Start()
        {
            Transform t = transform.FindChild("light");
            if (t != null)
            {
                m_light = t.gameObject;
            }

            if (m_bDisable)
            {
                return;
            }
        }

        public void OnMouseEnter()
        {
            if (m_bDisable)
            {
                return;
            }
            if (m_light != null)
            {
                m_light.SetActive(true);
            }

            if (animation != null)
            {
                animation.Play("btnanim");
            }
        }

        public void OnMouseExit()
        {
            if (m_bDisable)
            {
                return;
            }
            if (m_light != null)
            {
                m_light.SetActive(false);
            }
        }
    }
}