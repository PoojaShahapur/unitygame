namespace SDK.Lib
{
    public class MapCfg
    {
        public MapXml mMapXml;

        protected void loadXml()
        {
            mMapXml = Ctx.mInstance.mXmlCfgMgr.getXmlCfg<MapXml>(XmlCfgID.eXmlMapCfg);
        }

        public MapXmlItem getXmlItem(uint sceneId)
        {
            if (mMapXml == null)
            {
                loadXml();
            }

            foreach (XmlItemBase item in mMapXml.mList.list())
            {
                if ((item as MapXmlItem) != null)
                {
                    if ((item as MapXmlItem).mSceneId == sceneId)
                    {
                        return (item as MapXmlItem);
                    }
                }
            }

            return null;
        }
    }
}