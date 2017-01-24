namespace SDK.Lib
{
    /**
     * @brief Tile Texture Info
     */
    public class TileInfo : XmlItemBase
    {
        public float mOffsetX;
        public float mOffsetY;
        public float mTileX;
        public float mTileY;

        override public void parseXml(System.Security.SecurityElement xmlelem)
        {
            UtilXml.getXmlAttrFloat(xmlelem, "OffsetX", ref mOffsetX);
            UtilXml.getXmlAttrFloat(xmlelem, "OffsetY", ref mOffsetY);
            UtilXml.getXmlAttrFloat(xmlelem, "TileX", ref mTileX);
            UtilXml.getXmlAttrFloat(xmlelem, "TileY", ref mTileY);
        }
    }
}