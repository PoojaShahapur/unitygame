using Game.UI;
using System.Collections.Generic;

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
            XmlMarketCfg marketCfg = Ctx.m_instance.m_xmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            DataItemShop dataItemShop;
            foreach(ushort id in list)
            {
                dataItemShop = new DataItemShop();
                m_objList.Add(dataItemShop);
                dataItemShop.m_xmlItemMarket = marketCfg.getXmlItem(id) as XmlItemMarket;
            }

            IUIShop uiShop = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIShop) as IUIShop;
            if(uiShop != null)
            {
                //uiShop.updateShopData();
            }
        }
    }
}