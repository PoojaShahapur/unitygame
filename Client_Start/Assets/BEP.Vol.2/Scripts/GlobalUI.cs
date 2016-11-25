using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.BEP.Vol._2.Scripts;

// 先存下来, 调用 GUI.BeginGroup  之后 GUI.Label 直接从该结构体中取 (x, y, width, height)
public class OneNamePosition
{
    public float startX;
    public float startY;
    public float width;
    public float height;
    public int rank;//名次,用于对第1,2,3名特殊处理
    public string name;//玩家名

    public OneNamePosition(float x = 0, float y = 0, float w = 0, float h = 0)
    {
        startX = x;
        startY = y;
        width = w;
        height = h;
        rank = 0;
        name = "";
    }
}

public class GlobalUI : MonoBehaviour
{
    public GameObject reliveArea;//复活界面
    public Text countdownText;//游戏倒计时
    public Text massText;//重量

    //public GameObject player;
    public int totalGameSeconds = 300;//游戏总共持续的秒数    

    private int leftMinutes = 0;//剩余的分钟数
    private int leftSeconds = 0;//剩余的秒数
    private int endtimeStamp = 0;

    private int auto_relive_seconds = 5;
    private int old_auto_relive_seconds = 5;
    private bool is_relive_area_visible = false;

    // 排行榜相关的变量放在下面
    private GUIStyle imageInBoxNoPadding;//图片在box控件时候不要有内边距
    public GUIStyle firstRankStyle;//第1名的文字格式
    public GUIStyle secondRankStyle;//第2名的文字格式
    public GUIStyle thirdRankFontStyle;//第3名的文字格式
    public GUIStyle otherRankStyle;//其他排名的文字及格式
    public Texture firstRankImage;//第1名的图片
    public Texture secondRankImage;//第2名的图片
    public Texture thirdRankImage;//第3名的图片        
    
    public float rankDialogWidth_Percent = 0.19f;//排行榜界面占屏幕宽的百分比
    public float rankDialofHeight_Percent = 0.45f;//排行榜界面占屏幕高的百分比
    public float rankDialogStartX_Percent = 0.0f;//排行榜界面,左上角x的起始坐标占屏宽的百分比
    public float rankDialogStartY_Percent = 0.0f;//排行榜界面,左上角y的起始坐标占屏宽的百分比    
    public float dialogDistanceY_Percent = 0.001f;//排行榜对话框和前10名对话框之间的距离;前10名对话框和玩家排名对话框之间的间距,占屏高比例
    public float distanceBetweenNames_Percent = 0.0001f;//两名玩家之间的距离
    //public Texture rankBackGroundImage;//排行榜的背景图片

    public static GlobalUI _Instance;
    void setTextByLeftSeconds(int totalLeftSeconds)
    {
        leftMinutes = totalLeftSeconds / 60;
        leftSeconds = totalLeftSeconds % 60;
        countdownText.text = "剩余时间 " + leftMinutes.ToString("D2") + "分" + leftSeconds.ToString("D2") + "秒";
    }

    void SetMyMass(float radius = 0.0f)
    {
        massText.text = "重量 " + getMassTextByScale(radius);
    }

    public string getMassTextByScale(float scale)
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

    void Awake()
    {
        _Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        reliveArea.SetActive(false);

        setTextByLeftSeconds(totalGameSeconds);
        endtimeStamp = (int)Time.time + totalGameSeconds;
        //InvokeRepeating("ShowRankList", 3, 5);//开始三秒后每五秒排序一次
        if (0.0f == rankDialogStartX_Percent)
        {
            rankDialogStartX_Percent = 1.0f - rankDialogWidth_Percent;
        }

        imageInBoxNoPadding = new GUIStyle("box");
        imageInBoxNoPadding.padding = new RectOffset(0, 0, 0, 0);

        auto_relive_seconds = CreatePlayer._Instace.auto_relive_seconds;
        old_auto_relive_seconds = auto_relive_seconds;
    }

