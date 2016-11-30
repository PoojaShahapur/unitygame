using UnityEngine;

namespace SDK.Lib
{
    public class AuxSnowBlockUserData : AuxSceneEntityUserData
    {
        //public AudioClip eatMusic;
        //public GameObject fatherObj;

        enum EatState
        {
            Eaten_ByOther = 1,//被对方吃掉了
            Nothing_Happen = 2,//相安无事
        };

        void OnTriggerEnter(Collider other)
        {
            AuxSceneEntityUserData aUserData = this.GetComponent<AuxSceneEntityUserData>();
            BeingEntity aBeingEntity = aUserData.getUserData();

            AuxSceneEntityUserData bUserData = other.gameObject.GetComponent<AuxSceneEntityUserData>();
            BeingEntity bBeingEntity = bUserData.getUserData();

            //if (other.gameObject.CompareTag("SnowBall"))
            if (bBeingEntity.getEntityType() == EntityType.eSnowBlock)
            {
                EatState state = EatState.Nothing_Happen;

                //Food colSnow = other.gameObject.GetComponent<Food>() as Food;
                SnowBlock colSnow = bBeingEntity as SnowBlock;
                //if (colSnow == null)//碰到的是机器人，机器人的碰撞是子物体检测，Food是挂载在父物体上的
                //{
                //    colSnow = other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Food>() as Food;
                //}
                if (colSnow == null) return;

                // 检查是否会被吃掉（雪块可以直接吃掉，不比较半径）
                float M2O = 0.0f;// fatherObj.GetComponent<Transform>().localScale.x / colSnow.gameObject.GetComponent<Transform>().localScale.x;//自己与对方半径比
                if (M2O <= colSnow.m_canEatRate)
                {
                    state = EatState.Eaten_ByOther;
                }
                if (EatState.Nothing_Happen == state) return;

                //AudioSource.PlayClipAtPoint(eatMusic, Camera.main.transform.position);

                //计算缩放比率
                //float newBallRadius = UtilLogic.getRadiusByMass(UtilLogic.getMassByRadius(colSnow.gameObject.GetComponent<Transform>().localScale.x) + CreateSnowBlock.Instance.blockMass);
                float newBallRadius = UtilLogic.getRadiusByMass(UtilLogic.getMassByRadius(colSnow.transform().localScale.x) + CreateSnowBlock.Instance.blockMass);
                bool otherisRobot = colSnow.amIRobot();//对方是否机器人

                if (EatState.Eaten_ByOther == state)//被吃掉
                {
                    //修改对方的数据                
                    if (otherisRobot)//对方是否机器人
                    {
                        //other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Transform>().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        colSnow.transform().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        //GameObjectManager.getInstance().setEntityByRadius(newBallRadius, other.gameObject.GetComponent<TriggerEnterEvent>().fatherObj.GetComponent<Food>().entity);
                        //other.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                        colSnow.transform().localScale = new Vector3(1f, 1f, 1f);
                        //log.logHelper.DebugLog("吃球,state=" + state + ",对方是否为机器人" + otherisRobot.ToString() + ",摧毁" + other.gameObject.name);
                    }
                    else
                    {
                        ++colSnow.m_swallownum;
                        //GameObjectManager.getInstance().setEntityByRadius(newBallRadius, other.gameObject.GetComponent<Food>().entity);
                        colSnow.transform().localScale = new Vector3(newBallRadius, newBallRadius, newBallRadius);
                        try
                        {
                            colSnow.transform().FindChild("Player1").localScale = new Vector3(1f, 1f, 1f);
                        }
                        catch (System.Exception e)
                        {
                            log.logHelper.DebugLog(other.gameObject.name + " " + e.ToString());
                        }
                    }
                    //Destroy(fatherObj);
                    aBeingEntity.dispose();

                    //数秒钟刷新一次
                    //CreateSnowBlock.Instance.RefreshSnowBlock();
                    Ctx.mInstance.mSnowBlockMgr.RefreshSnowBlock();
                }
            }
        }
    }
}