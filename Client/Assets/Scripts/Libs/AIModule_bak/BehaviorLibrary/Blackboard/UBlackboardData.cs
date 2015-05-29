using System.Collections.Generic;
namespace BehaviorLibrary
{
    /**
     * @brief ºÚºÐÊý¾Ý
     */
    public class UBlackboardData
    {
        private Dictionary<string, object> datasStore = new Dictionary<string, object>();

        public bool HaveDatas
        {
            get
            {
                return datasStore.Keys.Count > 0;
            }
        }

        public object GetData(string name)
        {
            return datasStore[name];
        }

        public T GetData<T>(string name)
        {
            if (!datasStore.ContainsKey(name))
                return default(T);
            return (T)datasStore[name];
        }

        public void AddData(string name, object data)
        {
            var srcHad = datasStore.ContainsKey(name);
            datasStore[name] = data;
        }

        public object RemoveData(string name)
        {
            if (!datasStore.ContainsKey(name))
                return null;
            object result = datasStore[name];
            datasStore.Remove(name);
            return result;
        }

        public void UpdateData(string name)
        {
            if (!datasStore.ContainsKey(name))
                return;
        }
    }
}