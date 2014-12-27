using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 所有界面按钮的行为
    /// </summary>
    public class btn : InterActiveEntity
    {
        public bool Disable = false;
        GameObject mlight;

        // Use this for initialization
        public override void Start()
        {
            Transform t = transform.FindChild("light");
            if (t != null)
            {
                mlight = t.gameObject;
            }

            if (Disable)
            {
                return;
            }
        }

        void OnMouseEnter()
        {
            if (Disable)
            {
                return;
            }
            if (mlight != null)
            {
                mlight.SetActive(true);
            }

            if (animation != null)
            {
                animation.Play("btnanim");
            }
        }

        void OnMouseExit()
        {
            if (Disable)
            {
                return;
            }
            if (mlight != null)
            {
                mlight.SetActive(false);
            }
        }

        //向上一级发送
        public bool sendtoparent = false;
        public override void OnMouseUp()
        {
            if (Disable)
            {
                return;
            }
            (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).show();
        }

        void OnClick()
        {
            Debug.Log("空按钮函数");
        }
    }
}