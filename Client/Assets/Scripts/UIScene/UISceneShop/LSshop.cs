﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    // 显示的一项
    public class ShopItem
    {
        // 商店中的内容，只显示两行
        public Transform m_tran = null;                // 第一个位置
        public GameObject m_go = null;                 // 显示的内容
        public string m_path;                          // 目录

        public void onLoaded(IDispatchObject resEvt)            // 资源加载成功
        {
            IResItem res = resEvt as IResItem;
            m_go = res.InstantiateObject(m_path);
            m_go.transform.parent = m_tran;
            UtilApi.normalPosScale(m_go.transform);
        }

        public void unload()
        {
            if(m_go != null)
            {
                UtilApi.Destroy(m_go);
                m_go = null;
                Ctx.m_instance.m_resLoadMgr.unload(m_path);
            }
        }
    };

    /// <summary>
    /// 商店控制
    /// </summary>
    public class shop : InterActiveEntity
    {
        protected const int TOTALITEM = 2;
        protected const int TOTALPACK = 4;

        Transform pack1, pack2, pack7, pack15, pack40, buykuan;
        //Text rmb;

        //ShopItem[] m_shopItemArray = new ShopItem[TOTALITEM];

        Text[] m_shopPriceText = new Text[TOTALPACK];
        Text m_shopGoldNum;

        // Use this for initialization
        public override void Awake()
        {
            transform.localScale = new Vector3(0.00001f, 0.00001f, 0.00001f);
        }

        public override void Start()
        {
            //int idx = 0;
            //while(idx < TOTALITEM)
            //{
            //    m_shopItemArray[idx] = new ShopItem();
            //    ++idx;
            //}
            
            //m_shopItemArray[0].m_tran = UtilApi.GoFindChildByPObjAndName("mcam/shop/FirstItem").transform;
            //m_shopItemArray[1].m_tran = UtilApi.GoFindChildByPObjAndName("mcam/shop/SecondItem").transform;

            for (int i = 0; i < TOTALPACK; i++)
            {
                string textName = string.Format("mcam/shop/background/packprice{0}", i + 1);
                m_shopPriceText[i] = UtilApi.getComByP<Text>(textName);
            }
            m_shopGoldNum = UtilApi.getComByP<Text>("mcam/shop/background/goldnum");
        }

        public void close()
        {
            animation["showbuy"].speed = -1;
            animation["showbuy"].time = animation["showbuy"].length;
            animation.Play("showbuy");
        }

        public void show()
        {
            animation["showbuy"].speed = 1;
            animation.Play("showbuy");

            updateShopData();
        }

        int nowpacknum;
        float nowmoney = 0;
        public void showpack(int num)
        {
            if (pack1 == null)
            {
                pack1 = transform.FindChild("1pack");
                pack2 = transform.FindChild("2pack");
                pack7 = transform.FindChild("7pack");
                pack15 = transform.FindChild("15pack");
                pack40 = transform.FindChild("40pack");
                //rmb = transform.FindChild("buykuan").FindChild("text").FindChild("rmb").GetComponent<Text>();
            }

            switch (num)
            {
                case 1:
                    nowpacknum = 1;
                    pack1.gameObject.SetActive(true);
                    pack2.gameObject.SetActive(false);
                    pack7.gameObject.SetActive(false);
                    pack15.gameObject.SetActive(false);
                    pack40.gameObject.SetActive(false);
                    nowmoney = 100;
                    break;
                case 2:
                    nowpacknum = 2;
                    pack2.gameObject.SetActive(true);
                    pack7.gameObject.SetActive(false);
                    pack15.gameObject.SetActive(false);
                    pack40.gameObject.SetActive(false);
                    //rmb.text = "￥2.99";
                    nowmoney = 2.99f;
                    break;
                case 7:
                    nowpacknum = 7;
                    pack7.gameObject.SetActive(true);
                    pack15.gameObject.SetActive(false);
                    pack40.gameObject.SetActive(false);
                    //rmb.text = "￥9.99";
                    nowmoney = 9.99f;
                    break;
                case 15:
                    nowpacknum = 15;
                    pack15.gameObject.SetActive(true);
                    pack40.gameObject.SetActive(false);
                    //rmb.text = "￥19.99";
                    nowmoney = 19.99f;
                    break;
                case 40: pack40.gameObject.SetActive(true);
                    nowpacknum = 40;
                    //rmb.text = "￥49.99";
                    nowmoney = 49.99f;
                    break;
            }
            Rotationbuykuan();
        }

        bool isrmb = false;
        void Rotationbuykuan()
        {
            if (buykuan == null)
            {
                buykuan = transform.FindChild("buykuan");
            }

            if (nowpacknum != 1 && !isrmb)
            {
                //转到钱
                buykuan.Rotate(new Vector3(0, 0, 180));
                isrmb = true;
            }
            if (nowpacknum == 1 && isrmb)
            {
                //转到金子
                buykuan.Rotate(new Vector3(0, 0, 180));
                isrmb = false;
            }
        }

        /// <summary>
        /// 金币购买
        /// </summary>
        void goldbuy()
        {
            
        }

        // 更新数据
        public void updateShopData()
        {
            /*int idx = 0;
            while(idx < TOTALITEM)
            {
                if(null != m_shopItemArray[idx])
                {
                    m_shopItemArray[idx].unload();
                }

                ++idx;
            }

            idx = 0;
            TableItemBase objitem;
            DataItemShop shopItem;
            LoadParam param;
            while(idx <  Ctx.m_instance.m_dataPlayer.m_dataShop.m_objList.Count && idx < TOTALITEM)
            {
                shopItem = Ctx.m_instance.m_dataPlayer.m_dataShop.m_objList[idx];
                objitem = Ctx.m_instance.m_tableSys.getItem(TableID.TABLE_OBJECT, shopItem.m_xmlItemMarket.m_objid) as TableItemBase;
                m_shopItemArray[idx].m_path = (objitem.m_itemBody as TableObjectItemBody).path;
                param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
                LocalFileSys.modifyLoadParam(m_shopItemArray[idx].m_path, param);
                param.m_loaded = m_shopItemArray[idx].onLoaded;
                Ctx.m_instance.m_resLoadMgr.loadResources(param);
                Ctx.m_instance.m_poolSys.deleteObj(param);

                ++idx;
            }*/

            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            for (int i = 0; i < TOTALPACK; i++)
            {
                XmlItemMarket itemMarket = marketCfg.getXmlItem(i+1) as XmlItemMarket;
                m_shopPriceText[i].text = string.Format("{0}", itemMarket.m_price);
            }

            m_shopGoldNum.text = string.Format("{0}", Ctx.m_instance.m_dataPlayer.m_dataMain.m_gold);
        }
    }
}