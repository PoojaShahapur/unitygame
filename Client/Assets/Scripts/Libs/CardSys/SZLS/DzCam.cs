﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SDK.Common;
using Game.UI;
using UnityEngine.UI;

namespace SDK.Lib
{
    public class DzCam : InterActiveEntity
    {
        // Use this for initialization
        public override void Start()
        {
            dzban = GameObject.Find("dzban").transform;
            mHand = dzban;
            mycosttext = GameObject.Find("mycosttext").GetComponent<Text>();
            enemycosttext = GameObject.Find("enemycosttext").GetComponent<Text>();
            //mycosttext.text = "0/0";
            //enemycosttext.text = "0/0";

            mycarddeap = UtilApi.GoFindChildByPObjAndName("dz/di/mycarddeap").transform;
            enemyHero = UtilApi.GoFindChildByPObjAndName("enemyhero").transform;
            enemyHeroPower = UtilApi.GoFindChildByPObjAndName("enemyheropower").transform;
            myHero = UtilApi.GoFindChildByPObjAndName("hero").transform;
            myHeroPower = UtilApi.GoFindChildByPObjAndName("myheropower").transform;
            //youturntip = UtilApi.GoFindChildByPObjAndName("youturntip");

            enemycarddeap = UtilApi.GoFindChildByPObjAndName("dz/di/enemycarddeap").transform;
            enemyHand = UtilApi.GoFindChildByPObjAndName("enemyhand").transform;
            enemyHandtarget = UtilApi.GoFindChildByPObjAndName("enemyhandt").transform;
            mybattlefield = UtilApi.GoFindChildByPObjAndName("mybattlefield").transform;
            enemybattlefield = UtilApi.GoFindChildByPObjAndName("enemybattlefield").transform;
        }

        public void banpick()
        {
            //
        }

        bool isfirst = false;
        /// <summary>
        /// 如果自己是服务器且是先手,是由dzclient sendmsg过来
        /// 如果自己是客户端且是后手,是由rpc用
        /// </summary>
        [RPC]
        void youfirst()
        {
            isfirst = true;
        }

        List<Transform> cs = new List<Transform>();
        AnimationClip moveto(Vector3 s, Vector3 e)
        {
            AnimationClip ret = new AnimationClip();
            ret.SetCurve("", typeof(Transform), "localPosition.x", AnimationCurve.Linear(0, s.x, 1, e.x));
            ret.SetCurve("", typeof(Transform), "localPosition.y", AnimationCurve.Linear(0, s.y, 1, e.y));
            ret.SetCurve("", typeof(Transform), "localPosition.z", AnimationCurve.Linear(0, s.z, 1, e.z));
            return ret;
        }

        //第一次抽,hero发送过来
        public IEnumerator firstdarw()
        {
            Vector3[] fpoint = new Vector3[4];
            fpoint[0] = new Vector3(-1.5f, 0.3314696f, 0.1293559f);
            fpoint[1] = new Vector3(-0.3332209f, 0.3314696f, 0.1293559f);
            fpoint[2] = new Vector3(0.8387426f, 0.3314696f, 0.1293559f);
            fpoint[3] = new Vector3(1.98422f, 0.3314696f, 0.1293559f);

            //不管怎么样都抽3张
            for (int x = 0; x < 3; x++)
            {
                Transform c = newcard();
#if UNITY_5
                c.GetComponent<Animation>().AddClip(moveto(mycarddeap.position, fpoint[x]), "come");
                c.GetComponent<Animation>().Play("cardxuanzhuan");
                c.GetComponent<Animation>().Blend("come", 60);
#elif UNITY_4_6
                c.animation.AddClip(moveto(mycarddeap.position, fpoint[x]), "come");
                c.animation.Play("cardxuanzhuan");
                c.animation.Blend("come", 60);
#endif

                yield return new WaitForSeconds(0.5f);
                //加入到数组中
                cs.Add(c);
                //向对面发送
                //networkView.RPC("enemydraw", RPCMode.Others);
            }

            //把硬币动画做出来
            if (isfirst)
            {
                GameObject.Find("luckycoin").SendMessage("first");
            }
            else
            {
                GameObject.Find("luckycoin").SendMessage("cost");
            }

            yield return new WaitForSeconds(2.5f);//等动画完成

            if (!isfirst)
            {
                //向对面发送
                //networkView.RPC("enemydraw", RPCMode.Others);
                //再抽一张
                //实例第4张
                Transform c = newcard();
#if UNITY_5
                c.GetComponent<Animation>().AddClip(moveto(mycarddeap.position, fpoint[3]), "come");
                c.GetComponent<Animation>().Play("cardxuanzhuan");
                c.GetComponent<Animation>().Blend("come", 60);
#elif UNITY_4_6
                c.animation.AddClip(moveto(mycarddeap.position, fpoint[3]), "come");
                c.animation.Play("cardxuanzhuan");
                c.animation.Blend("come", 60);
#endif
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                //应用itween
                //卡牌分开..mininum
                int x = 1;
                foreach (Transform t in cs)
                {
                    iTween.MoveBy(t.gameObject, Vector3.right * 0.3f * x, 1);
                    x++;
                }
            }

            //出现标题与替换
            transform.FindChild("btandbtn").gameObject.SetActive(true);
        }
        //实例出一张
        Transform newcard()
        {
            Transform c = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(CardType.CARDTYPE_ATTEND).getObject(), new Vector3(10, 10, 10), transform.rotation) as GameObject).transform;

