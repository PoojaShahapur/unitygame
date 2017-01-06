using SDK.Lib;
using System.Collections.Generic;

namespace Game.Game
{
    /**
     * @brief 消息处理流程
     */
    public class ReqSceneInteractive
    {
        // 发送主角移动消息，现在发送所有的分裂的移动
        public static void sendPlayerMove(KBEngine.Entity playerEntity, System.UInt32 spaceID, KBEngine.NetworkInterface _networkInterface)
        {
            // 主角的位置
            UnityEngine.Vector3 position = playerEntity.position;
            UnityEngine.Vector3 direction = playerEntity.direction;

            playerEntity._entityLastLocalPos = position;
            playerEntity._entityLastLocalDir = direction;

            //if (false)
            //{
            //    playerEntity._entityLastLocalPos = position;
            //    playerEntity._entityLastLocalDir = direction;

            //    Bundle bundle = Bundle.createObject();
            //    bundle.newMessage(Message.messages["Baseapp_onUpdateDataFromClient"]);
            //    bundle.writeFloat(position.x);
            //    bundle.writeFloat(position.y);
            //    bundle.writeFloat(position.z);

            //    double x = ((double)direction.x / 360 * (System.Math.PI * 2));
            //    double y = ((double)direction.y / 360 * (System.Math.PI * 2));
            //    double z = ((double)direction.z / 360 * (System.Math.PI * 2));

            //    // 根据弧度转角度公式会出现负数
            //    // unity会自动转化到0~360度之间，这里需要做一个还原
            //    if (x - System.Math.PI > 0.0)
            //        x -= System.Math.PI * 2;

            //    if (y - System.Math.PI > 0.0)
            //        y -= System.Math.PI * 2;

            //    if (z - System.Math.PI > 0.0)
            //        z -= System.Math.PI * 2;

            //    bundle.writeFloat((float)x);
            //    bundle.writeFloat((float)y);
            //    bundle.writeFloat((float)z);
            //    bundle.writeUint8((System.Byte)(playerEntity.isOnGround == true ? 1 : 0));
            //    bundle.writeUint32(spaceID);
            //    bundle.send(_networkInterface);
            //}
            //else
            {
                // 发送主角分裂的 Child 的位置
                KBEngine.Bundle bundle = KBEngine.Bundle.createObject();
                bundle.newMessage(KBEngine.Message.messages["Baseapp_onUpdateDataFromClient"]);

                /**
                    消息格式
                    总共球的数量N个 : uint32
                    (
                    球的eid: ENTITY_ID
                    球的坐标和朝向: 6 个 float
                    是否在地面上 ： uint8
                    当前地图Id: SPACE_ID
                    )一共N条数据
                */
                PlayerChildMgr playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;

                int idx = 0;
                int len = playerChildMgr.getEntityCount();
                PlayerChild playerChild = null;
                KBEngine.Entity kbeEntity = null;
                System.Int32 eid = 0;

                bundle.writeUint32((uint)len);

                while (idx < len)
                {
                    playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;
                    kbeEntity = playerChild.getEntity();
                    eid = kbeEntity.id;

                    position = playerChild.getPos();
                    direction = playerChild.getRotate().eulerAngles;

                    bundle.writeInt32(eid);

                    bundle.writeFloat(position.x);
                    bundle.writeFloat(position.y);
                    bundle.writeFloat(position.z);

                    double x = ((double)direction.x / 360 * (System.Math.PI * 2));
                    double y = ((double)direction.y / 360 * (System.Math.PI * 2));
                    double z = ((double)direction.z / 360 * (System.Math.PI * 2));

                    // 根据弧度转角度公式会出现负数
                    // unity会自动转化到0~360度之间，这里需要做一个还原
                    if (x - System.Math.PI > 0.0)
                        x -= System.Math.PI * 2;

                    if (y - System.Math.PI > 0.0)
                        y -= System.Math.PI * 2;

                    if (z - System.Math.PI > 0.0)
                        z -= System.Math.PI * 2;

                    bundle.writeFloat((float)x);
                    bundle.writeFloat((float)y);
                    bundle.writeFloat((float)z);
                    bundle.writeUint8((System.Byte)(kbeEntity.isOnGround == true ? 1 : 0));
                    bundle.writeUint32(spaceID);

                    ++idx;
                }

                bundle.send(_networkInterface);
            }
        }

        // 发送主角分裂
        public static void sendMainSplit()
        {
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();
            PlayerChildMgr playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;

            int idx = 0;
            int len = playerChildMgr.getEntityCount();
            PlayerChild playerChild = null;
            KBEngine.Entity kbeEntity = null;
            System.Int32 eid = 0;
            UnityEngine.Vector3 pos;

            Dictionary<string, object> infos = new Dictionary<string, object>();
            List<object> listinfos = new List<object>();
            infos["values"] = listinfos;

            Dictionary<string, object> info = null;

            for (idx = 0; idx < len; idx++)
            {
                playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;
                kbeEntity = playerChild.getEntity();
                eid = kbeEntity.id;
                pos = playerChild.getPos();

                info = new Dictionary<string, object>();
                listinfos.Add(info);

                info["eid"] = eid;
                info["topos"] = pos;
            }

            player.cellCall("reqSplit", MsgLogicCV.eSplit, infos);
        }

