using UnityEngine;
using rpc;
using plane;
using GameBox.Framework;
using GameBox.Service.AssetManager;

namespace Giant
{
    public class FightScene : CommandScene
    {
        //场景上的信息
        private EnterRoomResponse room;

        public uint ServerUpdateInterval()
        {
            return room.syncinterval;
        }
        private PlayerInfo myInfo;
        public UIFight uiFight;
        public FightScene(EnterRoomResponse room)
        {
            this.room = room;
            this.myInfo = room.players.Find(player =>{
                return player.id == room.ms_and_id.id;
            });
        }

        public override void OnEnter()
        {
            SceneObject.scene = this;
            base.OnEnter();

            var asset = Game.instance.assetManager.Load("UI/UIFight.prefab", AssetType.PREFAB);
            var obj = GameObject.Instantiate(asset.Cast<GameObject>());
            asset.Dispose();
            var controller = obj.GetComponent<UIFight>();
            this.uiFight = controller;
            newItem.pos = new Vector2();
            foreach (var food in room.foods)
            {
                newItem.objid = food.food_id;
                newItem.pos.Set(food.x, food.y);
                HandleSceneCommand(newItem);
            }

            foreach (var player in room.players)
            {
                this.JoinTeam(player, player.id == room.ms_and_id.id);
                this.uiFight.OnNewPlayer(player.id);
            }
            this.myTeam.controller = controller;


            var handler = Game.instance.handler;
            handler.Register<MoveToSmallPlaneMsg, EmptyMsg>("MoveTo");
            handler.Register<TurnToMsg, EmptyMsg>("TurnTo");
            handler.Register<EmptyMsg, OkMsg>("Fire");
            handler.Register<HitMsg, EmptyMsg>("Hit");
            handler.Register<EatMsg, OkMsg>("Eat");
            handler.Register<PlaneMsg, EmptyMsg>("Remove");
            handler.Register<OkMsg, EmptyMsg>("StopMove");

            handler.BeginService("plane.PlanePush");
            handler.Register<MoveToBcMsgRoom, EmptyMsg>("PackPlayerMoveTo", OnFrameSyn);
            handler.Register<FireBcMsg, EmptyMsg>("Fire", OnFire);
            handler.Register<HitBcMsg, EmptyMsg>("Hit", OnHit);
            handler.Register<EatBcMsg, EmptyMsg>("Eat", OnEat);
            handler.Register<PlayerInfo, EmptyMsg>("PlayerEnter", OnPlayerEnter);
            handler.Register<FoodMsg, EmptyMsg>("NewFood", OnNewFood);
            handler.Register<PlaneBcMsg, EmptyMsg>("NewPlane", OnNewPlane);
            handler.Register<PlaneBcMsg, EmptyMsg>("RemovePlane", OnRemovePlane);
            handler.Register<MsAndId, EmptyMsg>("PlayerExit", OnPlayerExit);
            synTime = Time.time;
        }

        //服务器同步帧
        private EmptyMsg emptyMsg = new EmptyMsg();
        //private FrameSyn frameSyn = new FrameSyn();
        private float synTime = 0;
        private EmptyMsg OnFrameSyn(MoveToBcMsgRoom msg)
        {
            //临时解决切后台卡死问题
            if (Game.instance.IsPause)
                return emptyMsg;
            //Debug.Log("SYN TIME:" + (Time.time - synTime));
            //synTime = Time.time;
            //frameSyn.teamMoves.Clear();
            //frameSyn.sframeid = msg.curframe_and_roomid.ms;
            foreach (var mov in msg.moves)
            {
                var moveTeam = new MoveTeam();
                var move_to = mov.move_to;
                moveTeam.teamID = mov.ms_and_id.id;
                moveTeam.sframeid = msg.curframe_and_roomid.ms;
                moveTeam.angle = move_to.angle;
                moveTeam.teamPos = new Vector2(move_to.x, move_to.y);
                if (mov.move_to.movings != null)
                {
                    foreach (var tMove in mov.move_to.movings.movings)
                    {
                        moveTeam.moves.Add(tMove.plane_id, new Vector2(tMove.x, tMove.y));
                    }
                }
                HandleSceneCommand(moveTeam);
            }
            return emptyMsg;
        }

        private EmptyMsg OnFire(FireBcMsg msg)
        {
            TeamShoot teamShoot = new TeamShoot();
            plane.FireMsg fire = msg.fire;
            var frame_and_id = msg.ms_and_id;
            teamShoot.teamID = frame_and_id.id;
            teamShoot.sframeid = frame_and_id.ms;
            teamShoot.bulletTeamID = fire.bullet_team_id;
            teamShoot.speed = 7f;
            teamShoot.timeOut = 2;
            teamShoot.angle = fire.angle;
            teamShoot.pos = new Vector2(fire.x, fire.y);
            HandleSceneCommand(teamShoot);
            return emptyMsg;
        }

