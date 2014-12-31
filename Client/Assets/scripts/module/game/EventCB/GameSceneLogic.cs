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
            if (Input.GetKeyDown(KeyCode.M))  // 加载场景资源
            {

            }
            else if (Input.GetKeyDown(KeyCode.K))  // 加载 UI 资源
            {
                Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UIPack);
            }
        }

        private void onKeyUp(KeyCode keyCode)
        {
            
        }

        private void onMouseDown()
        {
            
        }

        private void onMouseUp()
        {
            //定义一条从主相机射向鼠标位置的一条射向
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //判断射线是否发生碰撞               
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider.gameObject.GetComponent<UIEventListener>() != null)        // 如果挂着 UIEventListener ，也说明是场景事件
                {
                    // 直接分发
                    hit.collider.gameObject.SendMessage("OnClick", hit.collider.gameObject);
                }
                //else if (isBtnName(hit.collider.gameObject.name))
                //{
                //    onClkBtn(hit);
                //}
                //判断碰撞物体是否为 Card
                else if (hit.collider.gameObject.name.Length >= 4 && hit.collider.gameObject.name.Substring(0, 4) == SceneEntityName.CARD)
                {
                    //打印出碰撞点的坐标
                    //Debug.Log(hit.point);
                }
                else if (hit.collider.gameObject.name.Length >= 6 && hit.collider.gameObject.name.Substring(0, 6) == SceneEntityName.PLAYER)
                {

                }
            }
        }

        private void onAxisDown()
        {
            Ctx.m_instance.m_playerMgr.getHero().evtMove();
        }

        public void loadUI()
        {
            //LoadParam param = (Ctx.m_instance.m_resMgr as IResMgr).getLoadParam();
            //param.m_path = Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathComUI] + "UIScrollForm.unity3d";
            //param.m_type = ResPackType.eBundleType;
            //param.m_resLoadType = ResLoadType.eLoadDicWeb;
            //param.m_prefabName = "UIScrollForm";
            //param.m_loadedcb = onResLoad;
            //param.m_resNeedCoroutine = false;
            //param.m_loadNeedCoroutine = true;
            //Ctx.m_instance.m_resMgr.load(param);

            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UIPack);
        }

        protected void onShopClk()
        {
            Ctx.m_instance.m_uiMgr.loadForm(UIFormID.UIPack);
        }

        //protected void onClkBtn(RaycastHit hit)
        //{
        //    Ctx.m_instance.m_interActiveEntityMgr.OnMouseUp(hit.collider.gameObject);
        //}

        //// 是否是按钮的名字
        //protected bool isBtnName(string name)
        //{
        //    if ("shopbtn" == name ||
        //        "btn1" == name ||           // 选择一个礼包
        //        "btn2" == name ||           // 选择
        //        "btn7" == name ||
        //        "btn15" == name ||
        //        "btn40" == name ||
        //        "close" == name ||
        //        "openbtn" == name ||
        //        "btn" == name ||
        //        "wdscbtn" == name ||
        //        "dzmoshibtn" == name ||
        //        "goldbuy" == name
        //        )
        //    {
        //        return true;
        //    }

        //    return false;
        //}
    }
}