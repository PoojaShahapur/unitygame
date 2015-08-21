using SDK.Common;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class MazeData
    {
        protected RoomInfo m_roomInfo;
        protected MazeOp m_mazeOp;
        protected MazePlayer m_mazePlayer;
        protected GameObject m_sceneRootGo;
        protected MazeScene m_mazeScene;

        public MazeData()
        {
            m_roomInfo = new RoomInfo();
            m_mazeOp = new MazeOp();
            m_mazePlayer = new MazePlayer();
            m_mazeScene = new MazeScene();
        }

        public RoomInfo roomInfo
        {
            get
            {
                return m_roomInfo;
            }
            set
            {
                m_roomInfo = value;
            }
        }

        public MazeOp mazeOp
        {
            get
            {
                return m_mazeOp;
            }
            set
            {
                m_mazeOp = value;
            }
        }

        public GameObject sceneRootGo
        {
            get
            {
                return m_sceneRootGo;
            }
            set
            {
                m_sceneRootGo = value;
            }
        }

        public MazePlayer mazePlayer
        {
            get
            {
                return m_mazePlayer;
            }
            set
            {
                m_mazePlayer = value;
            }
        }

        public MazeScene mazeScene
        {
            get
            {
                return m_mazeScene;
            }
            set
            {
                m_mazeScene = value;
            }
        }

        public void init()
        {
            Ctx.m_instance.m_tableSys.getTable(TableID.TABLE_SPRITEANI);

            string path = "RootGo";
            m_sceneRootGo = UtilApi.GoFindChildByPObjAndName(path);

            m_roomInfo.initMazeRoomCount(4);
            m_mazePlayer.init();
            path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "Ground.mp3");
            Ctx.m_instance.m_soundMgr.play(path);

            m_mazeScene.init();
            m_mazeScene.hide();
        }

        public void getWayPtList()
        {
            m_mazePlayer.clearPath();
            m_roomInfo.getWayPtList(m_mazePlayer.mazePlayerTrackAniControl.ptList);
        }

        public void setStartPos()
        {
            m_mazePlayer.setStartPos();
        }

        public void startMove()
        {
            m_mazePlayer.startMove();
        }
    }
}