using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    public int port = 12345;
    public bool started = false;
    UDPSocket mainsocket = new UDPSocket();

    public Button l, r, u, d;

    public void Start()
    {
        StartCoroutine(connect());
        l.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("L").serialize());});
        r.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("R").serialize());});
        u.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("U").serialize());});
        d.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("D").serialize());});
    }
    
    private IEnumerator connect()
    {
        UDPSocket listener = new UDPSocket();
        Debug.Log("Opening Server on " + NetworkHandler.GetDefaultBroadcast().ToString() + ":" + (port+1).ToString());
        listener.Server(NetworkHandler.GetDefaultBroadcast().ToString(), port+1);
        Debug.Log("opened Server");
        while(NetworkMessage.serveradress == null)
        {
            yield return new WaitForSeconds(1);
        } 

        Debug.Log(NetworkMessage.serveradress);

        listener.Close();

        mainsocket.Client(NetworkMessage.serveradress, port);


        started = true;        
    }
}   