        //命中其他玩家
        private HitTrangleCommand hitTrangle = new HitTrangleCommand();
        private EmptyMsg OnHit(HitBcMsg msg)
        {
            //if (!_IsMainTeam(msg.ms_and_id.id))
            //{
                HandleSceneCommand(msg.ms_and_id.id,msg.hit);
            //}
            return emptyMsg;
        }

        //命中食物
        private HitItemCommand hitFood = new HitItemCommand();
        private EmptyMsg OnEat(EatBcMsg msg)
        {
            //if (!_IsMainTeam(msg.ms_and_id.id))
            //{
                HandleSceneCommand(msg.ms_and_id.id, msg.eat);
            //}
            return emptyMsg;
        }

        //新玩家加入
        private EmptyMsg OnPlayerEnter(PlayerInfo msg)
        {
            this.JoinTeam(msg,false);
            uiFight.OnNewPlayer(msg.id);
            return emptyMsg;
        }

        //刷新食物
        private NewItem newItem = new NewItem();
        private EmptyMsg OnNewFood(FoodMsg msg)
        {
            newItem.objid = msg.food_id;
            newItem.pos = new Vector2(msg.x, msg.y);
            HandleSceneCommand(newItem);
            return emptyMsg;
        }

        //添加飞机 由服务器通知
        private EmptyMsg OnNewPlane(PlaneBcMsg msg)
        {
            //if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.@new,true);
            }
            return emptyMsg;
        }

        private EmptyMsg OnRemovePlane(PlaneBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.@new, false);
            }
            return emptyMsg;
        }

        private EmptyMsg OnPlayerExit(MsAndId msg)
        {
            if (_IsMainTeam(msg.id))
            {
                Game.instance.Disconnect();
            }
            else
            {
                var cmd = new TrangleTeamLeave();
                cmd.teamID = msg.id;
                HandleSceneCommand(cmd);
                uiFight.OnRemovePlayer(msg.id);
            }
            return emptyMsg;
        }

        protected override void OnTrangleTeamChange(uint teamid)
        {
            uiFight.OnPlayerChange(teamid);
        }

        public void JoinTeam(PlayerInfo player,bool bMySelf)
        {
            JoinTeam info = new JoinTeam();
            info.name = player.name;
            info.teamID = player.id;
            info.shootCD = 1.0f;
            info.moveSpeed = player.speed;
            info.turnSpeed = 720f;
            //角度位置
            info.angle = player.move.angle;
            info.pos = new Vector2(player.move.x, player.move.y);
            //小飞机 
            info.trangles.Clear();
            foreach (var trangle in player.move.movings.movings)
            {
                info.trangles.Add(trangle.plane_id, new Vector2(trangle.x, trangle.y));
            }
            info.isself = bMySelf;
            HandleSceneCommand(info);
        }

        public override void OnLeave()
        {
            if (uiFight)
                uiFight.Close();
            var handler = Game.instance.handler;
            handler.EndService("plane.Plane");
            handler.EndService("plane.PlanePush");
            base.OnLeave();
            SceneObject.scene = null;
        }

        public override void OnUpdate(float delta)
        {
            if (this.myTeam != null)
            {
                var cameraPos = cameraT.position;
                var myTeamPos = this.myTeam.pos;
                cameraPos.x = myTeamPos.x;
                cameraPos.y = myTeamPos.y;
                cameraT.position = cameraPos;
                base.OnUpdate(delta);

                if (this.myTeam.IsTrangleEmpty)
                {
                    Game.instance.Disconnect();
                }
            }
        }

        public override void OnTriggerEnterDangerZone(Trangle trangle)
        {
            var cmd = new TrangleInDangerZone();
            cmd.teamID = trangle.team.objctid;
            cmd.trangleid = trangle.objctid;
            cmd.liveTime = 1.0f;
            HandleSceneCommand(cmd);
        }

        public override void OnTriggerExitDangerZone(Trangle trangle)
        {
            var cmd = new TrangleOutDangerZone();
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

        private NewTrangle newTrangle = new NewTrangle();
        private RemoveTrangle removeTrangle = new RemoveTrangle();
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

   

        protected override void OnJoinTeam(JoinTeam cmd)
        {
            if (cmd.isself)
            {
                this.myTeam = SceneObject.Create<MainTrangleTeam>(mapT, "Prefabs/TrangleTeam", cmd);
                this.myTeam.objctid = cmd.teamID;
                teams.Add(cmd.teamID, myTeam);
            }
            else
            {
                base.OnJoinTeam(cmd);
            }
        }
    }
}
