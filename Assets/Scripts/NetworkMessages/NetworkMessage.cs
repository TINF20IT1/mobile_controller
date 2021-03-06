using System;
using System.Collections.Generic;
using UnityEngine;
public class NetworkMessage
{
    //public char messageID;
    //public string message;

    public enum MessageDestination : byte
    {
        Debug = 0,
        Rotation = 1,
        DebugIP = 2,
        Button = 3,
        Selection = 5
    }

    public static string serveradress = null;
    public static Dictionary<byte, Action<byte[],string>> functions = new Dictionary<byte, Action<byte[],string>>()
    {
        {0, displayMessage},
        {1, OrientationMessage.process},
        {2, getServer},
        {3, ButtonMessage.process},
        {4, UsernameMessage.process},
        {5, PlayerSelection.process},
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