using UnityEngine;
using System.Collections;

public class CreateSnowBlock : MonoBehaviour {
    public GameObject SnowBlockPrefab;
    public float xlimit_min = 120;
    public float xlimit_max = 830;
    public float zlimit_min = 130;
    public float zlimit_max = 820;
    public float y_height = 0.9f;

    public uint blockNum = 80;
    public float refreshTime = 3.0f;
    public float blockMass = 100.0f;//雪块质量

    public static CreateSnowBlock Instance;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CreateSnowFood());
    }

    IEnumerator CreateSnowFood()
    {
        uint _num = 0;
        while (_num < blockNum)
        {
            CreateASnowBlock();
            ++_num;
        }

        yield return 0;
    }

    public void CreateASnowBlock()
    {
        float x = Random.Range(xlimit_min, xlimit_max);
        float z = Random.Range(zlimit_min, zlimit_max);
        GameObject food = Instantiate(SnowBlockPrefab, new Vector3(x, y_height, z), Quaternion.identity) as GameObject;
    }

    public void RefreshSnowBlock()
    {
        Invoke("CreateASnowBlock", refreshTime);
    }
}
