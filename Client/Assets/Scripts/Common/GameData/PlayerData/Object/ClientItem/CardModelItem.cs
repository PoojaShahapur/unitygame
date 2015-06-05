namespace SDK.Common
{
    /**
     * @brief 卡牌模型配置
     */
    public class CardModelItem
    {
        public string m_handleModelPath = "";                 // 模型路径，有类型决定手牌模型
        public string m_outModelPath = "";         // 模型路径，场牌模型

        public string m_headerSubModel = "";       // 头像子模型名字，由每一张卡牌决定
        public string m_frameSubModel = "";        // 边框子模型名字，由卡的职业决定
        public string m_yaoDaiSubModel = "";       // 腰带子模型名字，由卡的职业决定
        public string m_pinZhiSubModel = "";       // 品质子模型名字，根据品质决定，但是这个不配置，就几种，代码写死
        public string m_raceSubModel = "";         // 种族子模型名字

        public string m_outHeaderSubModel = "";       // 头像子模型名字，场牌
    }
}