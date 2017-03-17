namespace SDK.Lib
{
    public class LoadedWebResMR : MsgRouteBase
    {
        public ITask mTask;

        public LoadedWebResMR()
            : base(MsgRouteID.eMRIDLoadedWebRes)
        {

        }

        override public void resetDefault()
        {
            mTask = null;
        }
    }
}
