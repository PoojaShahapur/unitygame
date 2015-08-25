using Mono.Xml;
using SDK.Lib;
using System.Collections;
using System.Security;

namespace SDK.Lib
{
    public class AttackActionSeq
    {
        protected MList<AttackActionItem> m_itemList;

        public MList<AttackActionItem> itemList
        {
            get
            {
                return m_itemList;
            }
        }

        public void parseXml(string str)
        {
            m_itemList = new MList<AttackActionItem>();
            AttackActionItem attackItem;

            SecurityParser _xmlDoc = new SecurityParser();
            _xmlDoc.LoadXml(str);

            SecurityElement rootNode = _xmlDoc.ToXml();         // Config 节点
            ArrayList attackItemXmlList = rootNode.Children;    // AttackItem 列表

            foreach (SecurityElement attackItemElem_ in attackItemXmlList)
            {
                attackItem = new AttackActionItem();
                m_itemList.Add(attackItem);
                attackItem.parseXmlElem(attackItemElem_);
            }
        }
    }
}