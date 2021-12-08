using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class NetworkHandler : MonoBehaviour
{
    public int port = 12345;
    public bool started = false;
    bool lastBstatus = false;
    public enum Network_Mode {Server, Client};
    UDPSocket mainsocket = new UDPSocket();

    public static string username;

    private double lastsend = 0;
    #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private Network_Mode GUI_Network_Mode = Network_Mode.Client;
    #else 
        private Network_Mode GUI_Network_Mode = Network_Mode.Server;
    #endif
    void Start()
    {
        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;    
        #endif 
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if(!started && GUI_Network_Mode == Network_Mode.Server && GUILayout.Button("Client")) GUI_Network_Mode = Network_Mode.Client;
        if(started  && GUI_Network_Mode == Network_Mode.Client || !started && GUI_Network_Mode != Network_Mode.Server) GUILayout.Box("Client");
        if(!started && GUI_Network_Mode == Network_Mode.Client && GUILayout.Button("Server")) GUI_Network_Mode = Network_Mode.Server;
        if(started  && GUI_Network_Mode == Network_Mode.Server || !started && GUI_Network_Mode != Network_Mode.Client) GUILayout.Box("Server");
        GUILayout.EndHorizontal();

        GUILayout.Box(SystemInfo.deviceUniqueIdentifier);

        if(GUI_Network_Mode == Network_Mode.Server)
        {
            GUILayout.Box(GetLocalIPAddress().ToString());
        
            if(!started && GUILayout.Button("Start Server"))
            {
                started = true;
                
                mainsocket.Server(GetLocalIPAddress().ToString(), port);
                StartCoroutine(AnounceServer());

            }
        }

        if(GUI_Network_Mode == Network_Mode.Client)
        {
            GUILayout.Box("listening on " + GetDefaultBroadcast().ToString());
            GUILayout.Box(GetLocalIPAddress() + " → " + NetworkMessage.serveradress);

            GUILayout.Box((Time.realtimeSinceStartupAsDouble - lastsend).ToString());

            if(!started && GUILayout.Button("Send Orientation\n\n\n\n\n\n\n"))
            {
                started = true;
                StartCoroutine(SendOrientation());
            }
        }
        GUILayout.EndVertical();

        if(GUI_Network_Mode == Network_Mode.Client)
        {
            if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.2f, Screen.width * 0.15f, Screen.width * 0.15f), "A"))
                mainsocket.Send(3,ButtonMessage.generate(KeyCode.A.ToString()).serialize());

            bool newBStatus = GUI.RepeatButton(new Rect(Screen.width * 0.8f, Screen.height * 0.6f, Screen.width * 0.15f, Screen.width * 0.15f), "B");

            if(lastBstatus != newBStatus)
            {
                mainsocket.Send(3,ButtonMessage.generate(KeyCode.B.ToString(),false, newBStatus).serialize());
            }

            lastBstatus = newBStatus;
        }

    }


    public static System.Net.IPAddress GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) return ip;   
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }


    public static System.Net.IPAddress GetDefaultBroadcast()
    {
        byte[] la = GetLocalIPAddress().GetAddressBytes();
        la[la.Length-1] = 255;
        return new System.Net.IPAddress(la);
    }
    public static System.Net.IPAddress GetDefaultGateway()
    {
        var gateway_address = NetworkInterface.GetAllNetworkInterfaces()
            .Where(e => e.OperationalStatus == OperationalStatus.Up)
            .SelectMany(e => e.GetIPProperties().GatewayAddresses)
            .FirstOrDefault();
        if (gateway_address == null)
        {

            byte[] la = GetLocalIPAddress().GetAddressBytes();
            la[la.Length-1] = 1;
            return new System.Net.IPAddress(la);

        } 
        return gateway_address.Address;
    }

    private IEnumerator AnounceServer()
    {
        UDPSocket c = new UDPSocket();
        c.Client(GetDefaultBroadcast().ToString(), port +1);

        while(true)
        {
            try
            {
                c.Send(2,"Hello, I'm the Server");
            }
            catch (Exception e) 
            {
                Debug.Log(e.ToString());
                try
                {
                    c.Client(GetDefaultBroadcast().ToString(), port+1);
                }
                catch (Exception e_) 
                {
                    Debug.Log(e_.ToString());
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator SendOrientation()
    {
        UDPSocket listener = new UDPSocket();
        Debug.Log("Opening Server on " + GetDefaultBroadcast().ToString() + ":" + (port+1).ToString());
        listener.Server(GetDefaultBroadcast().ToString(), port+1);
        Debug.Log("opened Server");
        while(NetworkMessage.serveradress == null)
        {
            yield return new WaitForSeconds(1);
        } 

        Debug.Log(NetworkMessage.serveradress);

        listener.Close();

        mainsocket.Client(NetworkMessage.serveradress, port);

        //DEBUG stuff
        string[] usernames = {"Peter", "Karl", "Klaus", "Gertrud", "Hanz", "Franz"};
        SendUsername(usernames[UnityEngine.Random.Range(0,usernames.Length)]);
        
        while(true)
        {
            try
            {
                mainsocket.Send(1,OrientationMessage.generate().serialize());
            }
            catch (Exception e) 
            {
                Debug.Log(e.ToString());
                try
                {
                    mainsocket.Client(NetworkMessage.serveradress, port);
                }
                catch (Exception e_) 
                {
                    Debug.Log(e_.ToString());
                }
            }

            lastsend = Time.realtimeSinceStartupAsDouble;
            yield return new WaitForSeconds(1f / 50f);
        }
    }


    void SendUsername(string username)
    {
        try
        {
            mainsocket.Send(4,UsernameMessage.generate(username).serialize());
        }
        catch (Exception e) 
        {
            Debug.Log(e.ToString());
            try
            {
                mainsocket.Client(NetworkMessage.serveradress, port);
            }
            catch (Exception e_) 
            {
                Debug.Log(e_.ToString());
            }
        }
    }
}
