using System;
using System.Collections.Generic;
using UnityEngine;
public class NetworkMessage
{
    //public char messageID;
    //public string message;

    public static string serveradress = null;
    public static Dictionary<byte, Action<byte[],string>> functions = new Dictionary<byte, Action<byte[],string>>()
    {
        {0, displayMessage},
        {1, OrientationMessage.process},
        {2, getServer},
        {3, ButtonMessage.process},
    };

    public static void displayMessage(byte[] data, string ip)
    {
        
        Debug.Log(System.Text.Encoding.UTF8.GetString(data));
    }

    public static void getServer(byte[] data, string ip)
    {

        serveradress = ip.Split(':')[0];
        Debug.Log(System.Text.Encoding.UTF8.GetString(data) + "(" + serveradress+ ")");
    }

    /*public NetworkMessage(char msgID, string msg)
    {

        messageID = msgID;
        message = msg;
    }*/
}