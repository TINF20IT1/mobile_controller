using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputVisualizer : MonoBehaviour
{
    public GameObject mobile_model;

    public double latenz;
   
    private int pos = 0;

    public List<GameObject> activeButtons = new List<GameObject>();
    void Update()
    {
        if(PlayerMessageManager.rotationList != null)
        {
            foreach (KeyValuePair<string, Queue<OrientationDataframe>> entry in PlayerMessageManager.rotationList)
            {
                if(entry.Key == null) continue;
                if(!GameObject.Find(entry.Key))
                {
                    GameObject go = Instantiate(mobile_model,Vector3.back * pos * 0.25f,Quaternion.identity,transform);
                    go.name = entry.Key;
                    pos++;
                }
                GameObject g = GameObject.Find(entry.Key);

                if(g.transform.Find("USERNAME").GetComponent<TextMesh>().text == "")
                {
                    g.transform.Find("USERNAME").GetComponent<TextMesh>().text = PlayerMessageManager.getUsername(entry.Key);
                }
                latenz = PlayerMessageManager.getLatenz(entry.Key);
                g.GetComponent<Transform>().localEulerAngles = PlayerMessageManager.getRotation(entry.Key);
            }
        }
        if(PlayerMessageManager.buttonList != null)
        {
            foreach (KeyValuePair<string, List<ButtonDataframe>> entry in PlayerMessageManager.buttonList)
            {
                if(entry.Key == null) continue;
                if(!GameObject.Find(entry.Key))
                {
                    GameObject go = Instantiate(mobile_model,Vector3.back * pos * 0.25f,Quaternion.identity,transform);
                    go.name = entry.Key;
                    pos++;
                }

                if(PlayerMessageManager.buttonTriggered(entry.Key,"A"))
                    GameObject.Find(entry.Key).GetComponent<SpeechBubbleHandler>().display("A");

                if(PlayerMessageManager.buttonHeld(entry.Key,"B"))
                {
                    GameObject go = GameObject.Find(entry.Key).GetComponent<SpeechBubbleHandler>().show("B");
                    if(go != null) activeButtons.Add(go);
                }

            }
        }

        foreach(GameObject go in activeButtons.ToArray())
        {
            if(!PlayerMessageManager.buttonHeld(go.transform.parent.name,go.name))
            {
                activeButtons.Remove(go);
                GameObject.Find(go.transform.parent.name).GetComponent<SpeechBubbleHandler>().hide(go);
            }
        }

    }

    
}
