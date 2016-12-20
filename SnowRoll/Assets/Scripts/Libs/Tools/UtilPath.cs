using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDK.Lib
{
    public class UtilPath
    {
        public static string normalPath(string path)
        {
            return path.Replace('\\', '/');
        }

        // 删除目录的时候，一定要关闭这个文件夹，否则删除文件夹可能出错
        static public void deleteDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, recursive);
                }
                catch (Exception err)
                {
                    Debug.Log(string.Format("{0}", "DeleteDirectory Error: ", err.Message));
                }
            }
        }

        // 目录是否存在
        static public bool existDirectory(string path)
        {
            return Directory.Exists(path);
        }

        // 文件是否存在
        static public bool existFile(string path)
        {
            return File.Exists(path);
        }

        // 移动文件
        static public void move(string srcPath, string destPath)
        {
            try
            {
                File.Move(srcPath, destPath);
            }
            catch (Exception err)
            {
                Debug.Log(string.Format("{0}", "move Error: ", err.Message));
            }
        }

        public static bool deleteFile(string path)
        {
            if (UtilPath.existFile(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception err)
                {
                    Debug.Log(string.Format("{0}", "deleteFile Error: ", err.Message));
                }
            }

            return true;
        }

        public static void copyFile(string sourceFileName, string destFileName, bool overwrite = false)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, overwrite);
            }
            catch (Exception err)
            {
                Debug.Log(string.Format("{0}", "copyFile Error: ", err.Message));
            }
        }

        static public void createDirectory(string pathAndName, bool isRecurse = false)
        {
            if (isRecurse)
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

                    if (!Directory.Exists(curCreatePath))
                    {
                        try
                        {
                            Directory.CreateDirectory(curCreatePath);
                        }
                        catch (Exception err)
                        {
                            Debug.Log(string.Format("{0}", "CreateDirectory Error: ", err.Message));
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if (!Directory.Exists(pathAndName))
                    {
                        // 这个接口默认就支持创建所有没有的目录
                        Directory.CreateDirectory(pathAndName);
                    }
                }
                catch (Exception err)
                {
                    Debug.Log(string.Format("{0}", "CreateDirectory Error: ", err.Message));
                }
            }
        }

        static public bool renameFile(string srcPath, string destPath)
        {
            try
            {
                if (UtilPath.existFile(srcPath))
                {
                    UtilPath.move(srcPath, destPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception excep)
            {
                Debug.Log(string.Format("{0}{1}", "renameFile Error: ", excep.Message));
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
            ret = ret.Replace("//", "/");
            return ret;
        }

        // 获取扩展名
        static public string getFileExt(string path)
        {
            int dotIdx = path.LastIndexOf('.');
            if (-1 != dotIdx)
            {
                return path.Substring(dotIdx + 1);
            }

            return "";
        }

        // 获取文件名字，没有路径，但是有扩展名字
        static public string getFileNameWithExt(string fullPath)
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

        // 获取文件路径，没有文件名字扩展
        static public string getFilePathNoExt(string fullPath)
        {
            int index = 0;
            string ret = fullPath;
            index = fullPath.LastIndexOf('.');
            if (index != -1)
            {
                ret = fullPath.Substring(0, index);
            }

            return ret;
        }

        // 搜索文件夹中的文件
        static public MList<string> getAllFile(string path, MList<string> includeExtList = null, MList<string> excludeExtList = null, bool recursion = false)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            MList<string> fileList = new MList<string>();

            string extName = "";
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo file in allFile)
            {
                extName = UtilPath.getFileExt(file.FullName);
                if (includeExtList != null && includeExtList.IndexOf(extName) != -1)
                {
                    fileList.Add(normalPath(file.FullName));
                }
                else if(excludeExtList != null && excludeExtList.IndexOf(extName) == -1)
                {
                    fileList.Add(normalPath(file.FullName));
                }
                else if(includeExtList == null && excludeExtList == null)
                {
                    fileList.Add(normalPath(file.FullName));
                }
            }

            if (recursion)
            {
                DirectoryInfo[] allDir = dir.GetDirectories();
                foreach (DirectoryInfo dirItem in allDir)
                {
                    fileList.merge(getAllFile(dirItem.FullName, includeExtList, excludeExtList, recursion));
                }
            }
            return fileList;
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

        // 递归拷贝目录
        static public void copyDirectory(string srcPath, string destPath, bool isRecurse = false)
        {
            DirectoryInfo sourceDirInfo = new DirectoryInfo(srcPath);
            DirectoryInfo targetDirInfo = new DirectoryInfo(destPath);

            if (targetDirInfo.FullName.StartsWith(sourceDirInfo.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                Debug.Log("destPath is srcPath subDir, can not copy");
                return;
            }

            if (!sourceDirInfo.Exists)
            {
                return;
            }

            if (!targetDirInfo.Exists)
            {
                targetDirInfo.Create();
            }

            FileInfo[] files = sourceDirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                UtilPath.copyFile(files[i].FullName, targetDirInfo.FullName + "/" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = sourceDirInfo.GetDirectories();

            if (isRecurse)
            {
                for (int j = 0; j < dirs.Length; j++)
                {
                    copyDirectory(dirs[j].FullName, targetDirInfo.FullName + "/" + dirs[j].Name, isRecurse);
                }
            }
        }

        static public void traverseDirectory(
            string srcPath,
            string destPath,
            Action<string, string, string> dirHandle = null,
            Action<string, string, string> fileHandle = null,
            bool isRecurse = false,
            bool isCreateDestPath = false
            )
        {
            DirectoryInfo sourceDirInfo = new DirectoryInfo(srcPath);
            DirectoryInfo targetDirInfo = null;

            // 如果不是目录规则的字符串，执行 new DirectoryInfo(destPath); 会报错
            if (!string.IsNullOrEmpty(destPath))
            {
                targetDirInfo = new DirectoryInfo(destPath);

                if (targetDirInfo.FullName.StartsWith(sourceDirInfo.FullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Debug.Log("destPath is srcPath subDir, can not copy");
                    return;
                }
            }

            if (!sourceDirInfo.Exists)
            {
                return;
            }

            if (!string.IsNullOrEmpty(destPath))
            {
                if (!UtilPath.existDirectory(destPath) && isCreateDestPath)
                {
                    UtilPath.createDirectory(destPath);
                    targetDirInfo = new DirectoryInfo(destPath);
                }
            }

            if (dirHandle != null)
            {
                if (string.IsNullOrEmpty(destPath))
                {
                    dirHandle(sourceDirInfo.FullName, sourceDirInfo.Name, "");
                }
                else
                {
                    dirHandle(sourceDirInfo.FullName, sourceDirInfo.Name, targetDirInfo.FullName);
                }
            }

            FileInfo[] files = sourceDirInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                if (fileHandle != null)
                {
                    if (string.IsNullOrEmpty(destPath))
                    {
                        fileHandle(files[i].FullName, files[i].Name, "");
                    }
                    else
                    {
                        fileHandle(files[i].FullName, files[i].Name, targetDirInfo.FullName);
                    }
                }
            }

            DirectoryInfo[] dirs = sourceDirInfo.GetDirectories();

            if (isRecurse)
            {
                for (int j = 0; j < dirs.Length; j++)
                {
                    if (string.IsNullOrEmpty(destPath))
                    {
                        traverseDirectory(dirs[j].FullName, "", dirHandle, fileHandle, isRecurse, isCreateDestPath);
                    }
                    else
                    {
                        traverseDirectory(dirs[j].FullName, targetDirInfo.FullName + "/" + dirs[j].Name, dirHandle, fileHandle, isRecurse, isCreateDestPath);
                    }
                }
            }
        }

        static public void deleteFiles(string srcPath, MList<string> fileList, MList<string> extNameList, bool isRecurse = false)
        {
            DirectoryInfo fatherFolder = new DirectoryInfo(srcPath);
            //删除当前文件夹内文件
            FileInfo[] files = fatherFolder.GetFiles();
            string extName = "";

            foreach (FileInfo file in files)
            {
                string fileName = file.Name;

                if (fileList != null)
                {
                    if (fileList.IndexOf(fileName) != -1)
                    {
                        UtilPath.deleteFile(file.FullName);
                    }
                }
                if (extNameList != null)
                {
                    extName = UtilPath.getFileExt(file.FullName);
                    if (extNameList.IndexOf(extName) != -1)
                    {
                        UtilPath.deleteFile(file.FullName);
                    }
                }
            }
            if (isRecurse)
            {
                //递归删除子文件夹内文件
                foreach (DirectoryInfo childFolder in fatherFolder.GetDirectories())
                {
                    deleteFiles(childFolder.FullName, fileList, extNameList, isRecurse);
                }
            }
        }

        // 递归删除所有的文件和目录
        static public void deleteSubDirsAndFiles(string curDir, MList<string> excludeDirList, MList<string> excludeFileList)
        {
            DirectoryInfo fatherFolder = new DirectoryInfo(curDir);
            //删除当前文件夹内文件
            FileInfo[] files = fatherFolder.GetFiles();
            string normalPath = "";

            foreach (FileInfo file in files)
            {
                string fileName = file.Name;
                normalPath = UtilPath.normalPath(file.FullName);
                if (!UtilPath.isEqualStrInList(normalPath, excludeFileList))
                {
                    UtilPath.deleteFile(file.FullName);
                }
            }

            // 递归删除子文件夹内文件
            foreach (DirectoryInfo childFolder in fatherFolder.GetDirectories())
            {
                normalPath = UtilPath.normalPath(childFolder.FullName);
                if(!UtilPath.isEqualStrInList(normalPath, excludeDirList))
                {
                    if(UtilPath.isSubStrInList(normalPath, excludeDirList) && !UtilPath.isSubStrInList(normalPath, excludeFileList))
                    {
                        UtilPath.deleteDirectory(childFolder.FullName, true);
                    }
                    else
                    {
                        UtilPath.deleteSubDirsAndFiles(childFolder.FullName, excludeDirList, excludeFileList);
                    }
                }
            }
        }

        // 字符串是否是子串
        static public bool isSubStrInList(string str, MList<string> list)
        {
            bool ret = false;

            int idx = 0;
            int len = 0;

            if(list != null)
            {
                idx = 0;
                len = list.length();

                while(idx < len)
                {
                    if(list[idx].IndexOf(str) != -1)
                    {
                        ret = true;
                        break;
                    }

                    ++idx;
                }
            }

            return ret;
        }

        static public bool isEqualStrInList(string str, MList<string> list)
        {
            bool ret = false;

            int idx = 0;
            int len = 0;

            if(list != null)
            {
                idx = 0;
                len = list.length();

                while(idx < len)
                {
                    if(list[idx] == str)
                    {
                        ret = true;
                        break;
                    }

                    ++idx;
                }
            }

            return ret;
        }

        // 打包成 unity3d 后文件名字会变成小写，这里修改一下
        static public void modifyFileNameToCapital(string path, string fileNameNoExt)
        {
            string srcFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt.ToLower(), UtilApi.UNITY3D);
            string destFullPath = string.Format("{0}/{1}.{2}", path, fileNameNoExt, UtilApi.UNITY3D);
            UtilPath.move(srcFullPath, destFullPath);

            srcFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt.ToLower(), UtilApi.UNITY3D);
            destFullPath = string.Format("{0}/{1}.{2}.manifest", path, fileNameNoExt, UtilApi.UNITY3D);
            UtilPath.move(srcFullPath, destFullPath);
        }

        // 大写转换成小写
        static public string toLower(string src)
        {
            return src.ToLower();
        }

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

            if (UtilPath.existDirectory(UtilPath.combine(rootPath, subPath)))
            {
                return;
            }

            int startIdx = 0;
            int splitIdx = 0;
            while ((splitIdx = subPath.IndexOf('/', startIdx)) != -1)
            {
                if (!UtilPath.existDirectory(UtilPath.combine(rootPath, subPath.Substring(0, startIdx + splitIdx))))
                {
                    UtilPath.createDirectory(UtilPath.combine(rootPath, subPath.Substring(0, startIdx + splitIdx)));
                }

                startIdx += splitIdx;
                startIdx += 1;
            }

            UtilPath.createDirectory(UtilPath.combine(rootPath, subPath));
        }

        // Android 运行时
        static public bool isAndroidRuntime()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        // 是否是 StreamingAssetsPath 目录
        static public bool isStreamingAssetsPath(string path)
        {
            path = UtilPath.normalPath(path);
            return path.IndexOf(MFileSys.msDataStreamStreamingAssetsPath) == 0;
        }

        static public string getCurrentDirectory()
        {
            string curPath = System.Environment.CurrentDirectory;
            curPath = UtilPath.normalPath(curPath);

            return curPath;
        }
    }
}