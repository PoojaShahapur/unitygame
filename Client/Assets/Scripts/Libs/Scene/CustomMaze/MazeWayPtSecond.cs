namespace SDK.Lib
{
    public class MazeStartJumpPt : MazePtBase
    {
        public MazeStartJumpPt()
            : base(eMazePtType.eStart_Jump)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeStartShowPt : MazePtBase
    {
        public MazeStartShowPt()
            : base(eMazePtType.eStart_Show)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeStartDoorPt : MazePtBase
    {
        public MazeStartDoorPt()
            : base(eMazePtType.eStart_Door)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeEndJumpPt : MazePtBase
    {
        public MazeEndJumpPt()
            : base(eMazePtType.eEnd_Jump)
        {

        }

        //override public void moveToDestPos(MazePlayer mazePlayer_)
        //{
        //    base.moveToDestPos(mazePlayer_);
        //    mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        //}

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeEndHidePt : MazePtBase
    {
        public MazeEndHidePt()
            : base(eMazePtType.eEnd_Hide)
        {

        }

        //override public void moveToDestPos(MazePlayer mazePlayer_)
        //{
        //    base.moveToDestPos(mazePlayer_);
        //    mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        //}

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeEndDoorPt : MazePtBase
    {
        public MazeEndDoorPt()
            : base(eMazePtType.eEnd_Door)
        {

        }

        //override public void moveToDestPos(MazePlayer mazePlayer_)
        //{
        //    base.moveToDestPos(mazePlayer_);
        //    mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        //}

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }

    public class MazeEndDiePt : MazePtBase
    {
        public MazeEndDiePt()
            : base(eMazePtType.eEnd_Die)
        {

        }

        override public void moveToDestPos(MazePlayer mazePlayer_)
        {
            base.moveToDestPos(mazePlayer_);
            mazePlayer_.mazePlayerTrackAniControl.moveToDestPos(this);
        }

        override public MazePtBase clone()
        {
            MazeStartPt pt = new MazeStartPt();
            pt.copyFrom(this);
            return pt;
        }

        override public void copyFrom(MazePtBase rh)
        {
            base.copyFrom(rh);
        }
    }
}