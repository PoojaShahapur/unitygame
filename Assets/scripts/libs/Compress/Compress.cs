using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;

/// <summary>
/// Summary description for ICSharp
/// </summary>
public class Compress
{
    /// <summary>
    /// ѹ��
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    static public string CompressStr(string param)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(param);
        //byte[] data = Convert.FromBase64String(param);
        MemoryStream ms = new MemoryStream();
        Stream stream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
        try
        {
            stream.Write(data, 0, data.Length);
        }
        finally 
        {
            stream.Close();
            ms.Close();
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// ��ѹ
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    static public string DecompressStr(string param)
    {
        string commonString = "";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(param);
        //byte[] buffer=Convert.FromBase64String(param);
        MemoryStream ms = new MemoryStream(buffer);
        Stream sm = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
        //����Ҫָ��Ҫ����ĸ�ʽ��Ҫ����������
        StreamReader reader = new StreamReader(sm,System.Text.Encoding.UTF8);
        try
        {
            commonString=reader.ReadToEnd();
        }
        finally
        {
            sm.Close();
            ms.Close();
        }
        return commonString;
    }

    /// <summary>
    /// ѹ��
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    static public string CompressByte(byte[] data)
    {
        MemoryStream ms = new MemoryStream();
        Stream stream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
        try
        {
            stream.Write(data, 0, data.Length);
        }
        finally
        {
            stream.Close();
            ms.Close();
        }
        return Convert.ToBase64String(ms.ToArray());
    }
    /// <summary>
    /// ��ѹ
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    static public string DecompressByte(byte[] buffer)
    {
        string commonString = "";
        MemoryStream ms = new MemoryStream(buffer);
        Stream sm = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(ms);
        //����Ҫָ��Ҫ����ĸ�ʽ��Ҫ����������
        StreamReader reader = new StreamReader(sm, System.Text.Encoding.UTF8);
        try
        {
            commonString = reader.ReadToEnd();
        }
        finally
        {
            sm.Close();
            ms.Close();
        }
        return commonString;
    }
}