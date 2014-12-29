using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;

namespace SDK.Lib
{
    /// <summary>
    /// 开包控制
    /// </summary>
    public class open : InterActiveEntity
    {
        //3种卡的预制
        public GameObject abilitypre, minionpre, weaponpre;
        //出来卡的5个点
        public Transform[] cardpostions;
        //开出来的卡
        Transform[] opendcard = new Transform[5];
        //包数;
        public Text goldtext;
        Transform mok;

        // Use this for initialization
        public override void Start()
        {
            //把完成设为假
            mok = transform.FindChild("openok");
            mok.gameObject.SetActive(false);

            abilitypre = Resources.Load("Card/opena") as GameObject;
            minionpre = Resources.Load("Card/openm") as GameObject;
            weaponpre = Resources.Load("Card/openw") as GameObject;
        }

        Transform CreateCard(card a)
        {
            Transform ret = null;
            switch (a.type)
            {
                case CardType.kability:
                    {
                        ret = (Transform)UtilApi.Instantiate(abilitypre.transform);
                    }
                    break;

                case CardType.kminion:
                    {
                        ret = (Transform)UtilApi.Instantiate(minionpre.transform);
                    }
                    break;

                case CardType.kweapon:
                    {
                        ret = (Transform)UtilApi.Instantiate(weaponpre.transform);
                    }
                    break;

                case CardType.khero:
                    {
                        ret = (Transform)UtilApi.Instantiate(minionpre.transform);
                    }
                    break;

                case CardType.kheroPower:
                    {
                        Debug.LogError("出现了英雄技能卡");
                    }
                    break;
            }
            ret.name = a.cardid;

            ret.SendMessage("setinfo", a);
            return ret;
        }

        void openpack()
        {
            if (!canopen)
            {
                return;
            }

            //open
            //List<card> cards = web.openPack();
            List<card> cards = null;
            //实例化
            int p = 0;
            foreach (card c in cards)
            {
                Transform a = CreateCard(c);
                opendcard[p] = a;
                a.parent = transform;
                a.position = cardpostions[p].position;
                a.Rotate(0, 0, 180f, Space.Self);
                p++;
            }
            nowopencount = 0;
            canopen = false;
            canback = false;
            UpdateGoldText();
        }

        void UpdateGoldText()
        {
            //goldtext.text = web.player.expack.ToString();
        }

        bool canopen = true;
        int nowopencount = 0;
        void openonecard()
        {
            nowopencount++;
            if (nowopencount == 5)
            {
                mok.gameObject.SetActive(true);
            }
        }

        void openokz()
        {
            //清掉...
            canopen = true;
            foreach (Transform t in opendcard)
            {
                UtilApi.Destroy(t.gameObject);
            }
            mok.gameObject.SetActive(false);
            transform.FindChild("openpack").SendMessage("gotoback");
            canback = true;
        }

        bool canback = true;
        public void goback()
        {
            if (canback)
            {
                AnimationState a = animation["openup"];
                a.speed = -1;
                a.time = a.length;
                animation.Play("openup");

                //Camera.main.SendMessage("back");
                (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).back();
            }
        }

        public void show()
        {
            UpdateGoldText();
            animation["openup"].speed = 1;
            animation.Play("openup");
            //Camera.main.SendMessage("push");
            (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).push();
        }
    }
}