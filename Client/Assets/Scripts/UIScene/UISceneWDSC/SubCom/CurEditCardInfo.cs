using System.Collections.Generic;

namespace Game.UI
{
    /**
     * @brief 当前编辑的卡牌信息
     */
    public class CurEditCardInfo
    {
        public uint index;
        public List<uint> id = new List<uint>();

        public void clear()
        {
            index = 0;
            id.Clear();
        }
    }
}