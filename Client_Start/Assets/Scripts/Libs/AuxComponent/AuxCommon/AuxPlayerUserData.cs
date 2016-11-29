﻿using UnityEngine;

namespace SDK.Lib
{
    public class AuxPlayerUserData : AuxSceneEntityUserData
    {
        public AudioClip eatMusic;
        public GameObject fatherObj;


        enum EatState
        {
            Eat_Other = 0,//吃掉对方
            Eaten_ByOther = 1,//被对方吃掉了
            Nothing_Happen = 2,//相安无事
        };

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("SnowBall"))
            {
                EatState state = EatState.Nothing_Happen;

                Food colSnow = other.gameObject.GetComponent<Food>() as Food;
                if (colSnow == null)//碰到的是机器人，机器人的碰撞是子物体检测，Food是挂载在父物体上的
                {
                    colSnow = other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Food>() as Food;
                }
                if (colSnow == null) return;

                // 检查谁吃谁
                float O2M = colSnow.gameObject.GetComponent<Transform>().localScale.x / fatherObj.GetComponent<Transform>().localScale.x;//对方与自己半径比
                float M2O = fatherObj.GetComponent<Transform>().localScale.x / colSnow.gameObject.GetComponent<Transform>().localScale.x;//自己与对方半径比
                log.logHelper.DebugLog("O2M=" + O2M.ToString() + ", M2O=" + M2O.ToString());
                if (O2M <= fatherObj.GetComponent<Food>().entity.m_canEatRate)
                {
                    state = EatState.Eat_Other;
                }
                else if (M2O <= colSnow.entity.m_canEatRate)
                {
                    state = EatState.Eaten_ByOther;
                }
                //log.logHelper.DebugLog("colSnow " + colSnow.entity.m_name + ", rate=" + colSnow.entity.m_canEatRate + 
                //    "fatherObj " + fatherObj.GetComponent<Food>().entity.m_name + ", rate=" + fatherObj.GetComponent<Food>().entity.m_canEatRate);
                if (EatState.Nothing_Happen == state) return;

                AudioSource.PlayClipAtPoint(eatMusic, Camera.main.transform.position);

                //计算缩放比率            
                float newBallRadius = UtilLogic.getRadiusByMass(UtilLogic.getMassByRadius(colSnow.gameObject.GetComponent<Transform>().localScale.x) + UtilLogic.getMassByRadius(fatherObj.GetComponent<Transform>().localScale.x));
                bool otherisRobot = colSnow.amIRobot();//对方是否机器人
                                                       //log.logHelper.DebugLog("吃球,state=" + state + colSnow.gameObject.name +  "是否为机器人" + otherisRobot.ToString());

                if (state == EatState.Eat_Other)//吃掉对方
                {
                    if (otherisRobot)
                    {
                        //吃掉机器人，修改自己的数据
                        fatherObj.GetComponent<Transform>().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        fatherObj.transform.FindChild("Sphere").localScale = new Vector3(1f, 1f, 1f);
                        //GameObjectManager.getInstance().setEntityByRadius(newBallRadius, fatherObj.GetComponent<Food>().entity);
                        //log.logHelper.DebugLog("吃球,state=" + state + ",对方是否为机器人" + otherisRobot.ToString() + ",摧毁" + other.gameObject.name);
                        GameObject.Destroy(other.gameObject);
                    }
                    else
                    {
                        log.logHelper.DebugLog("吃玩家,state=" + state + ",对方是否为机器人" + otherisRobot.ToString() + ",摧毁" + other.gameObject.name);
                        //无敌状态下
                        if (CreatePlayer._Instace.GetIsNiuBi()) return;

                        //吃掉玩家,弹出游戏结束对话框，询问玩家是否继续游戏
                        GameObject.Destroy(other.gameObject);
                        GlobalUI._Instance.ResetReliveSeconds();
                        GlobalUI._Instance.ShowReliveArea(true);
                        GlobalUI._Instance.SetEnemyName(fatherObj.name);
                    }

                    ++fatherObj.GetComponent<Food>().entity.m_swallownum;
                }
                else if (EatState.Eaten_ByOther == state)//被吃掉
                {
                    //修改对方的数据                
                    if (otherisRobot)//对方是否机器人
                    {
                        other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Transform>().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        //GameObjectManager.getInstance().setEntityByRadius(newBallRadius, other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Food>().entity);
                        other.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                        //log.logHelper.DebugLog("吃球,state=" + state + ",对方是否为机器人" + otherisRobot.ToString() + ",摧毁" + other.gameObject.name);
                    }
                    else
                    {
                        ++other.gameObject.GetComponent<Food>().entity.m_swallownum;
                        //GameObjectManager.getInstance().setEntityByRadius(newBallRadius, other.gameObject.GetComponent<Food>().entity);
                        other.GetComponent<Transform>().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        try
                        {
                            other.gameObject.transform.FindChild("Player1").localScale = new Vector3(1f, 1f, 1f);
                        }
                        catch (System.Exception e)
                        {
                            log.logHelper.DebugLog(other.gameObject.name + " " + e.ToString());
                        }
                    }
                    GameObject.Destroy(fatherObj);
                }
            }
        }
    }
}