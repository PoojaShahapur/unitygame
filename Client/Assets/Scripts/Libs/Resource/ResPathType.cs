namespace SDK.Lib
{
    /**
     * @brief 资源路径类型
     */
    public enum ResPathType
    {
        ePathScene,     // 场景路径
        ePathSceneXml,  // 场景路径
        ePathModule,    // Module 路径
        ePathComUI,     // UI 控件路径
        ePathCodePath,  // UI 代码目录
        ePathBeingPath, // Being 目录
        ePathAIPath,    // AI 目录
        ePathTablePath, // Table 目录
        ePathLangXml,   // 语言配置文件
        ePathXmlCfg,    // Xml 配置文件
        ePathModel,       // 各种 Prefab
        ePathMaterial,      // 材质目录
        ePathImage,      // 图片目录
        ePathWord,      // 过滤字、随机名字
        ePathAudio,      // 音频

        eTotal          // 总数
    }
}