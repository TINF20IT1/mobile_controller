using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player;


    public void Update()
    {
        foreach(PlayerInformation pi in PlayerMessageManager.getPlayers())
        {
            if(transform.Find(pi.userid) == null)
            {
                InstantiatePlayer(pi);
            }
        }
    }
    public void InstantiatePlayer(PlayerInformation pi)
    {
        Debug.Log("InstantiatePlayer");
        Debug.Log(pi.name);
        Debug.Log(gameObject);

        GameObject go = Instantiate(player,Vector3.zero,Quaternion.identity, transform);
        go.name = pi.userid;

        go.GetComponent<Player>().setInfo(pi);
    }
}
