using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Color player_color;
    public string id;

    [SerializeField]
    float movement_speed = 1f;

    Rigidbody rigid;

    void Start()
    {  
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //left right
        if(PlayerMessageManager.buttonHeld(id, "R")){
            transform.position += Vector3.right * movement_speed;
        } else if(PlayerMessageManager.buttonHeld(id, "L")){
            transform.position += Vector3.left * movement_speed;
        }

        if(PlayerMessageManager.buttonHeld(id, "U") || Input.GetKey(KeyCode.Space)){
            rigid.AddForce(Vector3.up, ForceMode.Impulse);
        }
        
        
    }
}
