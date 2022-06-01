using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Color player_color;
    public string id;

    [SerializedField]
    float movement_speed = 1f;

    RigidBody2 rigid;

    void Start()
    {  
        rigid = GetComponent<RigidBody2>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //left right
        if(PlayerMessageManager.buttonHeld(id, "R")){
            transform.position += Vector2.right * movement_speed;
        } else if(PlayerMessageManager.buttonHeld(id, "L")){
            transform.position += Vector2.left * movement_speed;
        }

        if(PlayerMessageManager.buttonHeld(id, "U")){
            
        }
        
        
    }
}
