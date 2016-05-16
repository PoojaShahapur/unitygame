﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    public class UtilPath
    {

        // 递归创建子目录
        public static void recureCreateSubDir(string rootPath, string subPath, bool includeLast = false)
        {
            subPath = normalPath(subPath);
            if (!includeLast)
            {
                if (subPath.IndexOf('/') == -1)
                {
                    return;
                }
                subPath = subPath.Substring(0, subPath.LastIndexOf('/'));
            }

            if (Directory.Exists(Path.Combine(rootPath, subPath)))
            {
                return;
            }

            int startIdx = 0;
            int splitIdx = 0;
            while ((splitIdx = subPath.IndexOf('/', startIdx)) != -1)
            {
                if (!Directory.Exists(Path.Combine(rootPath, subPath.Substring(0, startIdx + splitIdx))))
                {
                    Directory.CreateDirectory(Path.Combine(rootPath, subPath.Substring(0, startIdx + splitIdx)));
                }

                startIdx += splitIdx;
                startIdx += 1;
            }

            Directory.CreateDirectory(Path.Combine(rootPath, subPath));
        }

        public static string normalPath(string path)
        {
            return path.Replace('\\', '/');
        }

        static public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception err)
                {
                    Debug.Log(string.Format("{0}{1}", "CreateDirectory Error: ", err.Message));
                }
            }
        }

        // 删除目录的时候，一定要关闭这个文件夹，否则删除文件夹可能出错
        static public void DeleteDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                }
                catch (Exception err)
                {
                    Debug.Log(string.Format("{0}{1}", "DeleteDirectory Error: ", err.Message));
                }
            }
        }

        // 目录是否存在
        static public bool ExistDirectory(string path)
        {
            return Directory.Exists(path);
        }

        static public void recurseCreateDirectory(string pathAndName)
        {
            string normPath = normalPath(pathAndName);
            string[] pathArr = normPath.Split(new[] { '/' });

            string curCreatePath = "";
            int idx = 0;
            for (; idx < pathArr.Length; ++idx)
            {
                if (curCreatePath.Length == 0)
                {
                    curCreatePath = pathArr[idx];
                }
                else
                {
                    curCreatePath = string.Format("{0}/{1}", curCreatePath, pathArr[idx]);
                }

                CreateDirectory(curCreatePath);
            }
        }

        static public bool modifyFileName(string srcPath, string destPath)
        {
            try
            {
                if (File.Exists(srcPath))
                {
                    File.Move(srcPath, destPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception excep)
            {
                return false;
            }
        }

        static public string combine(params string[] pathList)
        {
            int idx = 0;
            string ret = "";
            while (idx < pathList.Length)
            {
                if (ret.Length > 0)
                {
                    if (pathList[idx].Length > 0)
                    {
                        if (ret[ret.Length - 1] != '/' || pathList[idx][pathList[idx].Length - 1] != '/')
                        {
                            ret += "/";
                        }
                        ret += pathList[idx];
                    }
                }
                else
                {
                    if (pathList[idx].Length > 0)
                    {
                        ret += pathList[idx];
                    }
                }
                ++idx;
            }
            ret.Replace("//", "/");
            return ret;
        }

        static public List<string> GetAll(string path, bool recursion = false)//搜索文件夹中的文件
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> FileList = new List<string>();

            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                //FileList.Add(fi.Name);
                FileList.Add(normalPath(fi.FullName));
            }

            if (recursion)
            {
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo d in allDir)
                {
                    GetAll(d.FullName, recursion);
                }
            }
            return FileList;
        }

        static public string getFileExt(string path)
        {
            int dotIdx = path.LastIndexOf('.');
            if (-1 != dotIdx)
            {
                return path.Substring(dotIdx + 1);
            }

            return "";
        }

        static public string getFileNameWithExt(string path)
        {
            path = normalPath(path);
            int slashIdx = path.LastIndexOf('/');
            if (-1 != slashIdx)
            {
                return path.Substring(slashIdx + 1, path.Length - slashIdx - 1);
            }
            else
            {
                return path;
            }
        }

        // 添加版本的文件名，例如 E:/aaa/bbb/ccc.txt?v=1024
        public static string versionPath(string path, string version)
        {
            if (!string.IsNullOrEmpty(version))
            {
                return string.Format("{0}?v={1}", path, version);
            }
            else
            {
                return path;
            }
        }

        // 删除所有除去版本号外相同的文件，例如 E:/aaa/bbb/ccc.txt?v=1024 ，只要 E:/aaa/bbb/ccc.txt 一样就删除，参数就是 E:/aaa/bbb/ccc.txt ，没有版本号的文件
        public static void delFileNoVer(string path)
        {
            path = normalPath(path);
            DirectoryInfo TheFolder = new DirectoryInfo(path.Substring(0, path.LastIndexOf('/')));
            FileInfo[] allFiles = TheFolder.GetFiles(string.Format("{0}*", path));
            foreach (var item in allFiles)
            {
                item.Delete();
            }
        }

        public static bool fileExistNoVer(string path)
        {
            path = normalPath(path);
            DirectoryInfo TheFolder = new DirectoryInfo(path.Substring(0, path.LastIndexOf('/')));
            FileInfo[] allFiles = TheFolder.GetFiles(string.Format("{0}*", path));

            return allFiles.Length > 0;
        }

        public static bool delFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return true;
        }

        public static void renameFile(string srcPath, string destPath)
        {
            if (File.Exists(srcPath))
            {
                try
                {
                    File.Move(srcPath, destPath);
                }
                catch (Exception /*err*/)
                {
                    Ctx.m_instance.m_logSys.catchLog(string.Format("修改文件名字 {0} 改成 {1} 失败", srcPath, destPath));
                }
            }
        }

        static public void saveTex2File(Texture2D tex, string filePath)
        {
            //将图片信息编码为字节信息
            byte[] bytes = tex.EncodeToPNG();
            //保存
            System.IO.File.WriteAllBytes(filePath, bytes);
        }

        static public void saveStr2File(string str, string filePath, Encoding encoding)
        {
            System.IO.File.WriteAllText(filePath, str, encoding);
        }

        static public void saveByte2File(string path, byte[] bytes)
        {
            System.IO.File.WriteAllBytes(path, bytes);
        }

        // 获取文件名字，没有路径，但是有扩展名字
        static public string getFileNameNoPath(string fullPath)
        {
            int index = fullPath.LastIndexOf('/');
            string ret = "";
            if (index == -1)
            {
                index = fullPath.LastIndexOf('\\');
            }
            if (index != -1)
            {
                ret = fullPath.Substring(index + 1);
            }
            else
            {
                ret = fullPath;
            }

            return ret;
        }

        // 获取文件名字，没有扩展名字
        static public string getFileNameNoExt(string fullPath)
        {
            int index = fullPath.LastIndexOf('/');
            string ret = "";
            if (index == -1)
            {
                index = fullPath.LastIndexOf('\\');
            }
            if (index != -1)
            {
                ret = fullPath.Substring(index + 1);
            }
            else
            {
                ret = fullPath;
            }

            index = ret.LastIndexOf('.');
            if (index != -1)
            {
                ret = ret.Substring(0, index);
            }

            return ret;
        }

        // 获取文件路径，没有文件名字
        static public string getFilePathNoName(string fullPath)
        {
            int index = fullPath.LastIndexOf('/');
            string ret = "";
            if (index == -1)
            {
                index = fullPath.LastIndexOf('\\');
            }
            if (index != -1)
            {
                ret = fullPath.Substring(0, index);
            }
            else
            {
                ret = fullPath;
            }

            return ret;
        }

        // 递归拷贝目录
        static public void recurseCopyDirectory(string srcDir, string destDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(destDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + "/" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                recurseCopyDirectory(dirs[j].FullName, target.FullName + "/" + dirs[j].Name);
            }
        }

        static public void recurseDeleteFiles(string dir, MList<string> fileList, MList<string> extNameList)
        {
            DirectoryInfo fatherFolder = new DirectoryInfo(dir);
            //删除当前文件夹内文件
            FileInfo[] files = fatherFolder.GetFiles();
            int dotIdx = 0;
            string extName = "";

            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                try
                {
                    if (fileList != null)
                    {
                        //if (!fileName.Equals("delFileName.dat"))
                        //{
                        //    File.Delete(file.FullName);
                        //}
                        if (fileList.IndexOf(fileName) != -1)
                        {
                            File.Delete(file.FullName);
                        }
                    }
                    if (extNameList != null)
                    {
                        dotIdx = fileName.LastIndexOf(".");
                        if (dotIdx != -1)
                        {
                            extName = fileName.Substring(dotIdx + 1);
                            if (extNameList.IndexOf(extName) != -1)
                            {
                                File.Delete(file.FullName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Ctx.m_instance.m_logSys.log(ex.Message, LogTypeId.eLogCommon);
                }
            }
            //递归删除子文件夹内文件
            foreach (DirectoryInfo childFolder in fatherFolder.GetDirectories())
            {
                recurseDeleteFiles(childFolder.FullName, fileList, extNameList);
            }
        }

        // 删除一个文件夹，但是不删除文件夹自己，包括这个文件夹中的所有文件和目录
        static public void deleteSubDirsAndFiles(string curDir)
        {
            DirectoryInfo fatherFolder = new DirectoryInfo(curDir);
            //删除当前文件夹内文件
            FileInfo[] files = fatherFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                try
                {
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    Ctx.m_instance.m_logSys.log(ex.Message, LogTypeId.eLogCommon);
                }
            }

            // 删除子文件夹
            foreach (DirectoryInfo childFolder in fatherFolder.GetDirectories())
            {
                UtilPath.DeleteDirectory(childFolder.FullName, true);
            }
        }

        // 打包成 unity3d 后文件名字会变成小写，这里修改一下
        static public void modifyFileNameToCapital(string path, string fileNameNoExt)
        {
            string srcFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt.ToLower(), UtilApi.UNITY3D);
            string destFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt, UtilApi.UNITY3D);

            if (File.Exists(srcFullPath))
            {
                File.Move(srcFullPath, destFullPath);
            }
            else
            {
                Debug.Log(string.Format("{0} 文件不存在", srcFullPath));
            }

            srcFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt.ToLower(), UtilApi.UNITY3D);
            destFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt, UtilApi.UNITY3D);

            if (File.Exists(srcFullPath))
            {
                File.Move(srcFullPath, destFullPath);
            }
            else
            {
                Debug.Log(string.Format("{0} 文件不存在", srcFullPath));
            }
        }

        // 删除一个文件
        static public void deleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        // 大写转换成小写
        static public string toLower(string src)
        {
            return src.ToLower();
        }
    }
}