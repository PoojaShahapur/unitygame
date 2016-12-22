namespace SDK.Lib
{
    /**
	 * @brief 玩家排名信息
	 */
    public struct RankInfo
    {
        public int rank;
        public uint eid;
        public string name;
        public float radius;
        public uint swallownum;

        public RankInfo(int _rank=0, uint _eid=0, string _name="", float _radius=0.0f, uint _swallownum=0)
        {
            rank = _rank;
            eid = _eid;
            name = _name;
            radius = _radius;
            swallownum = _swallownum;
        }
    }
}
