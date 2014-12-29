using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 主控制
    /// </summary>
    public class boxcam : InterActiveEntity
    {
        public Text goldtext, expacktext;
        public static GameObject msgbox;
        public static GameObject ynmsgbox;
        public GameObject box;

        // Use this for initialization
        public override void Start()
        {
            //得到用户的信息
            UpdateGandEx();
            msgbox = transform.FindChild("yesmsgbox").gameObject;
            ynmsgbox = transform.FindChild("yesnomsgbox").gameObject;

            box = UtilApi.GoFindChildByPObjAndName("box");
        }

        // Update is called once per frame
        void Update()
        {
        }

        void UpdateGandEx()
        {

        }

        void showMsgbox(string bt, string text)
        {
            msgbox.SendMessage("show", bt + "/" + text);
        }

        public void push()
        {
            animation["boxcampush"].speed = 1;
            animation.Play("boxcampush");
            box.animation["boxopendoor"].speed = 1;
            box.animation.Play("boxopendoor");
        }

        public void back()
        {
            UpdateGandEx();
            animation["boxcampush"].speed = -1;
            animation["boxcampush"].time = animation["boxcampush"].length;
            animation.Play("boxcampush");

            box.animation["boxopendoor"].speed = -1;
            box.animation["boxopendoor"].time = box.animation["boxopendoor"].length;
            box.animation.Play("boxopendoor");
        }

        void noopen()
        {
            showMsgbox("提示", "没有开放!现在只开放'打开扩展包'");
        }
    }
}