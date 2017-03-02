namespace SDK.Lib
{
    public enum FilesVerType
    {
        eResourceVer,
        eStreamingAssetsVer,
        ePersistentDataVer,
        eWebVer,
    }

    public class FileVerInfo
    {
        public string mOrigPath;    // 原始加载路径
        public string mResUniqueId; // 资源唯一ID
        public string mLoadPath;    // 加载的目录
        public string mFileMd5;    // 文件 md5
        public int mFileSize;      // 文件大小
    }
}