using UnityEngine.UI;

namespace SDK.Common
{
    /**
     * @brief 动态加载的 Image，但是 GameObject 是静态的
     */
    public class AuxDynImageStaticGO : AuxDynImage
    {
        // 查找 UI 组件
        override public void findWidget()
        {
            if (string.IsNullOrEmpty(m_goName))      // 如果 m_goName 为空，就说明就是当前 GameObject 上获取 Image 
            {
                m_image = UtilApi.getComByP<Image>(m_pntGo);
            }
            else
            {
                m_image = UtilApi.getComByP<Image>(m_pntGo, m_goName);
            }
        }
    }
}