            c.parent = dzban;
            return c;
        }

        List<Transform> willban = new List<Transform>();
        void addban(Transform t)
        {
            if (!myradey)
            {
                willban.Add(t);
            }
        }

        void subban(Transform t)
        {
            if (!myradey)
            {
                willban.Remove(t);
            }
        }
        public Transform mycarddeap;

        public IEnumerator replace()
        {

            for (int x = 0; x < willban.Count; x++)
            {
                Transform t = willban[x];
                Vector3 now = t.localPosition;

                AnimationClip back = moveto(now, mycarddeap.position);
#if UNITY_5
                t.GetComponent<Animation>().AddClip(back, "back");

                //让转倒播
                t.GetComponent<Animation>()["cardxuanzhuan"].speed = -1;
                t.GetComponent<Animation>()["cardxuanzhuan"].time = 1;

                t.GetComponent<Animation>().Play("cardxuanzhuan");
                //混合播
                t.GetComponent<Animation>().Blend("back", 60);
#elif UNITY_4_6
                t.animation.AddClip(back, "back");

                //让转倒播
                t.animation["cardxuanzhuan"].speed = -1;
                t.animation["cardxuanzhuan"].time = 1;

                t.animation.Play("cardxuanzhuan");
                //混合播
                t.animation.Blend("back", 60);
#endif

                yield return new WaitForSeconds(0.5f);

                AnimationClip come = moveto(mycarddeap.position, now);
                //实例一张
                Transform c = newcard();
                c.parent = dzban;
#if UNITY_5
                c.GetComponent<Animation>().AddClip(come, "come");
                //倒播
                c.GetComponent<Animation>().Play("cardxuanzhuan");
                c.GetComponent<Animation>().Blend("come", 60);
#elif UNITY_4_6
                c.animation.AddClip(come, "come");
                //倒播
                c.animation.Play("cardxuanzhuan");
                c.animation.Blend("come", 60);
#endif
                yield return new WaitForSeconds(0.5f);
                UtilApi.Destroy(t.gameObject);
            }

            //向对手发送已经ok
            myradey = true;
            //networkView.RPC("banok", RPCMode.Others);

            if (myradey && enemyradey)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(bancardgohand());
            }
        }

        bool myradey = false;
        bool enemyradey = false;
        [RPC]
        void banok()
        {
            enemyradey = true;

            if (myradey && enemyradey)
            {
                Ctx.m_instance.m_coroutineMgr.StartCoroutine(bancardgohand());
            }
        }

        /// <summary>
        /// 让替换的卡牌到手牌手牌区
        /// </summary>
        IEnumerator bancardgohand()
        {
            Debug.Log("bancardgohand");
            //不管你是先完成还是先完成replace的,都会到这里

            //让手牌到手牌区
            iTween.MoveTo(dzban.gameObject, new Vector3(0, 0, -2.66f), 0.5f);

            mHnadTarget = GameObject.Find("handt").transform;

            //一共共2个单位 分给所有人
            int x = 1;
            //间隔值
            float interval = 2.0f / dzban.childCount;
            float start = -1;

            foreach (Transform t in dzban.transform)
            {
                //进行排列
                Vector3 p = t.localPosition;
                p.x = start + x * interval;
                t.localPosition = p;
                //看向target
                t.LookAt(mHnadTarget);
                //修正
                t.Rotate(Vector3.up, 180f);
                //旋转x轴,
                t.Rotate(Vector3.forward, -8f);

                t.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                x++;
            }
            //等待动画完成
            yield return new WaitForSeconds(0.5f);

            //幸运币消失,
            Transform l = GameObject.Find("luckycoin").transform;

            if (isfirst)
            {
                //先手
                myHero.SendMessage("setGameID", 1);
                enemyHero.SendMessage("setGameID", 2);

                //回合开始
                turnBegin();
            }
            else
            {//后手
                myHero.SendMessage("setGameID", 2);
                enemyHero.SendMessage("setGameID", 1);
                //Todo实例一张幸运币卡
                Transform lucky = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(CardType.CARDTYPE_ATTEND).getObject(), l.position, Quaternion.identity) as GameObject).transform;
                lucky.parent = mHand;
                lucky.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

            UtilApi.Destroy(l.gameObject);
            clearUpHand();
            //把ban改为手牌
            dzban.name = "hand";
            //把英雄设为英雄类型
            myHero.SendMessage("setType", CardType.CARDTYPE_HERO);

            //把英雄设为英雄类型
            enemyHero.SendMessage("setType", CardType.CARDTYPE_HERO);
        }

        Transform dzban;
        Transform mHand;
        Transform mHnadTarget;
        /// <summary>
        /// 整理手牌,
        /// </summary>
        void clearUpHand()
        {
            // cardskin.isdrag = true;
            //一共共2个单位 分给所有人
            int x = 1;
            //间隔值
            float interval = 2.0f / dzban.childCount;
            float start = -1;

            foreach (Transform t in dzban.transform)
            {
                //进行排列
                Vector3 p = new Vector3(start + x * interval, 0.3253887f, 0.1255994f);
                p = mHand.TransformPoint(p);
                iTween.MoveTo(t.gameObject, p, 0.5f);
                iTween.ScaleTo(t.gameObject, Vector3.one * 0.7f, 0.2f);
                //看向target
                t.LookAt(mHnadTarget);
                //修正
                t.Rotate(Vector3.up, 180f);
                //旋转x轴,
                t.Rotate(Vector3.forward, -8f);

                x++;
            }
            //cardskin.isdrag = false;
        }

        public Transform enemyHero;
        public Transform enemyHeroPower;

        public Transform myHero;
        public Transform myHeroPower;
        void BroadcastCanattack()
        {
            //我方战场
            mybattlefield.BroadcastMessage("CanAttack", SendMessageOptions.DontRequireReceiver);
            //我方英雄
            myHero.SendMessage("CanAttack");
            //我方技能
            // myHeroPower.SendMessage("CanAttack");

            //对方战场
            enemybattlefield.BroadcastMessage("CanAttack", SendMessageOptions.DontRequireReceiver);
            //对方英雄
            // enemybattlefield.SendMessage("CanAttack");
            //对方技能
            // myHeroPower.SendMessage("CanAttack");
        }

        public static bool ismyturn = false;
        //public GameObject youturntip;
        [RPC]
        void turnBegin()
        {
            //出现你的回合
            //iTween.ScaleTo(youturntip, Vector3.one, 0.5f);
            //iTween.ScaleTo(youturntip, iTween.Hash(
            //    "scale", Vector3.one * 0.00001f,
            //    "time", 0.5f,
            //    "delay", 1f
            //    ));

            //结束回合向上
            GameObject.Find("dz").transform.FindChild("turn").SendMessage("myturn");

            ismyturn = true;
            draw();
            addCostMax();
            restoreCost();
        }
        //public Transform enemycard;
        public Transform enemycarddeap;
        public Transform enemyHand;
        public Transform enemyHandtarget;

        [RPC]
        void enemydraw()
        {
            int x = 1;
            int childcount = enemyHand.childCount + 1;
            //间隔值
            float interval = 2.0f / childcount;
            float start = -1;

            foreach (Transform t in enemyHand)
            {
                //进行排列
                Vector3 p = new Vector3(start + x * interval, 0, 0);
                p = enemyHand.TransformPoint(p);
                iTween.MoveTo(t.gameObject, p, 0.5f);

                x++;
            }

            //实例一张敌人卡
            Transform c = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getEnemyCardModel().getObject(), enemycarddeap.position, transform.rotation) as GameObject).transform;
            c.Rotate(-90f, -90f, 0);

            Vector3 pp = new Vector3(start + x * interval, 0, 0);
            pp = enemyHand.TransformPoint(pp);

            iTween.MoveTo(c.gameObject, pp, 1f);
            c.parent = enemyHand;

            //看向target
            //c.LookAt(enemyHandtarget);
            iTween.RotateTo(c.gameObject, new Vector3(-45, 0, 0), 1f);
        }

        /// <summary>
        /// 抽牌
        /// </summary>
        public void draw()
        {
            //向对面发送
            //networkView.RPC("enemydraw", RPCMode.Others);

            // cardskin.isdrag = true;

            Transform c = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getSceneCardModel(CardType.CARDTYPE_ATTEND).getObject(), mycarddeap.position, transform.rotation) as GameObject).transform;
            c.parent = mHand;
            c.Rotate(-90f, -90f, 0);
            clearUpHand();
        }

        List<Transform> cost = new List<Transform>();
        int costMax;
        void restoreCost()
        {
            restoreCost(costMax - cost.Count);
        }

        void restoreCost(int num)
        {
            //让所有向右移动0.182498
            foreach (Transform t in cost)
            {
                iTween.MoveBy(t.gameObject, Vector3.right * 0.182498f, 0.5f);
            }
            //实例水晶
            for (int x = 0; x < num; x++)
            {
                Transform c = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getcostModel().getObject()) as GameObject).transform;
                iTween.ShakeScale(c.gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
                cost.Add(c);
            }
            //更新文本
            updataMyCostText();
        }

        void addCostMax()
        {
            if (costMax != 10)
            {
                costMax++;
                updataMyCostText();
            }
        }

        //UILabel mycosttext, enemycosttext;
        Text mycosttext, enemycosttext;
        void updataMyCostText()
        {
            mycosttext.text = cost.Count + "/" + costMax;
            //向对面发送
            //networkView.RPC("costtext", RPCMode.Others, mycosttext.text);
        }

        [RPC]
        void costtext(string text)
        {
            enemycosttext.text = text;
        }

        //public Transform minionmoder;
        public Transform mybattlefield;
        void playCard(Transform t)
        {
            foreach (Transform bc in mybattlefield)
            {
                iTween.MoveBy(bc.gameObject, Vector3.left * 0.4f, 0.1f);
            }

            t.parent = mybattlefield;
            //移动到位置上去
            //0.8   6.4
            //-3.2

            int count = mybattlefield.childCount + 1;

            float length = count * 0.8f;
            float offset = -length / 2;

            Vector3 startp = Vector3.zero;
            startp.x = offset + length / count;

            startp.x = -startp.x;
            Debug.Log(-startp.x);
            iTween.MoveTo(t.gameObject, iTween.Hash(
                "position", startp,
                "islocal", true,
                "time", 0.2f,
                "oncomplete", "create",
                "oncompleteparams", t,
                "oncompletetarget", gameObject)
                );
            //向对方通告,
            //networkView.RPC("enemypaly", RPCMode.Others, "id123");
        }

        public Transform enemybattlefield;
        [RPC]
        void enemypaly(string id)
        {
            foreach (Transform bc in enemybattlefield)
            {
                iTween.MoveBy(bc.gameObject, Vector3.left * 0.4f, 0.1f);
            }
            Transform t = enemyHand.GetChild(0);

            t.parent = enemybattlefield;
            //移动到位置上去
            //0.8   6.4
            //-3.2

            int count = enemybattlefield.childCount + 1;

            float length = count * 0.8f;
            float offset = -length / 2;

            Vector3 startp = Vector3.zero;
            startp.x = offset + length / count;

            startp.x = -startp.x;

            iTween.MoveTo(t.gameObject, iTween.Hash(
                "position", startp,
                "islocal", true,
                "time", 0.2f,
                "oncomplete", "create",
                "oncompleteparams", t,
                "oncompletetarget", gameObject)
                );
        }

        /// <summary>
        /// 新建一个仆从的模型.
        /// </summary>
        /// <param name="t"></param>
        void create(Transform t)
        {
            Transform mt = (UtilApi.Instantiate(Ctx.m_instance.m_modelMgr.getMinionModel().getObject(), t.position, mybattlefield.rotation) as GameObject).transform;
            mt.parent = t.parent;
            if (mt.parent == mybattlefield)
            {
                mt.SendMessage("setMine");
            }
            UtilApi.Destroy(t.gameObject);
            clearUpHand();
        }
    }
}