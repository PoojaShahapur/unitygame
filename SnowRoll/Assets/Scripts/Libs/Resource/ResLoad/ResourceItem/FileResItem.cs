namespace SDK.Lib
{
    /**
     * @brief 本地文件系统，直接从本地加载
     */
    public class FileResItem : ResItem
    {
        // 加载的资源目录的前缀目录，打包后的资源 name 需要加这个前缀才能找到
        public const string PRE_PATH = "Assets/Resources";
        public const string UNITY3D_EXT = "unity3d";
        public const string SCENE_PRE_PATH = "Assets/Scenes";

        public string m_bundlePath;
    }
}