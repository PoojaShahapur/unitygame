using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 商城数据
     */
    public class DataShop
    {
        public List<DataItemShop> mObjList = new List<DataItemShop>();             // 道具列表

        // 更新商城内容
        public void updateShop(List<ushort> list)
        {
            mObjList.Clear();

            XmlMarketCfg marketCfg = XmlMarketCfg.loadAndRetXml<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            DataItemShop dataItemShop;

            foreach(ushort id in list)
            {
                dataItemShop = new DataItemShop();
                mObjList.Add(dataItemShop);
                dataItemShop.m_xmlItemMarket = marketCfg.getXmlItem(id) as XmlItemMarket;
            }
        }
    }
}