using System.Collections.Generic;

namespace SDK.Lib
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
            XmlMarketCfg marketCfg = Ctx.mInstance.mXmlCfgMgr.getXmlCfg<XmlMarketCfg>(XmlCfgID.eXmlMarketCfg);
            DataItemShop dataItemShop;
            foreach(ushort id in list)
            {
                dataItemShop = new DataItemShop();
                m_objList.Add(dataItemShop);
                dataItemShop.m_xmlItemMarket = marketCfg.getXmlItem(id) as XmlItemMarket;
            }
        }
    }
}