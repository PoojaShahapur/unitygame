using Game.Msg;
using SDK.Common;
using SDK.Lib;
using System;
using UnityEngine;

namespace FightCore
{
    /**
     * @brief 场景中可拖放的卡牌，仅限场景中可拖放的卡牌，只有手里卡牌区域中的卡牌可以拖放，其它的都是不能拖放的，目前装备、技能、手牌、出牌都是用这个类， hero 使用另外一个
     */
    public class IOControlBase : CardControlBase
    {
        public const uint WHITECARDID = 10000;

        public IOControlBase(SceneCardBase rhv) : 
            base(rhv)
        {
            
        }

        public override void init()
        {
            base.init();
        }

        virtual public void setMoveDisp(Action rhv)
        {
            
        }

        virtual public void setCenterPos(Vector3 rhv)
        {

        }

        virtual public void setOutSplitZ(float outSplitZ_)
        {

        }

        // 能拖动的必然所有的操作都完成后才能操作
        virtual protected void onStartDrag()
        {
            
        }

        // 开启拖动
        virtual public void enableDrag()
        {
            
        }

        // 关闭拖放功能
        virtual public void disableDrag()
        {
            
        }

        virtual public void enableDragTitle()
        {
            
        }

        virtual public void disableDragTitle()
        {
            
        }

        // 指明当前是否可以改变位置
        virtual protected bool canMove()
        {
            return false;
        }

        virtual protected void onMove()
        {
            
        }

        // 拖放结束处理
        virtual protected void onDragEnd()
        {
            
        }

        // 回退卡牌到原始位置
        virtual public void backCard2Orig()
        {
            
        }

        // 所有的卡牌都可以点击，包括主角、装备、技能、手里卡牌、出的卡牌
        virtual public void onCardClick(IDispatchObject dispObj)
        {
            
        }

        // 输入按下
        virtual public void onCardDown(IDispatchObject dispObj)
        {
            
        }

        // 输入释放
        virtual public void onCardUp(IDispatchObject dispObj)
        {

        }

        // 输入按下移动到卡牌上
        virtual public void onDragOver(IDispatchObject dispObj)
        {
            
        }

        // 输入按下移动到卡牌出
        virtual public void onDragOut(IDispatchObject dispObj)
        {
            
        }

        // 开始转换模型
        virtual public void startConvModel(int type)
        {

        }

        // 结束转换模型
        virtual public void endConvModel(int type)
        {

        }
    }
}