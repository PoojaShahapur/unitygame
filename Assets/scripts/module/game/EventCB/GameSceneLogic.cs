using SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Game
{
    public class GameSceneLogic : ISceneLogic
    {
        public GameSceneLogic()
        {
            Ctx.m_instance.m_inputMgr.addKeyListener(EventID.KEYDOWN_EVENT, onKeyDown);
            Ctx.m_instance.m_inputMgr.addKeyListener(EventID.KEYUP_EVENT, onKeyUp);
            Ctx.m_instance.m_inputMgr.addMouseListener(EventID.MOUSEDOWN_EVENT, onMouseDown);
            Ctx.m_instance.m_inputMgr.addMouseListener(EventID.MOUSEUP_EVENT, onMouseUp);
            Ctx.m_instance.m_inputMgr.addAxisListener(EventID.AXIS_EVENT, onAxisDown);
        }

        private void onKeyDown(KeyCode keyCode)
        {

        }

        private void onKeyUp(KeyCode keyCode)
        {

        }

        private void onMouseDown()
        {
            
        }

        private void onMouseUp()
        {

        }

        private void onAxisDown()
        {
            Ctx.m_instance.m_playerMgr.getHero().evtMove();
        }
    }
}