using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 主控制
    /// </summary>
    public class BoxCam : InterActiveEntity
    {
        public Text goldtext, expacktext;
        public static GameObject msgbox;
        public static GameObject ynmsgbox;
        public GameObject box;

        // Use this for initialization
        public override void Start()
        {
            //得到用户的信息
            msgbox = transform.FindChild("yesmsgbox").gameObject;
            ynmsgbox = transform.FindChild("yesnomsgbox").gameObject;

            box = UtilApi.GoFindChildByPObjAndName("box");
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void push()
        {
            animation["boxcampush"].speed = 1;
            animation.Play("boxcampush");
#if UNITY_5
		    box.GetComponent<Animation>()["boxopendoor"].speed = 1;
            box.GetComponent<Animation>().Play("boxopendoor");
#elif UNITY_4_6
            box.animation["boxopendoor"].speed = 1;
            box.animation.Play("boxopendoor");
#endif
        }

        public void back()
        {
            animation["boxcampush"].speed = -1;
            animation["boxcampush"].time = animation["boxcampush"].length;
            animation.Play("boxcampush");

#if UNITY_5
            box.GetComponent<Animation>()["boxopendoor"].speed = -1;
            box.GetComponent<Animation>()["boxopendoor"].time = box.GetComponent<Animation>()["boxopendoor"].length;
            box.GetComponent<Animation>().Play("boxopendoor");
#elif UNITY_4_6
            box.animation["boxopendoor"].speed = -1;
            box.animation["boxopendoor"].time = box.animation["boxopendoor"].length;
            box.animation.Play("boxopendoor");
#endif
        }
    }
}