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
            handler.Register<plane.MoveToMsg, rpc.EmptyMsg>("MoveTo");
            handler.Register<rpc.EmptyMsg, plane.OkMsg>("Fire");
            handler.Register<plane.HitMsg, rpc.EmptyMsg>("Hit");
            handler.Register<plane.EatMsg, plane.OkMsg>("Eat");
            handler.Register<plane.PlaneMsg, rpc.EmptyMsg>("New");
            handler.Register<plane.PlaneMsg, rpc.EmptyMsg>("Remove");

            handler.BeginService("plane.PlanePush");
            handler.Register<plane.MoveToBcMsg, rpc.EmptyMsg>("MoveTo", OnMove);
            handler.Register<plane.FireBcMsg, rpc.EmptyMsg>("Fire", OnFire);
            handler.Register<plane.HitBcMsg, rpc.EmptyMsg>("Hit", OnHit);
            handler.Register<plane.EatBcMsg, rpc.EmptyMsg>("Eat", OnEat);
            handler.Register<plane.PlayerInfo, rpc.EmptyMsg>("PlayerEnter", OnPlayerEnter);
            handler.Register<plane.FoodMsg, rpc.EmptyMsg>("NewFood", OnNewFood);
            handler.Register<plane.PlaneBcMsg, rpc.EmptyMsg>("NewPlane", OnNewPlane);
            handler.Register<plane.PlaneBcMsg, rpc.EmptyMsg>("RemovePlane", OnRemovePlane);
            handler.Register<plane.MsAndId, rpc.EmptyMsg>("PlayerExit", OnPlayerExit);
        }

        private rpc.EmptyMsg emptyMsg = new rpc.EmptyMsg();
        private rpc.EmptyMsg OnMove(plane.MoveToBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.move_to);
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
                HandleSceneCommand(msg.ms_and_id.id, msg.plane, true);
            }
            return emptyMsg;
        }

        private rpc.EmptyMsg OnRemovePlane(plane.PlaneBcMsg msg)
        {
            if (!Ctx.mInstance.mPlayerMgr.isHeroByThisId(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.plane, false);
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

        public void JoinTeam(plane.PlayerInfo player, bool bMySelf)
        {
            var info = new Giant.JoinTeam();
            info.name = player.name;
            info.teamID = player.id;
            info.turnSpeed = 720;
            info.moveSpeed = 2.0f;
            info.shootCD = 1.0f;
            info.invincibleTime = 5;
            info.isself = bMySelf;

            if (player.move != null)
            {
                info.move.teamPos = new UnityEngine.Vector2(player.move.x, player.move.y);
                info.move.angle = player.move.angle;
                for (int i = 0; i < player.move.movings.Count; ++i)
                {
                    var move = player.move.movings[i];
                    info.move.moves.Add(move.plane_id, new UnityEngine.Vector2(move.x, move.y));
                }
            }
            HandleSceneCommand(info);

            if(bMySelf)
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

        private Giant.MoveTeam moveTeam = new Giant.MoveTeam();
        public void HandleSceneCommand(uint teamid, plane.MoveToMsg move_to)
        {
            var movings = move_to.movings;
            if (movings.Count > 0)
            {
                moveTeam.teamID = teamid;
                moveTeam.angle = move_to.angle;
                moveTeam.teamPos = new UnityEngine.Vector2(move_to.x, move_to.y);
                moveTeam.moves.Clear();
                for (int i = 0; i < movings.Count; ++i)
                    moveTeam.moves.Add(movings[i].plane_id, new UnityEngine.Vector2(movings[i].x, movings[i].y));
                HandleSceneCommand(moveTeam);
            }
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

        protected void OnTrangleTeamChange(uint teamid)
        {
            //uiFight.OnPlayerChange(teamid);
        }
    }
}