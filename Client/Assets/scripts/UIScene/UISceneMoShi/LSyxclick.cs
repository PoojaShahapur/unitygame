﻿using UnityEngine;
using System.Collections;
using SDK.Common;
using Game.UI;
using SDK.Lib;

namespace Game.UI
{
    public class yxclick : InterActiveEntity
    {
        EnPlayerCareer myclass;
        Material classpic;
        // Use this for initialization
        public override void Start()
        {
            classpic = transform.FindChild("pic").renderer.material;
            switch (name)
            {
                case "圣骑士": myclass = EnPlayerCareer.HERO_OCCUPATION_1;
                    break;
                case "德鲁伊": myclass = EnPlayerCareer.HERO_OCCUPATION_2;
                    break;
                case "战士": myclass = EnPlayerCareer.HERO_OCCUPATION_3;
                    break;
                //case "术士": myclass = EnPlayerCareer.kwarlock;
                //    break;

                //case "法师": myclass = EnPlayerCareer.kmage;
                //    break;
                //case "潜行者": myclass = EnPlayerCareer.krogue;
                //    break;
                //case "牧师": myclass = EnPlayerCareer.kpriest;
                //    break;
                //case "猎人": myclass = EnPlayerCareer.khunter;
                //    break;

                //case "萨满祭司": myclass = EnPlayerCareer.kshama;
                //    break;
            }

            UtilApi.addEventHandle(gameObject, onMouseUp);
        }

        protected void onMouseUp(GameObject go)
        {
            OnMouseUpAsButton();
        }

        public void OnMouseUpAsButton()
        {
            //transform.root.SendMessage("setclass", myclass);
            //transform.root.SendMessage("setclasspic", classpic);
            //transform.root.SendMessage("setClassname", name);

            IUISceneMoShi uiMS = Ctx.m_instance.m_uiSceneMgr.getSceneUI(UISceneFormID.eUISceneMoShi) as IUISceneMoShi;
            if (uiMS != null)
            {
                uiMS.setclass(myclass);
                uiMS.setclasspic(classpic);
                uiMS.setClassname(name);
            }
        }
    }
}