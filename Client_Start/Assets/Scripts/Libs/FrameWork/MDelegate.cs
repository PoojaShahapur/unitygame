namespace SDK.Lib
{
    public delegate void MAction<T>(T arg1);
    public delegate TResult MFunc<T, TResult>(T arg1);
}