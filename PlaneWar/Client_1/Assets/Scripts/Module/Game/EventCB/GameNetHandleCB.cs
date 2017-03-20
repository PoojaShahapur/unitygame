using SDK.Lib;

namespace Game.Game
{
    /**
     * @brief 游戏网络处理
     */
    public class GameNetHandleCB : NetModuleDispHandle
    {
        public GameNetHandleCB()
        {

        }

        public override void init()
        {
            base.init();

            NetCmdDispHandle cmdHandle = null;
            cmdHandle = new GameTimeCmdHandle();
            this.addCmdHandle(stNullUserCmd.TIME_USERCMD, cmdHandle, cmdHandle.call);
        }

        public override void dispose()
        {
            base.dispose();

            Ctx.mInstance.mServerHandler_GB.EndService("plane.Plane");
            Ctx.mInstance.mServerHandler_GB.EndService("plane.PlanePush");
        }

        public void init_GB()
        {
            var handler = Ctx.mInstance.mServerHandler_GB;
            handler.Register<plane.MoveToSmallPlaneMsg, rpc.EmptyMsg>("MoveTo");
            handler.Register<plane.TurnToMsg, rpc.EmptyMsg>("TurnTo");
            handler.Register<rpc.EmptyMsg, plane.OkMsg>("Fire");
            handler.Register<plane.HitMsg, rpc.EmptyMsg>("Hit");
            handler.Register<plane.EatMsg, plane.OkMsg>("Eat");
            handler.Register<plane.PlaneMsg, rpc.EmptyMsg>("Remove");
            handler.Register<plane.OkMsg, rpc.EmptyMsg>("StopMove");

            handler.BeginService("plane.PlanePush");
            handler.Register<plane.MoveToBcMsgRoom, rpc.EmptyMsg>("PackPlayerMoveTo", OnFrameSyn);
            handler.Register<plane.FireBcMsg, rpc.EmptyMsg>("Fire", OnFire);
            handler.Register<plane.HitBcMsg, rpc.EmptyMsg>("Hit", OnHit);
            handler.Register<plane.EatBcMsg, rpc.EmptyMsg>("Eat", OnEat);
            handler.Register<plane.PlayerInfo, rpc.EmptyMsg>("PlayerEnter", OnPlayerEnter);
            handler.Register<plane.FoodMsg, rpc.EmptyMsg>("NewFood", OnNewFood);
            handler.Register<plane.PlaneBcMsg, rpc.EmptyMsg>("NewPlane", OnNewPlane);
            handler.Register<plane.PlaneBcMsg, rpc.EmptyMsg>("RemovePlane", OnRemovePlane);
            handler.Register<plane.MsAndId, rpc.EmptyMsg>("PlayerExit", OnPlayerExit);
            synTime = UnityEngine.Time.time;
        }

        //服务器同步帧
        private rpc.EmptyMsg emptyMsg = new rpc.EmptyMsg();
        //private FrameSyn frameSyn = new FrameSyn();
        private float synTime = 0;
        private rpc.EmptyMsg OnFrameSyn(plane.MoveToBcMsgRoom msg)
        {
            //Debug.Log("SYN TIME:" + (Time.time - synTime));
            //synTime = Time.time;
            //frameSyn.teamMoves.Clear();
            //frameSyn.sframeid = msg.curframe_and_roomid.ms;
            foreach (var mov in msg.moves)
            {
                var moveTeam = new Giant.MoveTeam();
                var move_to = mov.move_to;
                moveTeam.teamID = mov.ms_and_id.id;
                moveTeam.sframeid = msg.curframe_and_roomid.ms;
                moveTeam.angle = move_to.angle;
                moveTeam.teamPos = new UnityEngine.Vector2(move_to.x, move_to.y);
                if (mov.move_to.movings != null)
                {
                    foreach (var tMove in mov.move_to.movings.movings)
                    {
                        moveTeam.moves.Add(tMove.plane_id, new UnityEngine.Vector2(tMove.x, tMove.y));
                    }
                }
                HandleSceneCommand(moveTeam);
            }
            return emptyMsg;
        }

