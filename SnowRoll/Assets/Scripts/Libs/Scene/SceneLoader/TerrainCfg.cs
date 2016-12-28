using System.Security;

namespace SDK.Lib
{
    public class TerrainCfg
    {
        protected float mWidth;
        protected float mHeight;

        public float Width
        {
            get
            {
                return mWidth;
            }
        }

        public float Height
        {
            get
            {
                return mHeight;
            }
        }

        public void parse(SecurityElement xe)
        {
            string attr = "";
            UtilXml.getXmlAttrStr(xe, "size", ref attr);
            attr = attr.Substring(1, attr.Length - 2);
            char[] split = new char[1];
            split[0] = ',';
            string[] strarr = attr.Split(split);

            mWidth = float.Parse(strarr[0]);
            mHeight = float.Parse(strarr[2]);
        }
    }
}