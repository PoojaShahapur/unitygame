using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.BEP.Vol._2.Scripts;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {
    public ScrollRect rankArea;
    private RectTransform rankGrid;
    public GameObject myRankArea;
    public GameObject listItemPrefab;
    private List<SceneEntity> topN = new List<SceneEntity>();
    private List<GameObject> items = new List<GameObject>();

    // Use this for initialization
    void Start () {
        rankGrid = rankArea.content;

        //反序列化获取排名信息
        string rankInfoStr = PlayerPrefs.GetString("RankInfoStr");
        string[] rankliststr = rankInfoStr.Split('\n');
        int itemCount = rankliststr.Length;
        for (int i=0; i<itemCount; ++i)
        {
            SceneEntity _resDT = new SceneEntity();
            _resDT = (SceneEntity)SerializeUtilities.Desrialize(_resDT, rankliststr[i]);
            topN.Add(_resDT);
        }

        float girdHeight = itemCount * 50 - (itemCount - 1) * 2;
        rankGrid.sizeDelta = new Vector2(800, girdHeight);

        CreateListItem();
        rankArea.verticalNormalizedPosition = 1;
    }
		
    void CreateListItem()
    {
        for(int i=0; i<topN.Count;++i)
        {
            float y_pos = -25 - 48 * i;//-25:第一个item的起始位置，48为item高度50-2(缩进)
            GameObject food = Instantiate(listItemPrefab, new Vector3(400, y_pos, 0), Quaternion.identity) as GameObject;
            food.transform.parent = rankGrid;
            food.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            food.name = "Item" + i;

            items.Add(food);
        }
    }

    private string getMassTextByScale(float scale)
    {
        float mass = MathToolManager.getMassByRadius(scale);
        if (mass >= 1000.0f)
        {
            mass /= 1000.0f;
            return mass.ToString("F1") + "t";
        }
        else
        {
            return mass.ToString("F1") + "kg";
        }
    }

    void SetMyRankInfo()
    {
        for (int i = 1; i <= topN.Count; ++i)
        {
            if(!topN[i-1].m_isRobot)
            {
                //排名
                Text myRank = myRankArea.transform.FindChild("Rank").GetComponent<Text>() as Text;
                myRank.text = "" + i;

                //头像
                Image myAvatar = myRankArea.transform.FindChild("Avatar").GetComponent<Image>() as Image;
                Sprite avatarSprite = Resources.Load("DefaultAvatar", typeof(Sprite)) as Sprite;
                myAvatar.overrideSprite = avatarSprite;

                //用户名
                Text myName = myRankArea.transform.FindChild("Name").GetComponent<Text>() as Text;
                myName.text = topN[i - 1].m_name;

                //本轮质量
                Text myMass = myRankArea.transform.FindChild("Mass").GetComponent<Text>() as Text;
                myMass.text = getMassTextByScale(topN[i - 1].m_radius);

                //吞食数量
                Text mySwallowNum = myRankArea.transform.FindChild("SwallowNum").GetComponent<Text>() as Text;
                mySwallowNum.text = "" + topN[i - 1].m_swallownum;

                break;
            }
        }        
    }   

    void SetTopXRankInfo()
    {
        for (int i = 1; i <= topN.Count; ++i)
        {
            string childName = "Item" + i;
            Transform itemTransform = items[i-1].transform;
            //排名
            Text rank = itemTransform.FindChild("Rank").GetComponent<Text>() as Text;
            rank.text = "" + i;

            //头像
            Image avatar = itemTransform.FindChild("Avatar").GetComponent<Image>() as Image;
            Sprite avatarSprite = Resources.Load("DefaultAvatar", typeof(Sprite)) as Sprite;
            avatar.overrideSprite = avatarSprite;

            //用户名
            Text name = itemTransform.FindChild("Name").GetComponent<Text>() as Text;
            name.text = topN[i - 1].m_name;

            //本轮质量
            Text mass = itemTransform.FindChild("Mass").GetComponent<Text>() as Text;
            mass.text = getMassTextByScale(topN[i - 1].m_radius);

            //吞食数量
            Text swallowNum = itemTransform.FindChild("SwallowNum").GetComponent<Text>() as Text;
            swallowNum.text = "" + topN[i - 1].m_swallownum;
        }        
    }

    void OnGUI()
    {
        SetMyRankInfo();
        SetTopXRankInfo();
    }


    public void OnClickBackBtn()
    {
        SceneManager.LoadScene("StartGame");
    }

    void OnDestroy()
    {

    }
}
