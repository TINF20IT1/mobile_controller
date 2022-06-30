using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{   
    public string playerName, uniqueDeviceID;
    void Update()
    {
        playerName = PlayerMessageManager.getUsername(uniqueDeviceID);

        if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"L"))
            transform.Translate(Vector2.left);

        if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"R"))
            transform.Translate(Vector2.right);

        if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"U"))
            transform.Translate(Vector2.up);

        if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"D"))
            transform.Translate(Vector2.down);

        //if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"I"))
            //use Item
    }
}
