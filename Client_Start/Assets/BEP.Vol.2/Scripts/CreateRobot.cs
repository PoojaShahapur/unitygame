using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.BEP.Vol._2.Scripts;


static class RandomExtensions
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {        
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}

public class CreateParam
{
    public string name;
    public uint charid;

    public CreateParam(string n, uint i)
    {
        name = n;
        charid = i;
    }
}

public class CreateRobot : MonoBehaviour
{
    public GameObject SFood;
    public float xlimit_min = 120;
    public float xlimit_max = 190;
    public float zlimit_min = 230;
    public float zlimit_max = 250;
    public float y_height = 1.65f;

    public int small_Max_Num = 2;
    public float small_Min_Radius = 0.5f;
    public float small_Max_Radius = 1.5f;

    public int big_Max_Num = 2;
    public float big_Min_Radius = 3.0f;
    public float big_Max_Radius = 5.0f;

    private uint foodsNum = 0;

    public float startTime = 1.0f;//开始startTime时长后开始产生
    public float createTime = 6.0f;//雪球产生间隔

    public static CreateRobot Instance;
    private bool createFinish = false;
    public List<CreateParam> toBeCreateNames = new List<CreateParam>();
    //public GameObject player;//玩家
    private string randomName = "哈帅酷妹美吊炸天炫霸动萌可爱傻笨猪";
    private string[] allNameArray;//所有可用的名字列表

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        // 冒泡得到所有2个字的名字 11 * 10 = 110
        InvokeRepeating("CreateSnowFood", startTime, createTime);        
        List<string> allAvaliableNames = new List<string>();
        getAllPermutation(randomName, ref allAvaliableNames);
        allNameArray = allAvaliableNames.ToArray();
        new System.Random().Shuffle<string>(allNameArray);
    }

    // 并非完全的排列组合,就冒泡吧, N*(N-1)种
    void getAllPermutation(string name, ref List<string> nameList)
    {
        for(int i = 0; i < name.Length; ++i)
        {
            for(int j = i;j < name.Length; ++j)
            {
                nameList.Add(name[i].ToString() + name[j].ToString());
            }
        }        

        for (int i = 0; i < name.Length; ++i)
        {
            for (int j = i; j < name.Length && (j+1) < name.Length; ++j)
            {
                nameList.Add(name[i].ToString() + name[j].ToString() + name[j + 1].ToString());
            }
        }
    }
   
    public void CreateSnowFood()
    {
        if (!createFinish && foodsNum < small_Max_Num + big_Max_Num)
        {
            float scaleRate = 1.0f;
            if (foodsNum < small_Max_Num)
            {
                scaleRate = Random.Range(small_Min_Radius, small_Max_Radius);
            }
            else//小球已用完
            {
                scaleRate = Random.Range(big_Min_Radius, big_Max_Radius);
            }

            float x = Random.Range(xlimit_min, xlimit_max);
            float z = Random.Range(zlimit_min, zlimit_max);
            GameObject food = Instantiate(SFood, new Vector3(x, y_height * scaleRate, z), Quaternion.identity) as GameObject;
           
            food.GetComponent<Food>().GetComponent<Transform>().localScale = new Vector3(scaleRate, scaleRate, scaleRate);//缩放
            food.GetComponent<Food>().SetIsRobot(true);            
            food.GetComponent<AI>().SetPlayer(CreatePlayer._Instace.player);
            food.name = allNameArray[(int)foodsNum];
            //log.logHelper.DebugLog("CreateSnowFood" + food.name);
            food.GetComponent<Food>().entity.m_charid = foodsNum + 1;
            food.GetComponent<Food>().entity.m_canEatRate = food.GetComponent<Food>().canEatRate;
            food.GetComponent<Food>().setMyName(food.name);            
            food.GetComponent<Food>().setEntity(food);
            //Debug.Log(food.name + "y位置：" + food.transform.position.y + "   半径：" + scaleRate);

            ++foodsNum;
        }
        else
        {
            createFinish = true;
            GameObjectManager.getInstance().CreateSnowBall();
        }
    }

    public void CreateSnowFoodWithNameCharID(string name, uint charid)
    {
        if (foodsNum < small_Max_Num + big_Max_Num)
        {
            float scaleRate = 1.0f;
            if (foodsNum < small_Max_Num)
            {
                scaleRate = Random.Range(small_Min_Radius, small_Max_Radius);
            }
            else//小球已用完
            {
                scaleRate = Random.Range(big_Min_Radius, big_Max_Radius);
            }

            float x = Random.Range(xlimit_min, xlimit_max);
            float z = Random.Range(zlimit_min, zlimit_max);
            GameObject food = Instantiate(SFood, new Vector3(x, y_height * scaleRate, z), Quaternion.identity) as GameObject;

            food.GetComponent<Food>().GetComponent<Transform>().localScale = new Vector3(scaleRate, scaleRate, scaleRate);//缩放
            food.GetComponent<Food>().SetIsRobot(true);
            food.GetComponent<AI>().SetPlayer(CreatePlayer._Instace.player);
            food.name = name;
            //log.logHelper.DebugLog("创建物件名" + name);
            food.GetComponent<Food>().entity.m_charid = charid;
            food.GetComponent<Food>().entity.m_canEatRate = food.GetComponent<Food>().canEatRate;
            food.GetComponent<Food>().setMyName(food.name);
            food.GetComponent<Food>().setEntity(food);
            //Debug.Log(food.name + "y位置：" + food.transform.position.y + "   半径：" + scaleRate);

            ++foodsNum;
        }
    }


    public void ResetFoodsNum()
    {
        foodsNum = 0;
    }

    public void subFoodsNum(string name, uint charid)
    {        
        if (foodsNum >= 1)
        {
            --foodsNum;
        }        
        toBeCreateNames.Add(new CreateParam(name, charid));        
    }
}
