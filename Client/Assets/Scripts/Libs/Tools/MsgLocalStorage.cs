using System.IO;
using UnityEngine;

public class MsgLocalStorage
{
#if UNITY_EDITOR
    static string streamPath = Application.streamingAssetsPath;
#else
    static string streamPath = Application.persistentDataPath;
#endif

    static public byte[] readFileAllBytes(string fileName)
    {
        byte[] ret = null;
        try
        {
            ret = File.ReadAllBytes(streamPath + "/" + fileName);
        }
        catch
        {
            Debug.Log("Not Find File " + fileName);
        }

        return ret;
    }

    static public void writeBytesToFile(string fileName, byte[] buf)
    {
        File.WriteAllBytes(streamPath + "/" + fileName, buf);
    }

    static public string readFileAllText(string fileName)
    {
        string ret = null;
        try
        {
            ret = File.ReadAllText(streamPath + "/" + fileName);
        }
        catch
        {
            Debug.Log("Not Find File " + fileName);
        }

        return ret;
    }

    static public void writeTextToFile(string fileName, string text)
    {
        File.WriteAllText(streamPath + "/" + fileName, text);
    }

    static public LuaStringBuffer readLuaBufferToFile(string fileName)
    {
        byte[] ret = readFileAllBytes(fileName.ToString());
        LuaStringBuffer buffer = new LuaStringBuffer(ret);
        return buffer;
    }

    static public  void writeLuaBufferToFile(string fileName, LuaStringBuffer text)
    {
        writeBytesToFile(fileName, text.buffer);
    }
}