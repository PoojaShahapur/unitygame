using UnityEngine;
using System.Collections;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 牌店里单选框
    /// </summary>
    public class shopbtn : InterActiveEntity
    {
        static GameObject nowChoose;
        GameObject mlight;

        // Use this for initialization
        void Awake()
        {
            mlight = transform.FindChild("light").gameObject;

            if (name == "btn1")
            {
                OnMouseUpAsButton();
            }
        }

        void Update()
        {
            if (nowChoose == gameObject && !mlight.activeSelf)
            {
                lightup();
            }
            else if (nowChoose != gameObject && mlight.activeSelf)
            {
                dark();
            }
        }

        // Update is called once per frame
        public override void OnMouseUpAsButton()
        {
            switch (name)
            {
                case "btn1":
                    {
                        setbt("1包专家级扩展包");
                        //transform.parent.parent.SendMessage("showpack", 1);
                        (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).showpack(1);
                    }
                    break;
                case "btn2":
                    {
                        setbt("2包专家级扩展包");
                        //transform.parent.parent.SendMessage("showpack", 2);
                        (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).showpack(2);
                    }
                    break;
                case "btn7":
                    {
                        setbt("7包专家级扩展包");
                        //transform.parent.parent.SendMessage("showpack", 7);
                        (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).showpack(7);
                    }
                    break;
                case "btn15":
                    {
                        setbt("15包专家级扩展包");
                        //transform.parent.parent.SendMessage("showpack", 15);
                        (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).showpack(15);
                    }
                    break;
                case "btn40":
                    {
                        setbt("40包专家级扩展包");
                        //transform.parent.parent.SendMessage("showpack", 40);
                        (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("shop") as shop).showpack(40);
                    }
                    break;
            }
            nowChoose = gameObject;
        }

        void setbt(string bt)
        {
            //transform.parent.parent.FindChild("text").FindChild("bt").GetComponent<UILabel>().text = bt;
        }

        void lightup()
        {
            mlight.SetActive(true);
        }

        void dark()
        {
            mlight.SetActive(false);
        }
    }
}