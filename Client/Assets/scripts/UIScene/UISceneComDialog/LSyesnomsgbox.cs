using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Lib;
using System;
using SDK.Common;

namespace Game.UI
{
    /// <summary>
    /// 确认取消框控制,会回调一个bool参数,来确认用户的选择
    /// </summary>
    public class yesnomsgbox : InterActiveEntity
    {
        //回调的对象
        //public static GameObject callbackgameobject;
        //回调的函数名
        //public static string callbackeventname;
        public Action<bool> m_cb;
        Text t, bt;

        public override void Start()
        {
            transform.localScale = Vector3.one * 0.000001f;
            bt = transform.FindChild("bt").GetComponent<Text>();
            t = transform.FindChild("text").GetComponent<Text>();

            UtilApi.addEventHandle(UtilApi.TransFindChildByPObjAndPath(gameObject, "yesbtn"), onYesBtnClk);
            UtilApi.addEventHandle(UtilApi.TransFindChildByPObjAndPath(gameObject, "nobtn"), onNoBtnClk);
        }

        public void show(string str)
        {
            iTween.ScaleTo(gameObject, Vector3.one * 0.5f, 0.5f);
            if (str.IndexOf("/") == -1)
            {//直接显示
                //t.text = str;
            }
            else
            {//分解显示,标题与内容
                //string[] ttt = str.Split('/');
                //bt.text = ttt[0];
                //t.text = ttt[1];
            }
        }

        public void onYesBtnClk(GameObject go)
        {
            yes();
        }

        public void onNoBtnClk(GameObject go)
        {
            no();
        }

        void yes()
        {
            iTween.ScaleTo(gameObject, Vector3.one * 0.000001f, 0.5f);
            //callbackgameobject.SendMessage(callbackeventname, true);
            m_cb(true);
        }

        void no()
        {
            iTween.ScaleTo(gameObject, Vector3.one * 0.000001f, 0.5f);
            //callbackgameobject.SendMessage(callbackeventname, false);
            m_cb(false);
        }
    }
}