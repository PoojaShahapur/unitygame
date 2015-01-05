﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.Msg;
using System;
using Game.UI;

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

        PackUICardItem m_packItem = new PackUICardItem();
        ItemSceneBase m_midCard = new ItemSceneBase();                // 中间卡牌
        GameObject[] m_openedCardHolderArr = new GameObject[5];     // 中间卡牌占位
        ItemSceneBase[] m_openedCardArr = new ItemSceneBase[5];       // 中间卡牌

        // Use this for initialization
        public override void Start()
        {
            //把完成设为假
            mok = transform.FindChild("openok");
            mok.gameObject.SetActive(false);

            abilitypre = Resources.Load("Card/opena") as GameObject;
            minionpre = Resources.Load("Card/openm") as GameObject;
            weaponpre = Resources.Load("Card/openw") as GameObject;

            m_packItem.m_tran = UtilApi.GoFindChildByPObjAndName("open/openpack").transform;

            m_midCard.m_tran = UtilApi.GoFindChildByPObjAndName("open/cards").transform;

            int idx = 0;
            while(idx < 5)
            {
                m_openedCardArr[idx] = new ItemSceneBase();
                m_openedCardArr[idx].m_tran = UtilApi.GoFindChildByPObjAndName("open/GO" + idx).transform;
                ++idx;
            }
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
            updateData();

            UpdateGoldText();
            animation["openup"].speed = 1;
            animation.Play("openup");
            //Camera.main.SendMessage("push");
            (Ctx.m_instance.m_interActiveEntityMgr.getSceneEntity("mcam") as boxcam).push();
        }

        // 更新数据
        public void updateData()
        {
            TableItemBase objitem;
            DataItemObjectBase bojBase;

            if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 0)
            {
                bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];
                objitem = bojBase.m_tableItemObject;
                m_packItem.bojBase = bojBase;
                m_packItem.m_path = (objitem.m_itemBody as TableObjectItemBody).path;
                m_packItem.m_prefab = (objitem.m_itemBody as TableObjectItemBody).m_prefab;
                m_packItem.m_clkCB += onBtnClkOpenCB;
                m_packItem.load();
            }
        }

        // 点击开一个包裹
        protected void onBtnClkOpenCB(ItemSceneIOBase packItem)
        { 
            // 发消息通知开
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = (packItem as PackUICardItem).bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);

            // 释放之前的资源
            m_midCard.m_prefab = packItem.m_prefab;
            m_midCard.m_path = packItem.m_path;

            m_midCard.load();
        }

        public void update5Card(params uint[] idList)
        {
            int idx = 0;
            TableItemBase tableitem;
            while (idx < 5)
            {
                tableitem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_CARD, idList[idx]);
                if(tableitem != null)
                {
                    m_openedCardArr[idx].m_prefab = (tableitem.m_itemBody as TableCardItemBody).m_prefab;
                    m_openedCardArr[idx].m_path = (tableitem.m_itemBody as TableCardItemBody).path;
                }

                m_openedCardArr[idx].load();

                ++idx;
            }
        }
    }
}