using System.Collections.Generic;

namespace SDK.Lib
{
    /**
     * @brief 帧碰撞管理器，管理每一帧碰撞的信息，每一帧结束清除
     */
    public class FrameCollideMgr
    {
        protected Dictionary<string, string> mId2Id;   // 记录同一帧中发生碰撞的两个对象

        public FrameCollideMgr()
        {
            mId2Id = new Dictionary<string, string>();
        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void clear()
        {
            mId2Id.Clear();
        }

        // 添加碰撞
        public void addCollideById(string aId, string bId)
        {
            mId2Id[aId] = bId;
            mId2Id[bId] = aId;
        }

        // 判断是否在当前帧中发生了碰撞
        public bool isCollidedInCurFrame(string aId, string bId)
        {
            if ((this.mId2Id.ContainsKey(aId) && this.mId2Id[aId] == bId) ||
                (this.mId2Id.ContainsKey(bId) && mId2Id[bId] == aId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 获取或者添加是否在当前帧发生碰撞
        public bool isOrAddCollidedInCurFrame(string aId, string bId)
        {
            if ((this.mId2Id.ContainsKey(aId) && this.mId2Id[aId] == bId) ||
                (this.mId2Id.ContainsKey(bId) && mId2Id[bId] == aId))
            {
                return true;
            }
            else
            {
                this.mId2Id[aId] = bId;
                this.mId2Id[bId] = aId;

                return false;
            }
        }
    }
}