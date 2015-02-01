using UnityEngine;
using System.Collections;
using Game.UI;
using UnityEngine.UI;
using SDK.Common;
using SDK.Lib;

namespace Game.UI
{
    /// <summary>
    /// 对战时仆从的逻辑，就是self hero 最底下的牌
    /// </summary>
    public class dzminion : InterActiveEntity
    {
        public static GameObject attacker = null, underattacker = null;
        int health = 10, healthbuff = 0;
        int attack = 1, attackbuff = 0;
        //spell spell;
        public int gameID;
        bool ismine = false;
        bool iscanattack;
        public override void Start()
        {
            //得到游戏id
            //从id池中得到id;
            getGameID();
            originaLposition = Vector3.one * 100f;
        }

        Vector3 originaLposition;
        void OnMouseDown()
        {
            if (!ismine && !dzcam.ismyturn) //不是我的,和不是我的回合
            {
                return;
            }

            if (!iscanattack)
            {
                //不能攻击
                return;
            }

            iTween.Stop(gameObject);

            if (originaLposition == Vector3.one * 100f)
            {
                originaLposition = transform.position;
            }
            else
            {
                transform.position = originaLposition;
            }

            attacker = gameObject;
            //上升
            iTween.MoveBy(gameObject, Vector3.up * 1f, 0.5f);
        }

        void OnMouseUp()
        {
            if (!iscanattack && !dzcam.ismyturn)
            {
                //不能攻击
                return;
            }

            if (underattacker == attacker || underattacker == null)
            {
                //下降
                iTween.MoveTo(gameObject, originaLposition, 0.5f);
                return;
            }
            else
            {
                //进攻
                iTween.MoveFrom(gameObject, iTween.Hash("position", underattacker.transform.position, "time", 0.3f));
                //下降
                iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3.up * -1f, "delay", 0.3f, "time", 0.5f));
                CanNotAttack();
                Attack();
                //让对方计算
                //Camera.main.SendMessage("attack");
                Ctx.m_instance.m_camSys.m_dzcam.attack();
            }
        }

        void OnMouseEnter()
        {
            if (attacker == null) //没有在攻击,什么都不做
            {
                return;
            }

            underattacker = gameObject;

            if (attacker != null) //正在攻击
            {
                //用于显示死人头
                showwilldeath();
            }
        }

        //GameObject willdeathpic;
        void OnMouseExit()
        {
            if (attacker == null) //没有在攻击,什么都不做
            {
                return;
            }

            underattacker = null;
            //用于消失死人头
            hidewilldeath();
        }

        void hidewilldeath()
        {
            if (type != CardType.CARDTYPE_HERO && type != CardType.CARDTYPE_SKILL)
            {
                transform.FindChild("willdeath").gameObject.SetActive(false);
            }
        }

        void showwilldeath()
        {
            if (type != CardType.CARDTYPE_HERO && type != CardType.CARDTYPE_SKILL)
            {
                transform.FindChild("willdeath").gameObject.SetActive(true);
            }
        }

        void OnCollisionEnter()
        {
            if (transform.rigidbody != null)
            {
                UtilApi.Destroy(transform.rigidbody);
            }
        }

        /// <summary>
        /// 移除PLBUFF,如果有的话
        /// </summary>
        void RemovePL()
        {
            if (transform.FindChild("lz") == null)
            {
                //是没有粒子的
                return;
            }

            ParticleSystem p = transform.FindChild("lz").particleSystem;
            UtilApi.Destroy(p);
        }

        void CanAttack()
        {
            RemovePL();
            iscanattack = true;
            //亮起来
        }

        void CanNotAttack()
        {
            iscanattack = false;
            //暗下去
        }

        public void setMine()
        {
            ismine = true;
        }

        void underAttack()
        {
            //计算出伤害
            //得到攻击者的攻击力
            //dzminion target = attacker.GetComponent<dzminion>();
            dzminion target = null;
            int targetat = target.getAttack();
            healthbuff -= targetat;
            //显示伤害
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(showDemage(targetat));
        }

        int getAttack()
        {
            return attack + attackbuff;
        }

        int getHealth()
        {
            return health + healthbuff;
        }

        void Attack()
        {
            //向被攻击者发送消息
            underattacker.SendMessage("underAttack");
            //计算出伤害
            //得到被攻击者的攻击力
            //dzminion target = underattacker.GetComponent<dzminion>();
            dzminion target = null;
            int targetat = target.getAttack();
            healthbuff -= targetat;
            //显示伤害
            Ctx.m_instance.m_coroutineMgr.StartCoroutine(showDemage(targetat));
        }

        void showanim()
        {
            //进攻
            iTween.MoveFrom(gameObject, iTween.Hash("position", underattacker.transform.position, "time", 0.3f));
            Attack();
        }

        IEnumerator showDemage(int demage)
        {
            Transform de = transform.FindChild("demegadi");
            de.gameObject.SetActive(true);
            //UILabel text = de.FindChild("demega").GetComponent<UILabel>();
            Text text = de.FindChild("demega").GetComponent<Text>();
            text.text = "-" + demage.ToString();
            //更新自己的血量
            UpdateHealth();
            yield return new WaitForSeconds(1f);
            de.gameObject.SetActive(false);

            if (getHealth() <= 0) //如果为0 GG
            {
                if (type == CardType.CARDTYPE_HERO)
                {
                    if (name == "hero")
                    {
                        //失败
                        //Camera.main.SendMessage("defeat");
                        Ctx.m_instance.m_camSys.m_dzcam.defeat();
                    }
                    else
                    {
                        //胜利
                        //Camera.main.SendMessage("victory");
                        Ctx.m_instance.m_camSys.m_dzcam.victory();
                    }
                }
                UtilApi.Destroy(gameObject);
            }
        }

        void UpdateHealth()
        {
            //UILabel t = transform.FindChild("health").GetComponent<UILabel>();
            Text t = transform.FindChild("health").GetComponent<Text>();
            t.text = getHealth().ToString();

            if (healthbuff < 0) //红色
            {
                t.color = Color.red;
            }
            else if (healthbuff == 0)//白色
            {
                t.color = Color.white;
            }
            else//绿色
            {
                t.color = Color.green;
            }
        }

        CardType type = CardType.CARDTYPE_ATTEND;
        void setType(CardType t)
        {
            type = t;
        }

        void getGameID()
        {
            gameID = dzcam.GameIdcount;
            dzcam.GameIdcount++;
        }

        void setGameID(int id)
        {
            gameID = id;
        }
    }
}