namespace SDK.Lib
{
    public class MapCfg
    {
        public MapXml m_mapXml;

        protected void loadXml()
        {
            m_mapXml = Ctx.mInstance.mXmlCfgMgr.getXmlCfg<MapXml>(XmlCfgID.eXmlMapCfg);
        }

        public MapXmlItem getXmlItem(uint sceneId)
        {
            if (m_mapXml == null)
            {
                loadXml();
            }

            foreach (XmlItemBase item in m_mapXml.m_list)
            {
                if ((item as MapXmlItem) != null)
                {
                    if ((item as MapXmlItem).m_sceneId == sceneId)
                    {
                        return (item as MapXmlItem);
                    }
                }
            }

            return null;
        }
    }
}