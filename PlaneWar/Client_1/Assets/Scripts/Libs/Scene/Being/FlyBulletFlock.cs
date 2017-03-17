using System;

namespace SDK.Lib
{
    /**
     * @brief 飞行子弹聚集
     */
    public class FlyBulletFlock : ITickedObject, IDelayHandleItem, IPriorityObject
    {
        public FlyBulletMgr mFlyBulletMgr;
        protected uint mThisId;
        protected uint mOwnerPlayerThisId;
        private float timeout; //子弹队列存活时间
        protected UnityEngine.Vector3 mPos;
        protected float mMoveSpeed;

        public float TimeOut
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public FlyBulletFlock()
        {
            this.mFlyBulletMgr = new FlyBulletMgr();
        }

        public void init()
        {
            if(null != Ctx.mInstance.mFlyBulletFlockMgr)
            {
                Ctx.mInstance.mFlyBulletFlockMgr.addBulletFlock(this);
            }
        }

        public void dispose()
        {
            if (null != Ctx.mInstance.mFlyBulletFlockMgr)
            {
                Ctx.mInstance.mFlyBulletFlockMgr.removeBulletFlock(this);
            }
        }

        public void setPos(UnityEngine.Vector3 value)
        {
            this.mPos = value;
        }

        public UnityEngine.Vector3 getPos()
        {
            return this.mPos;
        }

        public void setMoveSpeed(float value)
        {
            this.mMoveSpeed = value;
        }

        public float getMoveSpeed()
        {
            return this.mMoveSpeed;
        }

        private Giant.MoveTeamBullet bulletMove = new Giant.MoveTeamBullet();  //子弹移动
        private Giant.BulletTimeOut bulletTimeOut = new Giant.BulletTimeOut(); //子弹消失
        virtual public void onTick(float delta)
        {
            this.mFlyBulletMgr.onTick(delta);
            if (this.TimeOut <= 0)
            {
                //清除资源
                bulletTimeOut.bulletID = this.getThisId();
                (Ctx.mInstance.mGameSys as Game.Game.GameSys).mGameNetHandleCB.HandleSceneCommand(bulletTimeOut);
            }
            else
            {
                //匀速运动，在FlyBullet中处理
                //scene.HandleSceneCommand(bulletMove);
                this.TimeOut -= delta;
            }
        }

        public void setThisId(uint thisId)
        {
            this.mThisId = thisId;
        }

        public uint getThisId()
        {
            return this.mThisId;
        }

        public void setOwnerPlayerThisId(uint thisId)
        {
            this.mOwnerPlayerThisId = thisId;
        }

        public uint getOwnerPlayerThisId()
        {
            return this.mOwnerPlayerThisId;
        }

        public uint getOwnerPlayerLocalId()
        {
            Player ownerPlayer = Ctx.mInstance.mPlayerMgr.getEntityByThisId(this.mThisId) as Player;
            uint localId = 0;

            if (null != ownerPlayer)
            {
                localId = ownerPlayer.mUniqueNumIdGen.genNewId();
            }

            return localId;
        }

        public void setClientDispose(bool isDispose)
        {
            
        }

        public bool isClientDispose()
        {
            return false;
        }
    }
}