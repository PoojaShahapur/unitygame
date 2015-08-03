namespace EditorTool
{
    class PackParam
    {
        public string m_type;
        public string m_inPath;
        public string m_outPath;
        public bool m_packAllFiles = false;     // 这个是指遍历当前目录下的所有文件，并且每一个文件单独打包
        public string[] m_extArr;       // 要打包的所有文件的扩展名字
    }
}