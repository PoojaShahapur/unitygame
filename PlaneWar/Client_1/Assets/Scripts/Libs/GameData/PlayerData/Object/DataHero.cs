namespace SDK.Lib
{
    /**
     * @brief 玩家所有的 hero 数据
     */
    public class DataHero
    {
        //场景上的信息
        public plane.EnterRoomResponse mRoom;
        public plane.PlayerInfo mMyInfo;

        public DataHero()
        {

        }

        public void init()
        {

        }

        public void dispose()
        {

        }

        public void setRoomInfo(plane.EnterRoomResponse room)
        {
            this.mRoom = room;
            this.mMyInfo = room.players.Find(player => {
                return player.id == room.ms_and_id.id;
            });
        }
    }
}