namespace SDK.Lib
{
    public class TickProcessObjectObsolete
    {
        public ITickedObject mTickObject;
		public float mPriority;

        public TickProcessObjectObsolete()
        {
            this.mTickObject = null;
            this.mPriority = 0.0f;
        }
    }
}