    // Update is called once per frame
    void Update()
    {
        int totalLeftSeconds = endtimeStamp - (int)Time.time;
        setTextByLeftSeconds(totalLeftSeconds);
        if (CreatePlayer._Instace.player != null) SetMyMass(CreatePlayer._Instace.player.GetComponent<Transform>().localScale.x);
        if (is_relive_area_visible) ShowReliveTime();//复活倒计时

        if (totalLeftSeconds <= 0)
        {
            SaveRankInfo();//存储排名信息

            CreateRobot.Instance.ResetFoodsNum();
            GameObject.Destroy(this.gameObject);
            SceneManager.LoadScene("EndGame");
        }
    }


    private float totalTime = 0;
    public void ResetReliveSeconds()
    {
        totalTime = 0;
        auto_relive_seconds = old_auto_relive_seconds;
        //刷新掉上次隐藏时的显示时间
        Text _btn_small = reliveArea.transform.FindChild("ReliveBtn").FindChild("Text").GetComponent<Text>() as Text;
        _btn_small.text = "立即复活（" + auto_relive_seconds + ")";
    }

    public int GetLeftReliveSeconds()
    {
        return auto_relive_seconds;
    }

    private void ShowReliveTime()//复活倒计时
    {
        //累加每帧消耗时间
        totalTime += Time.deltaTime;
        if (totalTime >= 1)//每过1秒执行一次
        {
            auto_relive_seconds--;
            Text _btn_small = reliveArea.transform.FindChild("ReliveBtn").FindChild("Text").GetComponent<Text>() as Text;
            _btn_small.text = "立即复活（"+ auto_relive_seconds + ")";
            totalTime = 0;
        }

        //自动复活
        if(0 == auto_relive_seconds)
        {
            //CreateRobot.Instance.ResetFoodsNum();                    
            //SceneManager.LoadScene("NewWorld");
            ShowReliveArea(false);
            CreatePlayer._Instace.SetIsNiuBi(false);
        }
    }

    public void SetEnemyName(string enemyname)
    {
        Text _btn_small = reliveArea.transform.FindChild("BackRoom").FindChild("Text").GetComponent<Text>() as Text;
        _btn_small.text = "你被" + enemyname + "吃掉了";
    }

    //序列化存储排名信息
    void SaveRankInfo()
    {
        string rankInfoStr = null;
        List<SceneEntity> topN = new List<SceneEntity>();
        GameObjectManager.getInstance().getTopNEntity(ref topN, 0);
        for (int i = 0; i < topN.Count; ++i)
        {
            rankInfoStr += SerializeUtilities.Serialize(topN[i]);
            rankInfoStr += '\n';
        }
        rankInfoStr = rankInfoStr.Remove(rankInfoStr.Length - 1);//将最后一个分隔符删除

        PlayerPrefs.SetString("RankInfoStr", rankInfoStr);
    }

    GUIStyle getStyleByRank(int rank)
    {
        if (1 == rank)
        {
            return firstRankStyle;
        }
        else if(2 == rank)
        {
            return secondRankStyle;
        }
        else if(3 == rank)
        {
            return thirdRankFontStyle;
        }
        else
        {
            return otherRankStyle;
        }        
    }

