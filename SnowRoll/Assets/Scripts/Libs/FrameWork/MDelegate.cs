namespace SDK.Lib
{
    public delegate void MAction();
    public delegate void MAction<T>(T arg1);
    public delegate TResult MFunc<T, TResult>(T arg1);
    public delegate void MAction4<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3);
}