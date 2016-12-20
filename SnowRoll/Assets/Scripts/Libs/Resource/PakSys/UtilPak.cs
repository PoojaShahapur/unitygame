using System.IO;

namespace SDK.Lib
{
	public static class UtilPak
	{
		public static string getFullPathNoFileName(string filePath)
		{
			filePath = normPath (filePath);
			int idx = filePath.LastIndexOf ("/");
			if (idx != -1)
			{
				return filePath.Substring (0, idx);
			}

			return null;
		}

		public static string getFullPathNoExtName(string filePath)
		{
			filePath = normPath (filePath);
			int idx = filePath.LastIndexOf (".");
			if (idx != -1)
			{
				return filePath.Substring (0, idx);
			}

			return null;
		}
		
		public static string getLastPathName(string filePath)
		{
			filePath = normPath (filePath);
			int idx = filePath.LastIndexOf ("/");
			if (idx != -1)
			{
				return filePath.Substring (idx + 1);
			}

			return null;
		}
		
		public static void mkDir(string dir)
		{
			dir = normPath (dir);
			int startPos = 0;
			string curPath;
			while ((startPos = dir.IndexOf('/', startPos)) != -1)
			{
				curPath = dir.Substring (0, startPos);
				if (!Directory.Exists(curPath))                    // 判断是否存在
	            {
					Directory.CreateDirectory(curPath);            // 创建新路径
	            }

				startPos += 1;
			}

			// 创建最后的目录
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
		}

		public static string normPath(string pPath)
		{
			pPath = pPath.Replace ("\\", "/");
			return pPath;
		}

		public static void setFlags(FileHeaderFlag flagsID, ref uint flags)
		{
			flags |= (uint)flagsID;
		}

		public static void clearFlags(FileHeaderFlag flagsID, ref uint flags)
		{
			flags &= ~(uint)flagsID;
		}

		public static bool checkFlags(FileHeaderFlag flagsID, ref uint flags)
		{
			return (flags & (uint)flagsID) > 0;
		}
	}
}