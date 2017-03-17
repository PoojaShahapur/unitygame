namespace SDK.Lib
{
    public enum MsgRouteID
    {
        eMRIDSocketOpened,      // socket Opened
        eMRIDSocketClosed,      // socket Closed
        eMRIDLoadedWebRes,      // web 资源加载完成
        eMRIDThreadLog,         // 线程打日志

        eMRID_NewItem,
        eMRID_JoinTeam,
        eMRID_MoveTeam,
        eMRID_NewTrangle,

        eMRID_RemoveTrangle,
        eMRID_MoveTeamBullet,
        eMRID_TeamShooteMRID_NewItem,
        eMRID_TeamShoot,
        eMRID_BulletTimeOut,
        eMRID_HitTrangleCommand,
        eMRID_HitItemCommand,
        eMRID_TrangleInDangerZone,
        eMRID_TrangleOutDangerZone,
        eMRID_TrangleTeamLeave,
    }
}