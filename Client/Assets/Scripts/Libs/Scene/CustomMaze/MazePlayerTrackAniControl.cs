using Game.UI;
using SDK.Lib;
using System.IO;
using UnityEngine;

namespace SDK.Lib
{
    public class MazePlayerTrackAniControl
    {
        public static float sTime = 1.0f;

        protected MazePlayer m_mazePlayer;

        protected NumAniParallel m_numAniParal;
        protected MList<MazePtBase> m_ptList;
        protected bool m_bDiePt;
        protected bool m_bBombPt;
        protected bool m_BDiedPlayEffect;

        public MazePlayerTrackAniControl(MazePlayer mazePlayer_)
        {
            m_mazePlayer = mazePlayer_;
            m_numAniParal = new NumAniParallel();
            m_numAniParal.setAniSeqEndDisp(onMoveEndHandle);
            m_ptList = new MList<MazePtBase>();
            m_bDiePt = false;
            m_bBombPt = false;
            m_BDiedPlayEffect = false;
        }

        public MList<MazePtBase> ptList
        {
            get
            {
                return m_ptList;
            }
            set
            {
                m_ptList = value;
            }
        }

        public void setStartPos()
        {
            m_mazePlayer.selfGo.transform.localPosition = m_ptList[0].pos;
            // 删除第一个点
            m_ptList.RemoveAt(0);
        }

        public void startMove()
        {
            m_ptList[0].moveToDestPos(m_mazePlayer);
            // 删除第一个点
            m_ptList.RemoveAt(0);
        }

        // 移动到下一个开始点，需要跳跃
        public void moveToDestPos(MazeStartPt pt_)
        {
            string path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "Jump.mp3");
            Ctx.m_instance.m_soundMgr.play(path, false);

