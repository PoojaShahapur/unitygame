using UnityEngine;
using UnityEngine.UI;
using Giant;
using System.Collections.Generic;

public class UIFight : MonoBehaviour,TeamController
{
    public Button btnShoot;
    public RectTransform stick;
    public GameObject dangerTip;
    public Image imgCD;
    public RectTransform rankContent;

    private Stack<RankItem> rankItemCache = new Stack<RankItem>();
    private List<RankItem> rank = new List<RankItem>();
    class RankItem
    {
        public uint id;
        public int score;
        public Transform parent;
        public Text txtRank;
        public Text txtName;
        public Text txtScore;
        public Text txtDis;
        public Transform imgDir;
    }
    private bool shoot = false;

    private void Awake()
    {
        for (int i = 0; i < rankContent.childCount; ++i)
        {
            CacheRankItem(rankContent.GetChild(i));
        }
    }

    private void Update()
    {
        if (SceneObject.scene == null)
            return;
        var myTeam = SceneObject.scene.myTeam;
        if (myTeam != null)
        {
            var myTeamPos = myTeam.pos;
            RankItem item = null;
            for (int i = 0; i < rank.Count; ++i)
            {
                item = rank[i];
                TrangleTeam team = SceneObject.scene.GetTrangleTeam(item.id);
                if (team != null)
                {
                    var targetTeamOut = team.pos;
                    if (!team.IsMainTeam)
                    {
                        item.imgDir.localRotation = Quaternion.FromToRotation(Vector3.up, (targetTeamOut - myTeamPos));
                        item.txtDis.text = Vector2.Distance(targetTeamOut, myTeamPos).ToString();
                    }
                }
            }
        }
    }

    private void CacheRankItem(Transform t)
    {
        var item = new RankItem();
        item.txtRank = t.Find("txtRank").GetComponent<Text>();
        item.txtName = t.Find("txtName").GetComponent<Text>();
        item.txtScore = t.Find("txtPlanes").GetComponent<Text>();
        item.txtDis = t.Find("txtDis").GetComponent<Text>();
        item.imgDir = t.Find("imgDir");
        item.parent = t;
        t.gameObject.SetActive(false);
        rankItemCache.Push(item);
    }

    private void CacheRankItem(uint num)
    {
        var cloneObj = rankContent.GetChild(0).gameObject;
        for (int i = 0; i < num; ++i)
        {
            var obj = GameObject.Instantiate(cloneObj);
            var t = obj.transform;
            t.SetParent(rankContent);
            t.localScale = Vector3.one;
            this.CacheRankItem(t);
        }
    }

    private RankItem newRankItem()
    {
        if (rankItemCache.Count == 0)
        {
            this.CacheRankItem(2);
        }
        var item = this.rankItemCache.Pop();
        item.parent.gameObject.SetActive(true);
        return item;
    }

    private void Rank()
    {
        rank.Sort((player_1, player_2) =>
        {
             return player_2.score - player_1.score;
        });
        for (int i = 0; i < rank.Count; ++i)
        {
            rank[i].parent.SetSiblingIndex(i);
            rank[i].txtRank.text = (i + 1).ToString();
        }
    }

    public void OnNewPlayer(uint id)
    {
        var team = SceneObject.scene.GetTrangleTeam(id);
        if (team != null)
        {
            var item = newRankItem();
            item.id = id;
            item.txtName.text = team.TeamInfo.name;
            item.score = team.TrangleCount;
            item.txtScore.text = item.score.ToString();
            item.imgDir.gameObject.SetActive(!team.IsMainTeam);
            if (team.IsMainTeam)
                item.txtDis.text = "自己";
            rank.Add(item);

            Rank();
            this._updateContentHeight();
        }
    }

    public void OnPlayerChange(uint id)
    {
        var team = SceneObject.scene.GetTrangleTeam(id);
        var player = rank.Find((item) =>
        {
            return item.id == id;
        });
        if (player != null)
        {
            player.score = team.TrangleCount;
            player.txtScore.text = player.score.ToString();
        }
        Rank();
    }

    //回收item
    public void OnRemovePlayer(uint id)
    {
        var player = rank.Find((item) =>
        {
            return item.id == id;
        });
        if (player != null)
        {
            rank.Remove(player);
            player.parent.gameObject.SetActive(false);
            rankItemCache.Push(player);
        }
        this._updateContentHeight();
    }

    private void _updateContentHeight()
    {
        var size = this.rankContent.sizeDelta;
        if (rank.Count > 0)
        {
            size.y = rank[0].parent.GetComponent<LayoutElement>().preferredHeight * rank.Count;
        }
        this.rankContent.sizeDelta = size;
    }

    private void OnEnable()
    {
        btnShoot.onClick.AddListener(OnShootClick);
    }

    private void OnDisable()
    {
        btnShoot.onClick.RemoveListener(OnShootClick);
    }

    private void OnShootClick()
    {
        shoot = true;
    }

    private void OnReliveClick()
    {
        //Game.Instantiate.ReliveMainTeam();
    }

    public Vector2 GetDir()
    {
        if (stick.localPosition == Vector3.zero)
            return Vector2.zero;
        else
            return stick.parent.TransformDirection(stick.localPosition);
    }

    public bool GetShoot()
    {
        var ret = shoot;
        shoot = false;
        return ret;
    }

    public void SetDanger(bool danger)
    {
        dangerTip.SetActive(danger);
    }


    public void SetShootCD(float per)
    {
        imgCD.fillAmount = per;
        btnShoot.interactable = per <= 0;
    }

    public void Close()
    {
        GameObject.Destroy(this.gameObject);
    }
}
