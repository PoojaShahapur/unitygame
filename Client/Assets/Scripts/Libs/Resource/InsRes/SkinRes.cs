using Mono.Xml;
using SDK.Common;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class SkinRes : TextResBase
    {
        protected string[] m_boneArr;   // 蒙皮的骨头列表

        public string[] boneArr
        {
            get
            {
                return m_boneArr;
            }
            set
            {
                m_boneArr = value;
            }
        }

        public override void init(ResItem res)
        {
            base.init(res);

            SecurityParser xmlDoc = new SecurityParser();
            xmlDoc.LoadXml(m_text);

            SecurityElement rootNode = xmlDoc.ToXml();
            ArrayList itemMeshList = rootNode.Children;
            SecurityElement itemMesh;

            ArrayList itemSubMeshList;
            SecurityElement itemSubMesh;
            string meshName = "";
            string subMeshName = "";
            string bonesList = "";

            foreach (SecurityElement itemNode1f in itemMeshList)
            {
                itemMesh = itemNode1f;
                meshName = UtilXml.getXmlAttrStr(itemMesh, "name");

                itemSubMeshList = itemMesh.Children;
                foreach (SecurityElement itemNode2f in itemSubMeshList)
                {
                    itemSubMesh = itemNode2f;
                    subMeshName = UtilXml.getXmlAttrStr(itemSubMesh, "name");
                    bonesList = UtilXml.getXmlAttrStr(itemSubMesh, "bonelist");
                    m_boneArr = bonesList.Split(',');
                }
            }

            m_text = "";
        }
    }
}