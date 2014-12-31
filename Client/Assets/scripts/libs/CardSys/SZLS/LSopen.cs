using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SDK.Common;
using Game.Msg;

namespace SDK.Lib
{
    // 显示的一项
    public class PackItem
    {
        // 包裹中的内容，只显示两行
        public Transform m_tran = null;                // 第一个位置
        public GameObject m_go = null;                 // 显示的内容
        public string m_prefab;                        // 预制名字
        public string m_path;                          // 目录
        public DataItemObjectBase bojBase;             // 道具类

        public void onloaded(IDispatchObject resEvt)            // 资源加载成功
        {
            IResItem res = resEvt as IResItem;
            m_go = res.InstantiateObject(m_prefab);
            m_go.transform.parent = m_tran;
            UtilApi.normalPosScale(m_go.transform);

            addEventHandle();
        }

        public void unload()
        {
            if (m_go != null)
            {
                UtilApi.Destroy(m_go);
                m_go = null;
                Ctx.m_instance.m_resMgr.unload(m_path);
            }
        }

        // 添加事件监听
        protected void addEventHandle()
        {
            UtilApi.addEventHandle(m_go, onBtnClkBuy);
        }

        // 点击开包按钮
        protected void onBtnClkBuy(GameObject go)
        {
            stUseObjectPropertyUserCmd cmd = new stUseObjectPropertyUserCmd();
            cmd.qwThisID = bojBase.m_srvItemObject.dwThisID;
            cmd.useType = 1;
            UtilMsg.sendMsg(cmd);
        }
    };

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

        PackItem m_packItem = new PackItem();

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
            m_packItem.unload();

            TableItemBase objitem;
            DataItemObjectBase bojBase;
            LoadParam param;

            if (Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList.Count > 0)
            {
                bojBase = Ctx.m_instance.m_dataPlayer.m_dataPack.m_objList[0];
                objitem = bojBase.m_tableItemObject;
                m_packItem.bojBase = bojBase;
                m_packItem.m_path = (objitem.m_itemBody as TableObjectItemBody).path;
                m_packItem.m_prefab = (objitem.m_itemBody as TableObjectItemBody).m_prefab;
                param = Ctx.m_instance.m_resMgr.getLoadParam();
                param.m_path = m_packItem.m_path;
                param.m_prefabName = m_packItem.m_prefab;
                param.m_loaded = m_packItem.onloaded;
                Ctx.m_instance.m_resMgr.loadResources(param);
            }
        }
    }
}