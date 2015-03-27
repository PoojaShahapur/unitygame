using UnityEngine;
using System.Xml;

namespace SDK.Lib
{
    public class SceneNodeCfg
    {
        protected Vector3 m_pos;
        protected Quaternion m_rotate;
        protected Vector3 m_scale;
        protected string m_prefab;

        public void parse(XmlElement xe)
        {
            m_pos = new Vector3();
            m_rotate = new Quaternion();
            m_scale = new Vector3();

            m_prefab = xe.GetAttribute("prefab");

            string attr = xe.GetAttribute("pos");
            attr = attr.Substring(1, attr.Length - 2);
            char[] split = new char[1];
            split[0] = ',';
            string[] strarr = attr.Split(split);

            m_pos.x = float.Parse(strarr[0]);
            m_pos.y = float.Parse(strarr[1]);
            m_pos.z = float.Parse(strarr[2]);

            attr = xe.GetAttribute("rotate");
            attr = attr.Substring(1, attr.Length - 2);
            split[0] = ',';
            strarr = attr.Split(split);

            m_rotate.x = float.Parse(strarr[0]);
            m_rotate.y = float.Parse(strarr[1]);
            m_rotate.z = float.Parse(strarr[2]);
            m_rotate.w = float.Parse(strarr[3]);

            attr = xe.GetAttribute("scale");
            attr = attr.Substring(1, attr.Length - 2);
            split[0] = ',';
            strarr = attr.Split(split);

            m_scale.x = float.Parse(strarr[0]);
            m_scale.y = float.Parse(strarr[1]);
            m_scale.z = float.Parse(strarr[2]);
        }
    }
}