    void OnGUI()
    {        
        GUI.Box(new Rect(rankDialogStartX_Percent * Screen.width, rankDialogStartY_Percent * Screen.height
            , Screen.width * rankDialogWidth_Percent, Screen.height * rankDialofHeight_Percent), "排行榜");
        //log.logHelper.DebugLog("从坐标(" + rankDialogStartX_Percent * Screen.width + ", " + rankDialogStartY_Percent * Screen.height
          //  + "开始画排行榜三个字,width=" + Screen.width * rankDialogWidth_Percent + ",height=" + Screen.height * rankDialofHeight_Percent);

        // 下面先计算下 group 需要的屏高,因为排行榜界面的屏宽是统一的.
        // 10名文字的高度之和 +  11个名字的间距
        int n = 10;
        List<SceneEntity> topN = new List<SceneEntity>();
        GameObjectManager.getInstance().getTopNEntity(ref topN, n);
        for(int i = 0; i < topN.Count; ++i)
        {
            //log.logHelper.DebugLog("第" + (i+1).ToString() + "名:" + topN[i].m_name + ",半径:" + topN[i].m_object.GetComponent<Transform>().localScale.x);
        }
        float groupHeight = distanceBetweenNames_Percent * Screen.height;
        List<OneNamePosition> labelInfoList = new List<OneNamePosition>();
        for(int i = 0; i < n && i < topN.Count; ++i)
        {
            OneNamePosition label = new OneNamePosition();
            label.startX = rankDialogStartX_Percent * Screen.width;
            label.rank = i + 1;
            label.name = label.rank.ToString() + ". " + topN[i].m_name;
            if (0 == i)//第一名
            {
                label.startY = (rankDialogStartY_Percent + rankDialofHeight_Percent + dialogDistanceY_Percent + distanceBetweenNames_Percent) * Screen.height;                
                Vector2 tmpNameSize = firstRankStyle.CalcSize(new GUIContent(label.name));
                label.width = tmpNameSize.x;
                label.height = tmpNameSize.y;
                labelInfoList.Add(label);
                groupHeight += label.height + distanceBetweenNames_Percent * Screen.height;
                continue;
            }
            label.startY = labelInfoList[i - 1].startY + labelInfoList[i - 1].height + distanceBetweenNames_Percent * Screen.height;
            GUIStyle rankStyle = getStyleByRank(label.rank);
            Vector2 nameSize = rankStyle.CalcSize(new GUIContent(label.name));
            label.width = nameSize.x;
            label.height = nameSize.y;
            labelInfoList.Add(label);
            groupHeight += label.height + distanceBetweenNames_Percent * Screen.height;
        }

        // 下面开始绘制排行榜界面
        float allPlayerInfoStartY = (rankDialogStartY_Percent + rankDialofHeight_Percent + dialogDistanceY_Percent) * Screen.height;
        //log.logHelper.DebugLog("从坐标(" + rankDialogStartX_Percent * Screen.width + ", " + allPlayerInfoStartY
          //  + "开始画玩家名,width=" + Screen.width * rankDialogWidth_Percent + ",height=" + groupHeight);
        GUI.BeginGroup(new Rect(rankDialogStartX_Percent * Screen.width, allPlayerInfoStartY
            , Screen.width * rankDialogWidth_Percent, groupHeight), "");
        GUI.Box(new Rect(0, 0, Screen.width * rankDialogWidth_Percent, groupHeight), "");
        for (int i = 0; i < labelInfoList.Count; ++i)
        {
            //log.logHelper.DebugLog("第" + labelInfoList[i].rank.ToString() + "名:" + labelInfoList[i].name);
            
            GUI.Box(new Rect(0, labelInfoList[i].startY - allPlayerInfoStartY
             , Screen.width * rankDialogWidth_Percent, labelInfoList[i].height), labelInfoList[i].name, getStyleByRank(i + 1));            
        }
        GUI.EndGroup();                    
        
    }

    public void ShowReliveArea(bool show)
    {
        is_relive_area_visible = show;
        reliveArea.SetActive(show);
        CreatePlayer._Instace.SetIsDontMove(show);//重生界面下不可移动
        if(show) CreatePlayer._Instace.SetIsNiuBi(true);//进入无敌时刻，但脱离无敌时刻须在其他地方设置
        //先生成玩家雪球
        if (show)
        {
            CreatePlayer._Instace.OnCreatePlayer();
        }
    }

    void OnDestroy()
    {
    }
}
