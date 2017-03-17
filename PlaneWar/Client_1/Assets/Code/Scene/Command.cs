using UnityEngine;
using System.Collections.Generic;

namespace Giant
{
    public class Command : SDK.Lib.MsgRouteBase
    {
        public uint frameid; //帧id

        public Command()
            : base(SDK.Lib.MsgRouteID.eMRID_NewItem, SDK.Lib.MsgRouteType.eMRT_SCENE_COMMAND)
        {
            if (SceneObject.scene != null)
                frameid = SceneObject.scene.frame;
        }
    }

    public class NewItem : Command
    {
        public uint objid;
        public Vector2 pos;

        public NewItem()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_NewItem;
        }
    }

    //队伍命令
    public class TeamCommand : Command
    {
        public uint teamID;
    }

    //队伍加入
    public class JoinTeam : TeamCommand
    {
        public MoveTeam move = new MoveTeam();
        public string name;
        public float shootCD;
        public float turnSpeed;
        public float moveSpeed;
        public float invincibleTime;
        public bool isself;

        public JoinTeam()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_JoinTeam;
        }
    }

    public class MoveTeam : TeamCommand
    {
        public float angle;
        public Vector2 teamPos;
        public Dictionary<uint,Vector2> moves = new Dictionary<uint, Vector2>();

        public MoveTeam()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_MoveTeam;
        }
    }

    public class NewTrangle : TeamCommand
    {
        public uint trangleid;

        public NewTrangle()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_NewTrangle;
        }
    }

    public class RemoveTrangle : TeamCommand
    {
        public uint trangleid;

        public RemoveTrangle()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_RemoveTrangle;
        }
    }

    public class MoveTeamBullet : TeamCommand
    {
        public uint bulletID;
        public Vector2 pos;

        public MoveTeamBullet()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_MoveTeamBullet;
        }
    }

    public class TeamShoot : TeamCommand
    {
        public uint bulletTeamID;
        public Vector2 pos;
        public float angle;
        public float timeOut;
        public float speed;

        public TeamShoot()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_TeamShoot;
        }
    }

    public class BulletTimeOut : TeamCommand
    {
        public uint bulletID;

        public BulletTimeOut()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_BulletTimeOut;
        }
    }

    public class HitTrangleCommand : TeamCommand
    {
        public uint bulletTeamid;
        public uint bulletid;
        public uint targetTeamID;
        public uint targetTrangleID;

        public HitTrangleCommand()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_HitTrangleCommand;
        }
    }

    public class HitItemCommand : TeamCommand
    {
        public uint bulletTeamid;
        public uint bulletid;
        public uint itemID;

        public HitItemCommand()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_HitItemCommand;
        }
    }

    public class TrangleInDangerZone : TeamCommand
    {
        public uint trangleid;
        public float liveTime; //在危险区域存活时间

        public TrangleInDangerZone()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_TrangleInDangerZone;
        }
    }

    public class TrangleOutDangerZone : TeamCommand
    {
        public uint trangleid;

        public TrangleOutDangerZone()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_TrangleOutDangerZone;
        }
    }

    public class TrangleTeamLeave : TeamCommand
    {
        public TrangleTeamLeave()
        {
            this.mMsgID = SDK.Lib.MsgRouteID.eMRID_TrangleTeamLeave;
        }
    }
}

