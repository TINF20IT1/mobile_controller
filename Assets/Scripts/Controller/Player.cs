using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    PlayerInformation info;

    [SerializeField]
    float movement_speed = 1f;

    Rigidbody rigid;


    //TODO: Spawn Script Script
    //TODO: Color Management
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(PlayerMessageManager.getUsers().Length > 0)
            info = PlayerMessageManager.getPlayers()[0];
    }

    void FixedUpdate()
    {
        if(info == null) return;
        //left right
        if(PlayerMessageManager.buttonHeld(info.userid, "R"))
        {
            transform.position += Vector3.right * movement_speed * Time.fixedDeltaTime;
        } else if(PlayerMessageManager.buttonHeld(info.userid, "L"))
        {
            transform.position += Vector3.left * movement_speed * Time.fixedDeltaTime;
        }

        if(PlayerMessageManager.buttonHeld(info.userid, "U"))
        {
            rigid.AddForce(Vector3.up, ForceMode.Impulse);
        }

        if(PlayerMessageManager.buttonHeld(info.userid, "D"))
        {
            Debug.Log("Down");
        }

        if(PlayerMessageManager.buttonTriggered(info.userid, "I"))
        {
            Debug.Log("Item");
        }
        
        
    }
}
