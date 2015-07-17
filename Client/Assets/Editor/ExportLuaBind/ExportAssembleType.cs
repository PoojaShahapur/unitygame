using EditorTool;
using SDK.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace SDK.Lib
{
    public class ExportAssembleType
    {
        [MenuItem("Assets/LuaExport/Build-[BindLua.cs]-File")]
        static public void exportTypt()
        {
            string _path;
            byte[] bytes = null;
            Dictionary<string, bool> namespaceDic = new Dictionary<string, bool>();
            string namespaceStr = "";

            // 写入的文件
            _path = ExportUtil.getDataPath("Editor/BindLua.cs");
            FileStream fileStream;
            if (File.Exists(@_path))
            {
                File.Delete(@_path);
            }

            fileStream = new FileStream(_path, FileMode.Create);

            // 遍历类型
            string bindTypeInfo = "";
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type _type in assembly.GetTypes())
                {
                    if (bSelfType(_type.Namespace))
                    {
                        if (!namespaceDic.ContainsKey(_type.Namespace))
                        {
                            namespaceDic[_type.Namespace] = true;
                            namespaceStr += string.Format("using {0};\r\n", _type.Namespace);
                        }
                        bindTypeInfo += string.Format("         _GT(typeof({0})),\r\n", _type.Name);
                    }
                }
            }

            // 写入名字空间
            bytes = Encoding.UTF8.GetBytes(namespaceStr);
            fileStream.Write(bytes, 0, bytes.Length);

            // 首先写入文件前半部分
            _path = ExportUtil.getDataPath("Editor/ExportLuaBind/BindLuaCsOne.txt");
            bytes = File.ReadAllBytes(_path);
            fileStream.Write(bytes, 0, bytes.Length);

            // 写入文件类型
            bytes = Encoding.UTF8.GetBytes(bindTypeInfo);
            fileStream.Write(bytes, 0, bytes.Length);

            // 写入文件后半部分
            _path = ExportUtil.getDataPath("Editor/ExportLuaBind/BindLuaCsTwo.txt");
            bytes = File.ReadAllBytes(_path);
            fileStream.Write(bytes, 0, bytes.Length);

            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
        }

        static public bool bSelfType(string nameSpace)
        {
            if(nameSpace == "SDK.Lib" ||
               nameSpace == "SDK.Common")
            {
                return true;
            }

            return false;
        }
    }
}