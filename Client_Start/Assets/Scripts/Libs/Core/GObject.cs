namespace SDK.Lib
{
    public class GObject
    {
        protected string mTypeId;     // 名字

        public GObject()
        {
            mTypeId = "GObject";
        }

        public string getTypeId()
        {
            return mTypeId;
        }
    }
}