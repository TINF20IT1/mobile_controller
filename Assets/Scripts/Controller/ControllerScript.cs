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

    public Button l,  r, u;

    public void Start()
    {
        StartCoroutine(connect());
        l.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("L").serialize());});
        r.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("R").serialize());});
        u.onClick.AddListener(() => {if(started) 
            mainsocket.Send(0, ButtonMessage.generate("U").serialize());});
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

    private IEnumerator connect()
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


        started = true;        
    }
}   