        private rpc.EmptyMsg OnFire(plane.FireBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.fire);
            }
            return emptyMsg;
        }

        private Giant.HitTrangleCommand hitTrangle = new Giant.HitTrangleCommand();
        private rpc.EmptyMsg OnHit(plane.HitBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.hit);
            }
            return emptyMsg;
        }

        private Giant.HitItemCommand hitFood = new Giant.HitItemCommand();
        private rpc.EmptyMsg OnEat(plane.EatBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.eat);
            }
            return emptyMsg;
        }

        private rpc.EmptyMsg OnPlayerEnter(plane.PlayerInfo msg)
        {
            this.JoinTeam(msg, false);
            //uiFight.OnNewPlayer(msg.id);
            return emptyMsg;
        }

        private Giant.NewItem newItem = new Giant.NewItem();
        private rpc.EmptyMsg OnNewFood(plane.FoodMsg msg)
        {
            newItem.objid = msg.food_id;
            newItem.pos = new UnityEngine.Vector2(msg.x, msg.y);
            HandleSceneCommand(newItem);
            return emptyMsg;
        }

        private rpc.EmptyMsg OnNewPlane(plane.PlaneBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.@new, true);
            }
            return emptyMsg;
        }

        private rpc.EmptyMsg OnRemovePlane(plane.PlaneBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.@new, false);
            }
            return emptyMsg;
        }

        private rpc.EmptyMsg OnPlayerExit(plane.MsAndId msg)
        {
            if (Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.id))
            {
                Ctx.mInstance.mNetEventHandle.Disconnect();
            }
            else
            {
                var cmd = new Giant.TrangleTeamLeave();
                cmd.teamID = msg.id;
                HandleSceneCommand(cmd);
                //uiFight.OnRemovePlayer(msg.id);
            }
            return emptyMsg;
        }

        protected void OnTrangleTeamChange(uint teamid)
        {
            //uiFight.OnPlayerChange(teamid);
        }

        private Giant.JoinTeam info = new Giant.JoinTeam();
        public void JoinTeam(plane.PlayerInfo player, bool bMySelf)
        {
            info.name = player.name;
            info.teamID = player.id;
            info.shootCD = 1.0f;
            info.moveSpeed = 5.0f;
            info.turnSpeed = 720f;
            //角度位置
            info.angle = player.move.angle;
            info.pos = new UnityEngine.Vector2(player.move.x, player.move.y);
            //小飞机 
            info.trangles.Clear();
            foreach (var trangle in player.move.movings.movings)
            {
                info.trangles.Add(trangle.plane_id, new UnityEngine.Vector2(trangle.x + player.move.x, trangle.y + player.move.y));
            }
            info.isself = bMySelf;
            HandleSceneCommand(info);

            if (bMySelf)
            {
                Ctx.mInstance.mGlobalDelegate.mMainChildChangedDispatch.dispatchEvent(null);
            }
        }


        public void OnTriggerEnterDangerZone(Giant.Trangle trangle)
        {
            var cmd = new Giant.TrangleInDangerZone();
            cmd.teamID = trangle.team.objctid;
            cmd.trangleid = trangle.objctid;
            cmd.liveTime = 1.0f;
            HandleSceneCommand(cmd);
        }

        public void OnTriggerExitDangerZone(Giant.Trangle trangle)
        {
            var cmd = new Giant.TrangleOutDangerZone();
            cmd.teamID = trangle.team.objctid;
            cmd.trangleid = trangle.objctid;
            HandleSceneCommand(cmd);
        }


        public void HandleSceneCommand(uint teamid, plane.EatMsg eat)
        {
            hitFood.teamID = teamid;
            hitFood.bulletTeamid = eat.bullet_team_id;
            hitFood.bulletid = eat.bullet_id;
            hitFood.itemID = eat.food_id;
            HandleSceneCommand(hitFood);
        }

        public void HandleSceneCommand(uint teamid, plane.HitMsg hit)
        {
            hitTrangle.teamID = teamid;
            hitTrangle.bulletTeamid = hit.bullet_team_id;
            hitTrangle.bulletid = hit.bullet_id;
            hitTrangle.targetTrangleID = hit.plane_id;
            hitTrangle.targetTeamID = hit.target_user_id;
            HandleSceneCommand(hitTrangle);
        }

        private Giant.NewTrangle newTrangle = new Giant.NewTrangle();
        private Giant.RemoveTrangle removeTrangle = new Giant.RemoveTrangle();
        public void HandleSceneCommand(uint teamid, plane.PlaneMsg plane, bool add)
        {
            if (add)
            {
                newTrangle.trangleid = plane.plane_id;
                newTrangle.teamID = teamid;
                HandleSceneCommand(newTrangle);
            }
            else
            {
                removeTrangle.trangleid = plane.plane_id;
                removeTrangle.teamID = teamid;
                HandleSceneCommand(removeTrangle);
            }
            this.OnTrangleTeamChange(teamid);
        }

        private Giant.TeamShoot teamShoot = new Giant.TeamShoot();
        public void HandleSceneCommand(uint teamid, plane.FireMsg fire)
        {
            teamShoot.teamID = teamid;
            teamShoot.bulletTeamID = fire.bullet_team_id;
            teamShoot.speed = 4.5f;
            teamShoot.timeOut = 2;
            teamShoot.angle = fire.angle;
            teamShoot.pos = new UnityEngine.Vector2(fire.x, fire.y);
            HandleSceneCommand(teamShoot);
        }

        private static object[] cmdParam = new object[1];
        public void HandleSceneCommand(Giant.Command cmd)
        {
            //if (!TryHandleSceneCommand(this, cmd))
            //{
            //    var tCmd = cmd as Giant.TeamCommand;
            //    if (tCmd != null)
            //    {
            //        //Giant.TrangleTeam team;
            //        //if (teams.TryGetValue(tCmd.teamID, out team))
            //        //{
            //        //    TryHandleSceneCommand(team, cmd);
            //        //}

            //        Player player = null;
            //        player = Ctx.mInstance.mPlayerMgr.getEntityByThisId(tCmd.teamID) as Player;

            //        if(null != player)
            //        {
            //            TryHandleSceneCommand(player, cmd);
            //        }
            //    }
            //}

            // 分发 Route
            Ctx.mInstance.mSysMsgRoute.addMsg(cmd);
        }

        private bool TryHandleSceneCommand(object obj, Giant.Command cmd)
        {
            var method = obj.GetType().GetMethod("On" + cmd.GetType().Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            if (method != null)
            {
                cmdParam[0] = cmd;
                method.Invoke(obj, cmdParam);
                return true;
            }
            return false;
        }
    }
}