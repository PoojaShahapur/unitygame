﻿using SDK.Lib;
using System.Collections.Generic;

namespace Game.Game
{
    /**
     * @brief 消息处理流程
     */
    public class ReqSceneInteractive
    {
        // 检查 Child 并且发送主角移动
        public static void checkChildAndSendPlayerMove()
        {
            if(isPosOrRotateChanged())
            {
                KBEngine.Entity playerEntity = Ctx.mInstance.mClientApp.gameapp.player();
                System.UInt32 spaceID = Ctx.mInstance.mClientApp.gameapp.spaceID;
                KBEngine.NetworkInterface _networkInterface = Ctx.mInstance.mClientApp.gameapp.networkInterface();

                sendPlayerMove(playerEntity, spaceID, _networkInterface);
            }
        }

        public static bool isPosOrRotateChanged()
        {
            bool ret = false;

            PlayerChildMgr playerChildMgr = null;
            if (Ctx.mInstance.mPlayerMgr.getHero() != null && Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge != null)
                playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;
            if (playerChildMgr == null)
                return ret;

            int idx = 0;
            int len = playerChildMgr.getEntityCount();
            PlayerChild playerChild = null;

            if (len > 0)
            {
                while (idx < len)
                {
                    playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                    if(BeingState.eBSSeparation == playerChild.getBeingState())
                    {
                        ret = true;
                        break;
                    }

                    ++idx;
                }
            }

            return ret;
        }

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
                int len = 0;
                MList<int> idxList = new MList<int>();

                while (idx < playerChildMgr.getEntityCount())
                {
                    PlayerChild playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;
                    
                    position = playerChild.getPos();
                    direction = playerChild.getRotate().eulerAngles;

                    UnityEngine.Vector3 oldPosition = playerChild.getPreSendPosition();
                    if (UnityEngine.Vector3.Distance(position, oldPosition) > 1.0f)
                    {
                        ++len;
                        idxList.Add(idx);
                    }
                    ++idx;
                }

                idx = 0;
                if (len > 0)
                {
                    // 发送主角分裂的 Child 的位置
                    KBEngine.Bundle bundle = KBEngine.Bundle.createObject();
                    bundle.newMessage(KBEngine.Message.messages["Baseapp_onUpdateDataFromClient"]);

                    PlayerChild playerChild = null;
                    KBEngine.Entity kbeEntity = null;
                    System.Int32 eid = 0;

                    bundle.writeUint32((uint)len);

                    while (idx < len)
                    {
                        playerChild = playerChildMgr.getEntityByIndex(idxList[idx]) as PlayerChild;
                        kbeEntity = playerChild.getEntity();
                        eid = kbeEntity.id;

                        position = playerChild.getPos();
                        direction = playerChild.getRotate().eulerAngles;
                        
                        playerChild.setPreSendPosition(position);

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

                        Ctx.mInstance.mLogSys.log(
                            string.Format("ReqSceneInteractive::sendPlayerMove, Send Move eid = {0}, PosX = {1}, PosY = {2}, PosZ = {3}", eid, position.x, position.y, position.z),
                            LogTypeId.eLogBeingMove);

                        ++idx;
                    }

                    bundle.send(_networkInterface);
                }
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
            if (!Ctx.mInstance.mCommonData.isSplitSuccess())
            {
                Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendSplit, Not send split, server not return", LogTypeId.eLogScene);
                return;
            }

            PlayerChildMgr playerChildMgr = Ctx.mInstance.mPlayerMgr.getHero().mPlayerSplitMerge.mPlayerChildMgr;

            // 这个分裂修改列表数据，因此只查找前面的数据
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();
            int idx = 0;
            int num = playerChildMgr.getEntityCount();

            //已达到最大数量
            if(Ctx.mInstance.mSnowBallCfg.isGreatEqualMaxNum(num))
            {
                Ctx.mInstance.mLogSys.log(string.Format("ReqSceneInteractive::sendSplit, Not send split, arrive max num ball, current ball num = {0}, Max ball num = {1}", num, Ctx.mInstance.mSnowBallCfg.mMaxSnowNum), LogTypeId.eLogScene);
                return;
            }

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
            ulong uniqueId = 0;

