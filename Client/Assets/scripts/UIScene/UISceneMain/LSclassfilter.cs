﻿using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.UI;
using SDK.Lib;

namespace Game.UI
{
    /// <summary>
    /// 用来过滤职业
    /// </summary>
    public class classfilter : InterActiveEntity
    {
        bool isup = false;

        static EnPlayerCareer nowclass;
        EnPlayerCareer myclass;
        Vector3 lastpostion = new Vector3();

        // Use this for initialization
        public override void Start()
        {
            switch (name)
            {
                //case "sq": myclass = CardClass.kpaladin;
                //    break;
                //case "dly": myclass = CardClass.kdruid;
                //    break;
                case "zs": myclass = EnPlayerCareer.HERO_OCCUPATION_1;
                    break;
                case "ss": myclass = EnPlayerCareer.HERO_OCCUPATION_2;
                    break;
                case "fs": myclass = EnPlayerCareer.HERO_OCCUPATION_3; OnMouseUpAsButton();//显示第1个
                    break;
                //case "dz": myclass = CardClass.krogue;
                //    break;
                //case "ms": myclass = CardClass.kpriest;
                //    break;
                //case "lr": myclass = CardClass.khunter;
                //    break;
                //case "sm": myclass = CardClass.kshama;
                //    break;
            }
        }

        void Update()
        {
            if (nowclass == myclass && !isup)
            {
                //up
                iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3.forward * 0.1f,
                                                       "space", Space.World,
                                                       "time", 0.1f));
                //变大
                // iTween.ScaleBy(gameObject, Vector3.one * 1.2f, 0.1f);
                isup = true;
            }
            if (nowclass != myclass && isup)
            {
                //down
                iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3.forward * -0.1f,
                                                       "space", Space.World,
                                                       "time", 0.1f));
                //变 小
                // iTween.ScaleTo(gameObject, Vector3.one * 0.0254f, 0.1f);
                isup = false;
            }
        }

        void OnMouseUpAsButton()
        {
            if (!isup)
            {
                nowclass = myclass;
                //transform.root.FindChild("page").SendMessage("onclass", myclass);
                IUISceneWDSC uiSC = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneWDSC) as IUISceneWDSC;
                uiSC.onclass(myclass);
            }
        }

        void classdown(string sendname)
        {
            // nowup = sendname;
            if (sendname == name)
            {
                return;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.05055332f);
            isup = false;
        }

        public void classfilterhide(EnPlayerCareer c)
        {
            lastpostion = transform.localPosition;
            if (lastpostion != transform.localPosition)
            {
                Debug.Log(lastpostion);
            }

            if (myclass == EnPlayerCareer.HERO_OCCUPATION_1)
            {
                gototwo();
                return;
            }
            if (myclass != c)
            {
                chide();
            }
            else
            {
                gotoone();
            }
        }

        void chide()
        {
            transform.Translate(Vector3.forward * 10, Space.World);
        }

        void gototwo()
        {
            transform.localPosition = new Vector3(-1.594784f, transform.localPosition.y, transform.localPosition.z);
        }

        void gotoone()
        {
            transform.localPosition = new Vector3(-2.006191f, transform.localPosition.y, transform.localPosition.z);
            OnMouseUpAsButton();
        }

        public void gotoback()
        {
            transform.localPosition = new Vector3(lastpostion.x, 0.07995506f, 0.05055332f);
            if (nowclass == myclass)
            {
                isup = false;
                OnMouseUpAsButton();
            }
        }
    }
}