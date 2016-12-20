using System.Security;
using UnityEngine;

namespace SDK.Lib
{
    public class SceneNodeCfg
    {
        protected Vector3 mPos;
        protected Quaternion mRotate;
        protected Vector3 mScale;
        protected string mPrefab;

        public void parse(SecurityElement xe)
        {
            mPos = new Vector3();
            mRotate = new Quaternion();
            mScale = new Vector3();

            UtilXml.getXmlAttrStr(xe, "prefab", ref mPrefab);

            string attr = "";
            UtilXml.getXmlAttrStr(xe, "pos", ref attr);
            attr = attr.Substring(1, attr.Length - 2);
            char[] split = new char[1];
            split[0] = ',';
            string[] strarr = attr.Split(split);

            mPos.x = float.Parse(strarr[0]);
            mPos.y = float.Parse(strarr[1]);
            mPos.z = float.Parse(strarr[2]);

            UtilXml.getXmlAttrStr(xe, "rotate", ref attr);
            attr = attr.Substring(1, attr.Length - 2);
            split[0] = ',';
            strarr = attr.Split(split);

            mRotate.x = float.Parse(strarr[0]);
            mRotate.y = float.Parse(strarr[1]);
            mRotate.z = float.Parse(strarr[2]);
            mRotate.w = float.Parse(strarr[3]);

            UtilXml.getXmlAttrStr(xe, "scale", ref attr);
            attr = attr.Substring(1, attr.Length - 2);
            split[0] = ',';
            strarr = attr.Split(split);

            mScale.x = float.Parse(strarr[0]);
            mScale.y = float.Parse(strarr[1]);
            mScale.z = float.Parse(strarr[2]);
        }
    }
}