            int split_num = 0;
            while (idx < num)
            {
                playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                if (playerChild.canSplit() && Ctx.mInstance.mSnowBallCfg.isLessMaxNum(num + split_num))
                {
                    ++split_num;
                    playerChild.setLastMergedTime();    // 分裂立刻设置融合时间，防止立刻融合

                    ischanged = true;

                    kbeEntity = playerChild.getEntity();
                    eid = kbeEntity.id;
                    pos = playerChild.getPos();

                    // 设置位置
                    splitRadius = (playerChild as BeingEntity).getSplitWorldRadius();
                    // 设置位置
                    pos = playerChild.getPos();
                    initPos = pos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, playerChild.getBallWorldRadius() + splitRadius + Ctx.mInstance.mSnowBallCfg.mSplitRelStartPos);
                    initPos = Ctx.mInstance.mSceneSys.adjustPosInRange(initPos);

                    toPos = initPos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mSplitRelDist);
                    toPos = Ctx.mInstance.mSceneSys.adjustPosInRange(toPos);

                    info = new Dictionary<string, object>();
                    listinfos.Add(info);

                    info["eid"] = eid;
                    info["frompos"] = initPos;
                    info["topos"] = toPos;
                    uniqueId = (ulong)eid;
                    info["uniqueid"] = uniqueId;

                    Ctx.mInstance.mLogSys.log(string.Format("ReqSceneInteractive::sendSplit, Send Split eid = {0}, initPos.x = {1}, initPos.y = {2}, initPos.z = {3}, toPos.x = {4}, toPos.y = {5}, toPos.z = {6}, uniqueid = {7}", eid, initPos.x, initPos.y, initPos.z, toPos.x, toPos.y, toPos.z, uniqueId), LogTypeId.eLogSplitMergeEmit);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendSplit, Can not Split", LogTypeId.eLogSplitMergeEmit);
                }

