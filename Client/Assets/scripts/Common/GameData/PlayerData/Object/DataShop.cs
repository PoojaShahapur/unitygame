﻿using System.Collections.Generic;

namespace SDK.Common
{
    /**
     * @brief 商城数据
     */
    public class DataShop
    {
        public List<DataItemShop> m_objList = new List<DataItemShop>();             // 道具列表

        // 更新商城内容
        public void updateShop(List<ushort> list)
        {
            m_objList.Clear();
            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg(XmlCfgID.eXmlMarketCfg) as XmlMarketCfg;
            DataItemShop dataItemShop;
            foreach(ushort id in list)
            {
                dataItemShop = new DataItemShop();
                m_objList.Add(dataItemShop);
                dataItemShop.m_xmlItemMarket = marketCfg.getXmlItem(id) as XmlItemMarket;
            }

            (Ctx.m_instance.m_interActiveEntityMgr.getActiveEntity("shop") as ILSshop).updateShopData();
        }
    }
}