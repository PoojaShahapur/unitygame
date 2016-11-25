using UnityEngine;
using System.Collections;
using Assets.BEP.Vol._2.Scripts;

public class OptionEvent : MonoBehaviour {   
    public void OnClickOpBtn()
    {
        float curRadius = CreatePlayer._Instace.player.GetComponent<Transform>().localScale.x;
        curRadius /= 2;//分裂为两个
        CreatePlayer._Instace.player.GetComponent<Transform>().localScale = new Vector3(curRadius, curRadius, curRadius);

        Vector3 curPos = CreatePlayer._Instace.player.GetComponent<Transform>().position;
        Quaternion curRotation = CreatePlayer._Instace.player.GetComponent<Transform>().rotation;

        float dis = -1.0f;
        float radius = CreatePlayer._Instace.player.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * curRadius;
        GameObject player = Instantiate(CreatePlayer._Instace.PlayrPrefab, new Vector3(curPos.x + 2 * radius + dis, curPos.y / 2, curPos.z), Quaternion.identity) as GameObject;
        player.GetComponent<Food>().SetIsRobot(false);
        player.name = "复制体";//CreatePlayer._Instace.player.name;
        player.GetComponent<Food>().entity.m_charid = 0;//自己的charid为0
        player.GetComponent<Food>().entity.m_canEatRate = player.GetComponent<Food>().canEatRate;
        player.GetComponent<Food>().setMyName(player.name);
        player.GetComponent<Food>().setEntity(player);
        player.GetComponent<Transform>().localScale = new Vector3(curRadius, curRadius, curRadius);
        player.GetComponent<Transform>().rotation = curRotation;

        ChildrenItemInfo playerInfo = new ChildrenItemInfo(dis, 0, player);
        CreatePlayer._Instace.childrenList.Add(playerInfo);

        Debug.Log(curPos + "  " + player.GetComponent<Transform>().position + "   " + radius);
    }
}
