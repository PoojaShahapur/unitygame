namespace SDK.Common
{
    public enum EventID
    {
        LOADED_EVENT = 0,
        FAILED_EVENT = 1,
    }

    public class Event
    {
        public string m_name;
        public object m_param;      // 回传的参数
    }
}
