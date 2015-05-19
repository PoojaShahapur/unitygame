namespace SDK.Common
{
    /**
     * @brief 卡牌模型配置
     */
    public class CardModelItem
    {
        public string m_path = "";                  // 模型路径，有类型决定

        public string m_headerSubModel = "";       // 头像子模型名字，由每一张卡牌决定
        public string m_frameSubModel = "";        // 边框子模型名字，由卡的职业决定
        public string m_yaoDaiSubModel = "";       // 腰带子模型名字，由卡的职业决定
        public string m_pinZhiSubModel = "";       // 品质子模型名字，根据品质决定，但是这个不配置，就几种，代码写死
    }
}