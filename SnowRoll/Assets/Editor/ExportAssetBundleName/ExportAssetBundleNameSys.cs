using System.Collections.Generic;
using System.IO;

namespace EditorTool
{
    public class ExportAssetBundleNameSys
    {
        protected AssetBundleNameXmlData mAbNameXmlData;

        public ExportAssetBundleNameSys()
        {
            mAbNameXmlData = new AssetBundleNameXmlData();
        }

        public void init()
        {

        }

        public void clear()
        {
            mAbNameXmlData.clear();
        }

        public void parseXml()
        {
            mAbNameXmlData.parseXml();
        }

        public void setAssetBundleName()
        {
            mAbNameXmlData.setAssetBundleName();
        }

        // ���� Res Ŀ¼ AB ��Դ����ӳ��
        public void exportResABKV()
        {
            List<string> list = new List<string>();
            mAbNameXmlData.exportResABKV(list);

            writeFile(UtilEditor.getWorkPath("AssetBundlePath.txt"), list);
        }

        public void writeFile(string path, List<string> list)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            
            if (File.Exists(@path))                  // ����ļ�����
            {
                File.Delete(@path);
            }

            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);

            foreach(string line in list)
            {
                streamWriter.WriteLine(line);
            }

            streamWriter.Close();
            fileStream.Close();
        }
    }
}