        // 进行分裂
        public static void sendSplit()
        {
            PlayerChildMgr playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;

            // 这个分裂修改列表数据，因此只查找前面的数据
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();
            int idx = 0;
            int num = playerChildMgr.getEntityCount();
            PlayerChild playerChild;
            bool ischanged = false;

            UnityEngine.Vector3 pos;
            UnityEngine.Vector3 initPos;
            UnityEngine.Vector3 toPos;
            float splitRadius = 0;

            Dictionary<string, object> infos = new Dictionary<string, object>();
            List<object> listinfos = new List<object>();
            infos["values"] = listinfos;

            Dictionary<string, object> info = null;

            KBEngine.Entity kbeEntity = null;
            System.Int32 eid = 0;

            while (idx < num && Ctx.mInstance.mSnowBallCfg.isLessMaxNum(playerChildMgr.getEntityCount()))
            {
                playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                if (playerChild.canSplit())
                {
                    ischanged = true;

                    kbeEntity = playerChild.getEntity();
                    eid = kbeEntity.id;
                    pos = playerChild.getPos();

                    // 设置位置
                    splitRadius = (playerChild as BeingEntity).getSplitRadius();
                    // 设置位置
                    pos = playerChild.getPos();
                    initPos = pos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, playerChild.getBallRadius() + splitRadius + Ctx.mInstance.mSnowBallCfg.mSplitRelStartPos);

                    toPos = initPos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mSplitRelDist + 30);

                    info = new Dictionary<string, object>();
                    listinfos.Add(info);

                    info["eid"] = eid;
                    info["frompos"] = initPos;
                    info["topos"] = toPos;
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("Can not Split", LogTypeId.eLogSplitMergeEmit);
                }

                ++idx;
            }

            if (ischanged)
            {
                if (Ctx.mInstance.mSnowBallCfg.isGreatEqualMaxNum(playerChildMgr.getEntityCount()))
                {
                    Ctx.mInstance.mLogSys.log("Split GreatEqual Max", LogTypeId.eLogSplitMergeEmit);
                }
            }

            player.cellCall("reqSplit", (int)MsgLogicCV.eSplit, infos);

            Ctx.mInstance.mLogSys.log("Send Split", LogTypeId.eLogSceneInterActive);
        }

        // 融合
        public static void sendMerge(BeingEntity aBeing, BeingEntity bBeing)
        {
            // 这个分裂修改列表数据，因此只查找前面的数据
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();

            Dictionary<string, object> infos = new Dictionary<string, object>();
            List<object> listinfos = new List<object>();
            infos["values"] = listinfos;

            Dictionary<string, object> info = null;

            KBEngine.Entity kbeEntity = null;
            System.Int32 eid = 0;

            kbeEntity = aBeing.getEntity();
            eid = kbeEntity.id;

            info = new Dictionary<string, object>();
            listinfos.Add(info);

            info["eid"] = eid;
            info["frompos"] = aBeing.getPos();
            info["topos"] = aBeing.getPos();

            kbeEntity = bBeing.getEntity();
            eid = kbeEntity.id;

            info = new Dictionary<string, object>();
            listinfos.Add(info);

            info["eid"] = eid;
            info["frompos"] = aBeing.getPos();
            info["topos"] = aBeing.getPos();

            player.cellCall("reqSplit", (int)MsgLogicCV.eMerge, infos);

            Ctx.mInstance.mLogSys.log("Send Merge", LogTypeId.eLogSceneInterActive);
        }

        // 吐雪块
        public static void sendShit()
        {
            PlayerChildMgr playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;

            // 这个分裂修改列表数据，因此只查找前面的数据
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();
            int idx = 0;
            int num = playerChildMgr.getEntityCount();
            PlayerChild playerChild;

            UnityEngine.Vector3 pos;
            UnityEngine.Vector3 initPos;
            UnityEngine.Vector3 toPos;

            Dictionary<string, object> infos = new Dictionary<string, object>();
            List<object> listinfos = new List<object>();
            infos["values"] = listinfos;

            Dictionary<string, object> info = null;

            KBEngine.Entity kbeEntity = null;
            System.Int32 eid = 0;
            float emitRadius = 1;
            bool isEmited = false;

            while (idx < num)
            {
                playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                if (playerChild.canEmitSnow())
                {
                    isEmited = true;

                    kbeEntity = playerChild.getEntity();
                    eid = kbeEntity.id;
                    pos = playerChild.getPos();

                    initPos = playerChild.getPos() + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, playerChild.getBallRadius() + emitRadius + Ctx.mInstance.mSnowBallCfg.mEmitRelStartPos);
                    toPos = initPos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mEmitRelDist);

                    info = new Dictionary<string, object>();
                    listinfos.Add(info);

                    info["eid"] = eid;
                    info["frompos"] = initPos;
                    info["topos"] = toPos;
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("Can not Emit", LogTypeId.eLogSplitMergeEmit);
                }

                ++idx;
            }

            if (isEmited)
            {
                player.cellCall("reqSplit", (int)MsgLogicCV.eShit, infos);

                Ctx.mInstance.mLogSys.log("Send Shit", LogTypeId.eLogSceneInterActive);
            }
        }
    }
}