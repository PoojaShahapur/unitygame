namespace SDK.Lib
{
    public class TickProcessObject
    {
        public ITickedObject m_tickObject;
		public float m_priority;

        public TickProcessObject()
        {
            m_tickObject = null;
            m_priority = 0.0f;
        }
    }
}