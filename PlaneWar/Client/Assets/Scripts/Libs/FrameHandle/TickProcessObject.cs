namespace SDK.Lib
{
    public class TickProcessObject
    {
        public ITickedObject mTickObject;
		public float mPriority;

        public TickProcessObject()
        {
            this.mTickObject = null;
            this.mPriority = 0.0f;
        }
    }
}