﻿using EditorTool;
using SDK.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace EditorTool
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

            // 打开写入的文件
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
                    if (bNeedExportNS(_type.Namespace))
                    {
                        // 如果是模板类型，名字是 "DynamicBuffer`1"，但是 Type.MemberType == MemberTypes.TypeInfo
                        // 如果是内部类，那么 Type.MemberType == MemberTypes.NestedType
                        // 如果是协程(Coroutine)，例如 protected IEnumerator initAssetByCoroutine()，Type.MemberType == MemberTypes.NestedType
                        // 去掉模板类、内部类、协程、接口、抽象类
                        // 抽象类导致 ToLuaExport.cs 导出的 bind 文件编译不过
                        // 函数中有模板类， 导致 ToLuaExport.cs 宕机。 static public void newRectSplit(Transform trans, float unitWidth, float areaRadius, float fYDelta, int splitCnt, ref List<Vector3> posList)
                        if (!_type.Name.Contains("`1") &&
                            _type.MemberType == MemberTypes.TypeInfo &&
                            !_type.IsInterface && 
                            !_type.IsAbstract)
                        {
                            if (!bNeedExportClassName(_type.Name))
                            {
                                continue;
                            }

                            if (!namespaceDic.ContainsKey(_type.Namespace))
                            {
                                namespaceDic[_type.Namespace] = true;
                                namespaceStr += string.Format("using {0};\r\n", _type.Namespace);
                            }

                            bindTypeInfo += string.Format("         _GT(typeof({0})),\r\n", _type.Name);
                        }
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

        static protected bool bNeedExportNS(string nameSpace)
        {
            if(nameSpace == "SDK.Lib" ||
               nameSpace == "SDK.Common")
            {
                return true;
            }

            return false;
        }

        static protected bool bNeedExportClassName(string className)
        {
            if (className.Contains("UtilMath") ||
                className.Contains("Dec") ||
                className.Contains("DynSceneGrid"))
            {
                return false;
            }

            return true;
        }
    }
}