            Vector3 srcPos = m_mazePlayer.selfGo.transform.localPosition;
            Vector3 destPos = pt_.pos;
            //float time = 1;

            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 5;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniParal.addOneNumAni(curveAni);
            curveAni.setGO(m_mazePlayer.selfGo);
            curveAni.setTime(sTime);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, srcPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);
            curveAni.setAniEndDisp(onMove2DestEnd);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniParal.play();
        }

        protected void moveToNextPos(MazePtBase pt_)
        {
            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_mazePlayer.selfGo);
            posAni.destPos = pt_.pos;
            posAni.setTime(sTime);
            posAni.setEaseType(iTween.EaseType.linear);

            m_numAniParal.play();
        }

        // 简单直接移动移动动画
        public void moveToDestPos(MazeComPt pt_)
        {
            moveToNextPos(pt_);
        }

        // 移动到结束点
        public void moveToDestPos(MazeEndPt pt_)
        {
            moveToNextPos(pt_);
        }

        // 爆炸点
        public void moveToDestPos(MazeBombPt pt_)
        {
            // 爆炸点
            m_bBombPt = true;

            moveToNextPos(pt_);
        }

        // 死亡点
        public void moveToDestPos(MazeDiePt pt_)
        {
            // 死亡点
            m_bDiePt = true;

            PosAni posAni;
            posAni = new PosAni();
            m_numAniParal.addOneNumAni(posAni);
            posAni.setGO(m_mazePlayer.selfGo);
            posAni.destPos = pt_.pos;
            posAni.setTime(3);
            posAni.setEaseType(iTween.EaseType.easeInOutBounce);

            m_numAniParal.play();

            playDieAniAndSound();
        }

        protected void playDieAniAndSound()
        {
            m_mazePlayer.sceneEffect.stop();
            m_mazePlayer.sceneEffect.setTableID(32);
            m_mazePlayer.sceneEffect.play();

            string path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "BossDie.mp3");
            Ctx.m_instance.m_soundMgr.play(path, false);
        }

        // 移动结束回调
        protected void onMoveEndHandle(NumAniSeqBase dispObj)
        {
            if (m_ptList.Count() > 0 && !m_bDiePt)  // 说明还有 WayPoint 可以走
            {
                MazePtBase pt = m_ptList[0];
                m_ptList.RemoveAt(0);
                pt.moveToDestPos(m_mazePlayer);
            }
            else    // 如果运行到终点位置
            {
                m_ptList.Clear();

                if(m_BDiedPlayEffect)
                {
                    playDieAniAndSound();
                }

                //bool bChangeScene = false;
                if(!m_bBombPt && !m_bDiePt)     // 如果胜利
                {
                    //Ctx.m_instance.m_maze.mazeData.mazeScene.show();
                    //bChangeScene = true;

                    if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eSecond)
                    {
                        Ctx.m_instance.m_maze.mazeData.roomInfo.showLightWin();
                        Ctx.m_instance.m_maze.mazeData.mazePlayer.hide();
                    }
                    else
                    {
                        Ctx.m_instance.m_maze.mazeData.mazeScene.showStar();
                    }
                }
                m_bBombPt = false;
                m_bDiePt = false;
                m_BDiedPlayEffect = false;
                Ctx.m_instance.m_uiMgr.loadAndShow(UIFormID.eUIMaze);

                UIMaze uiMaze = Ctx.m_instance.m_uiMgr.getForm(UIFormID.eUIMaze) as UIMaze;
                if (uiMaze != null)
                {
                    uiMaze.toggleResetBtn(true);
                }

                string path = "";
                path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "Ground.mp3");
                Ctx.m_instance.m_soundMgr.stop(path);
                path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "GameOver.mp3");
                Ctx.m_instance.m_soundMgr.play(path, false);

                //if(bChangeScene)
                //{
                //    if (Ctx.m_instance.m_maze.mazeData.curSceneIdx == (int)eSceneIndex.eFirst)
                //    {
                //        Ctx.m_instance.m_maze.mazeData.mazeScene.loadSecondScene();
                //    }
                //}
            }
        }

        public void onMove2DestEnd(NumAniBase ani)
        {
            onMoveEndHandle(null);
        }

        // 开始
        public void moveToDestPos(MazeStartJumpPt pt_)
        {
            string path = Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathAudio], "Jump.mp3");
            Ctx.m_instance.m_soundMgr.play(path, false);

            Vector3 srcPos = m_mazePlayer.selfGo.transform.localPosition;
            Vector3 destPos = pt_.pos;
            //float time = 1;

            Vector3 midPt;      // 中间点
            midPt = (srcPos + destPos) / 2;
            midPt.y = 5;

            SimpleCurveAni curveAni = new SimpleCurveAni();
            m_numAniParal.addOneNumAni(curveAni);
            curveAni.setGO(m_mazePlayer.selfGo);
            curveAni.setTime(sTime);
            curveAni.setPlotCount(3);
            curveAni.addPlotPt(0, srcPos);
            curveAni.addPlotPt(1, midPt);
            curveAni.addPlotPt(2, destPos);
            curveAni.setAniEndDisp(onMove2DestEnd);

            curveAni.setEaseType(iTween.EaseType.easeInExpo);

            m_numAniParal.play();
        }

        public void moveToDestPos(MazeStartShowPt pt_)
        {
            m_mazePlayer.selfGo.transform.localPosition = pt_.pos;
            onMoveEndHandle(null);
        }

        public void moveToDestPos(MazeStartDoorPt pt_)
        {
            m_mazePlayer.selfGo.transform.localPosition = pt_.pos;
            onMoveEndHandle(null);
        }

        public void moveToDestPos(MazeEndJumpPt pt_)
        {
            moveToNextPos(pt_);
        }

        public void moveToDestPos(MazeEndHidePt pt_)
        {
            moveToNextPos(pt_);
        }

        public void moveToDestPos(MazeEndDoorPt pt_)
        {
            moveToNextPos(pt_);
        }

        public void moveToDestPos(MazeEndDiePt pt_)
        {
            m_bDiePt = true;
            m_BDiedPlayEffect = true;
            moveToNextPos(pt_);
        }
    }
}