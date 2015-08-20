namespace SDK.Lib
{
    public class MazeBombPt : MazePtBase
    {
        /**
         * @brief 爆炸点
         */
        public MazeBombPt()
            : base(eMazePtType.eBomb)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeBombPt pt = new MazeBombPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }
}