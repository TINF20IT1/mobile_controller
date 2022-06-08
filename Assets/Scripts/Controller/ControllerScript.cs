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

    public Button l, r, u, d, i;

    public void SendUserSelection(PlayerSelection ps)
    {
        if(started) 
            mainsocket.Send(5, ps.serialize());
    }

    public void SendDown(string button)
    {

    }

    public void SendUpMessage(string button)
    {
        Debug.Log(button + " UP");
        if(!started) return;

        mainsocket.Send(
            (byte)NetworkMessage.MessageDestination.Button, 
            ButtonMessage.generate(button, false,false).serialize());
    }

    public void SendDownMessage(string button)
    {
        Debug.Log(button + " Down");
        if(!started) return;
        
        mainsocket.Send(
            (byte)NetworkMessage.MessageDestination.Button, 
            ButtonMessage.generate(button,false,true).serialize());
    }

    public void Start()
    {
        StartCoroutine(connect());

        i.onClick.AddListener(() => {if(started) 
            mainsocket.Send((byte)NetworkMessage.MessageDestination.Button, 
            ButtonMessage.generate("I").serialize());});
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