namespace SDK.Common
{
    public interface ILocalFileSys
    {
        byte[] LoadFileByte(string path, string name);
        string getLocalWriteDir();
        string getLocalReadDir();
    }
}