namespace SDK.Lib
{
    public class MazeDiePt : MazePtBase
    {
        /**
         * @brief 死亡点
         */
        public MazeDiePt()
            : base(eMazePtType.eDie)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeDiePt pt = new MazeDiePt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }
}