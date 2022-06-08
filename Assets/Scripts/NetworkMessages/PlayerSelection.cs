using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

public class PlayerSelection : NetworkMessage
{

    public string id;
    public string name;

    public char r,g,b;

    public char character;
    
    public PlayerSelection(){}
    public byte[] serialize()
    {
        List<byte> bytes = new List<byte>();

        bytes.Add((byte)(id.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(id));


        bytes.Add((byte)(name.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(name));

        bytes.Add((byte)(character));
        bytes.Add((byte)(r));
        bytes.Add((byte)(g));
        bytes.Add((byte)(b));


        
        return bytes.ToArray();
    }

    public static PlayerSelection deserialize(byte[] data)
    {
        Debug.Log("1");
        PlayerSelection um = new PlayerSelection();
        Debug.Log("2-");

        int pos = 0;

        int stringlength = data[pos];
        pos++;
                Debug.Log("2");


        byte[] stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        um.id = System.Text.Encoding.ASCII.GetString(stringdata);
        pos += stringlength;

        Debug.Log("3");

        stringlength = data[pos];
        pos++;
                Debug.Log("4");


        stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        um.name = System.Text.Encoding.ASCII.GetString(stringdata);

        Debug.Log("5");

        return um;

    }

    public static void process(byte[] data, string ip)
    {
        PlayerSelection ps = PlayerSelection.deserialize(data);
        PlayerMessageManager.handleNewPlayerSelection(ps);
    }
}