                ++idx;
            }

            if (ischanged)
            {
                Ctx.mInstance.mCommonData.setSplitSuccess(false);
                player.cellCall("reqSplit", (int)MsgLogicCV.eSplit, infos);

                Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendSplit, Send Split", LogTypeId.eLogSceneInterActive);
            }
        }

        // 融合
        //public static void sendMerge(BeingEntity aBeing, BeingEntity bBeing)
        //{
        //    // 这个分裂修改列表数据，因此只查找前面的数据
        //    PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();

        //    Dictionary<string, object> infos = new Dictionary<string, object>();
        //    List<object> listinfos = new List<object>();
        //    infos["values"] = listinfos;

        //    Dictionary<string, object> info = null;

        //    KBEngine.Entity kbeEntity = null;
        //    System.Int32 eid = 0;

        //    kbeEntity = aBeing.getEntity();
        //    eid = kbeEntity.id;

        //    info = new Dictionary<string, object>();
        //    listinfos.Add(info);

        //    info["eid"] = eid;
        //    info["frompos"] = aBeing.getPos();
        //    info["topos"] = aBeing.getPos();

        //    kbeEntity = bBeing.getEntity();
        //    eid = kbeEntity.id;

        //    info = new Dictionary<string, object>();
        //    listinfos.Add(info);

        //    info["eid"] = eid;
        //    info["frompos"] = aBeing.getPos();
        //    info["topos"] = aBeing.getPos();

        //    player.cellCall("reqSplit", (int)MsgLogicCV.eMerge, infos);

        //    Ctx.mInstance.mLogSys.log(string.Format("Send Merge eid = {0}, frompos.x = {1}, frompos.y = {2}, frompos.z = {3}", eid, aBeing.getPos().x, aBeing.getPos().y, aBeing.getPos().z), LogTypeId.eLogSceneInterActive);
        //}

        public static void sendMerge(BeingEntity aBeing, BeingEntity bBeing)
        {
            // 这个分裂修改列表数据，因此只查找前面的数据
            PlayerMain player = Ctx.mInstance.mPlayerMgr.getHero();

            List<object> listinfos = new List<object>();

            KBEngine.Entity kbeEntity = null;
            System.Int32 aeid = 0;
            System.Int32 beid = 0;

            kbeEntity = aBeing.getEntity();
            aeid = kbeEntity.id;
            listinfos.Add(aeid);

            kbeEntity = bBeing.getEntity();
            beid = kbeEntity.id;
            listinfos.Add(beid);

            player.cellCall("reqMerge", listinfos);

            //(KBEngine.KBEngineApp.app.findEntity((System.Int32)player.getThisId()) as KBEngine.Avatar).notifyMerge((System.Int32)aBeing.getThisId(), (System.Int32)bBeing.getThisId(), (System.Int32)aBeing.getThisId(), aBeing.getMass() + bBeing.getMass());

            Ctx.mInstance.mLogSys.log(string.Format("ReqSceneInteractive::sendMerge, Send Merge aThisid = {0}, frompos.x = {1}, frompos.y = {2}, frompos.z = {3}, bThisId = {4}, bPos.x = {5}, , bPos.y = {6}, bPos.z = {7}, dist = {8}", aeid, aBeing.getPos().x, aBeing.getPos().y, aBeing.getPos().z, beid, bBeing.getPos().x, bBeing.getPos().y, bBeing.getPos().z, (bBeing.getPos() - aBeing.getPos()).magnitude), LogTypeId.eLogSceneInterActive);
        }

        // 吐雪块
        public static void sendShit()
        {
            if (!Ctx.mInstance.mCommonData.isEmitSuccess())
            {
                Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendShit, Emit SnowBall Wait Server Return", LogTypeId.eLogScene);
                return;
            }

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
            ulong uniqueId = 0;

            while (idx < num)
            {
                playerChild = playerChildMgr.getEntityByIndex(idx) as PlayerChild;

                if (playerChild.canEmitSnow())
                {
                    isEmited = true;

                    emitRadius = playerChild.getEmitSnowWorldSize();

                    kbeEntity = playerChild.getEntity();
                    eid = kbeEntity.id;
                    pos = playerChild.getPos();

                    initPos = playerChild.getPos() + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, playerChild.getBallWorldRadius() + emitRadius + Ctx.mInstance.mSnowBallCfg.mEmitRelStartPos);
                    initPos = Ctx.mInstance.mSceneSys.adjustPosInRange(initPos);

                    toPos = initPos + playerChild.getRotate() * new UnityEngine.Vector3(0, 0, Ctx.mInstance.mSnowBallCfg.mEmitRelDist);
                    toPos = Ctx.mInstance.mSceneSys.adjustPosInRange(toPos);

                    info = new Dictionary<string, object>();
                    listinfos.Add(info);

                    initPos = Ctx.mInstance.mSceneSys.adjustPosInRange(initPos);

                    info["eid"] = eid;
                    info["frompos"] = initPos;
                    info["topos"] = toPos;
                    uniqueId = UtilMath.makeUniqueId((uint)eid, Ctx.mInstance.mPlayerSnowBlockMgr.getCurId());
                    info["uniqueid"] = uniqueId;

                    Ctx.mInstance.mLogSys.log(string.Format("ReqSceneInteractive::sendShit, Shit One eid = {0}, initPos.x = {1}, initPos.x = {2}, initPos.x = {3}, toPos.x = {4}, toPos.y = {5}, toPos.z = {6}, uniqueId = {7}", eid, initPos.x, initPos.y, initPos.z, toPos.x, toPos.y, toPos.z, uniqueId), LogTypeId.eLogSplitMergeEmit);

                    Ctx.mInstance.mPlayerSnowBlockMgr.emitOne(initPos, toPos, UnityEngine.Quaternion.identity, 10);
                }
                else
                {
                    Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendShit, Can not Emit", LogTypeId.eLogSplitMergeEmit);
                }

                ++idx;
            }

            if (isEmited)
            {
                Ctx.mInstance.mCommonData.setEmitSuccess(false);
                player.cellCall("reqSplit", (int)MsgLogicCV.eShit, infos);

                Ctx.mInstance.mLogSys.log("ReqSceneInteractive::sendShit, Send Shit", LogTypeId.eLogSplitMergeEmit);
            }
        }
    }
}