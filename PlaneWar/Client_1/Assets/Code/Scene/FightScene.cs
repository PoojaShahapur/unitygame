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
            handler.Register<MoveToMsg, EmptyMsg>("MoveTo");
            handler.Register<EmptyMsg, OkMsg>("Fire");
            handler.Register<HitMsg, EmptyMsg>("Hit");
            handler.Register<EatMsg, OkMsg>("Eat");
            handler.Register<PlaneMsg, EmptyMsg>("New");
            handler.Register<PlaneMsg, EmptyMsg>("Remove");

            handler.BeginService("plane.PlanePush");
            handler.Register<MoveToBcMsg, EmptyMsg>("MoveTo", OnMove);
            handler.Register<FireBcMsg, EmptyMsg>("Fire", OnFire);
            handler.Register<HitBcMsg, EmptyMsg>("Hit", OnHit);
            handler.Register<EatBcMsg, EmptyMsg>("Eat", OnEat);
            handler.Register<PlayerInfo, EmptyMsg>("PlayerEnter", OnPlayerEnter);
            handler.Register<FoodMsg, EmptyMsg>("NewFood", OnNewFood);
            handler.Register<PlaneBcMsg, EmptyMsg>("NewPlane", OnNewPlane);
            handler.Register<PlaneBcMsg, EmptyMsg>("RemovePlane", OnRemovePlane);
            handler.Register<MsAndId, EmptyMsg>("PlayerExit", OnPlayerExit);
        }

        private EmptyMsg emptyMsg = new EmptyMsg();
        private EmptyMsg OnMove(MoveToBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.move_to);
            }
            return emptyMsg;
        }

        private EmptyMsg OnFire(FireBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.fire);
            }
            return emptyMsg;
        }

        private HitTrangleCommand hitTrangle = new HitTrangleCommand();
        private EmptyMsg OnHit(HitBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id,msg.hit);
            }
            return emptyMsg;
        }

        private HitItemCommand hitFood = new HitItemCommand();
        private EmptyMsg OnEat(EatBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.eat);
            }
            return emptyMsg;
        }

        private EmptyMsg OnPlayerEnter(PlayerInfo msg)
        {
            this.JoinTeam(msg,false);
            uiFight.OnNewPlayer(msg.id);
            return emptyMsg;
        }

        private NewItem newItem = new NewItem();
        private EmptyMsg OnNewFood(FoodMsg msg)
        {
            newItem.objid = msg.food_id;
            newItem.pos = new Vector2(msg.x, msg.y);
            HandleSceneCommand(newItem);
            return emptyMsg;
        }

        private EmptyMsg OnNewPlane(PlaneBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.plane,true);
            }
            return emptyMsg;
        }

        private EmptyMsg OnRemovePlane(PlaneBcMsg msg)
        {
            if (!_IsMainTeam(msg.ms_and_id.id))
            {
                HandleSceneCommand(msg.ms_and_id.id, msg.plane,false);
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
            var info = new JoinTeam();
            info.name = player.name;
            info.teamID = player.id;
            info.turnSpeed = 1440;
            info.moveSpeed = 3.5f;
            info.shootCD = 1.0f;
            info.invincibleTime = 5;
            info.isself = bMySelf;
            
            if (player.move != null)
            {
                info.move.teamPos = new Vector2(player.move.x, player.move.y);
                info.move.angle = player.move.angle;
                for (int i = 0; i < player.move.movings.Count; ++i)
                {
                    var move = player.move.movings[i];
                    info.move.moves.Add(move.plane_id, new Vector2(move.x, move.y));
                }
            }
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

        private MoveTeam moveTeam = new MoveTeam();
        public void HandleSceneCommand(uint teamid, plane.MoveToMsg move_to)
        {
            var movings = move_to.movings;
            if (movings.Count > 0)
            {
                moveTeam.teamID = teamid;
                moveTeam.angle = move_to.angle;
                moveTeam.teamPos = new Vector2(move_to.x, move_to.y);
                moveTeam.moves.Clear();
                for (int i = 0; i < movings.Count; ++i)
                    moveTeam.moves.Add(movings[i].plane_id, new Vector2(movings[i].x, movings[i].y));
                HandleSceneCommand(moveTeam);
            }
        }

        private TeamShoot teamShoot = new TeamShoot();
        public void HandleSceneCommand(uint teamid, plane.FireMsg fire)
        {
            teamShoot.teamID = teamid;
            teamShoot.bulletTeamID = fire.bullet_team_id;
            teamShoot.speed = 7f;
            teamShoot.timeOut = 2;
            teamShoot.angle = fire.angle;
            teamShoot.pos = new Vector2(fire.x, fire.y);
            HandleSceneCommand(teamShoot);
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
