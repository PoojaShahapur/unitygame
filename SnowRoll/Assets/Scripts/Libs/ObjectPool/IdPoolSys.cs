namespace SDK.Lib
{
    /**
     * @brief 有 Id 的缓存池
     */
    public class IdPoolSys
    {
        protected MDictionary<string, MList<IRecycle>> mId2PoolDic;

        public IdPoolSys()
        {
            this.mId2PoolDic = new MDictionary<string, MList<IRecycle>>();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public IRecycle getObject(string id)
        {
            IRecycle ret = null;

            if (this.mId2PoolDic.ContainsKey(id))
            {
                if (this.mId2PoolDic[id].Count() > 0)
                {
                    ret = this.mId2PoolDic[id][0];
                    this.mId2PoolDic[id].RemoveAt(0);
                }
            }

            return ret;
        }

        public void deleteObj(string id, IRecycle obj)
        {
            if (!this.mId2PoolDic.ContainsKey(id))
            {
                this.mId2PoolDic[id] = new MList<IRecycle>();
            }

            this.mId2PoolDic[id].Add(obj);
        }
    }
}