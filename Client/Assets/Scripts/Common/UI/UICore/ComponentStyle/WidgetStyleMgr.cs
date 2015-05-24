using System.Collections.Generic;

namespace SDK.Common
{
    public class WidgetStyleMgr
    {
        protected Dictionary<WidgetStyleID, Dictionary<int, WidgetStyle>> m_style2Dic;

        public WidgetStyleMgr()
        {
            m_style2Dic = new Dictionary<WidgetStyleID, Dictionary<int, WidgetStyle>>();
        }

        public void addWidgetStype(WidgetStyleID widgetId, int comId, WidgetStyle style)
        {
            if(!m_style2Dic.ContainsKey(widgetId))
            {
                m_style2Dic[widgetId] = new Dictionary<int, WidgetStyle>();
            }
            m_style2Dic[widgetId][comId] = style;
        }

        public T GetWidgetStyle<T>(WidgetStyleID widgetId, int comId) where T : WidgetStyle
        {
            return m_style2Dic[widgetId][comId] as T;
        }
    }
}