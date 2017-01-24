namespace SDK.Lib
{
    public class GObject
    {
        protected string mTypeId;     // 名字

        public GObject()
        {
            this.mTypeId = "GObject";
        }

        public string getTypeId()
        {
            return this.mTypeId;
        }
    }
}