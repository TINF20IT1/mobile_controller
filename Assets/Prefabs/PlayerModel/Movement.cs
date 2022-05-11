using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{   

    public string playerName, uniqueDeviceID;
    public List<GameObject> activeButtons = new List<GameObject>();
    void Update()
    {
        if(PlayerMessageManager.buttonList != null)
        {
            foreach (KeyValuePair<string, List<ButtonDataframe>> entry in PlayerMessageManager.buttonList)
            {
                if(entry.Key == null) continue;
                
                if(PlayerMessageManager.buttonTriggered(uniqueDeviceID,"L"))
                {
                    //LEFT
                    transform.Translate(Vector2.left);
                }
                if(PlayerMessageManager.buttonHeld(uniqueDeviceID,"R"))
                {
                    //RIGHT
                     transform.Translate(Vector2.right);
                }
                if(PlayerMessageManager.buttonHeld(uniqueDeviceID,"U"))
                {
                    //JUMP
                    transform.Translate(Vector2.up);
                }

            }
        }
    }
}
