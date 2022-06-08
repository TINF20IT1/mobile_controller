using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

public class UsernameMessage : NetworkMessage
{

    public string id;
    public string name;

    public UsernameMessage(){}

    public static UsernameMessage generate(string name)
    {
        UsernameMessage um = new UsernameMessage();

        um.id = SystemInfo.deviceUniqueIdentifier;
        um.name = name;

        return um;
    }
    
    public byte[] serialize()
    {
        List<byte> bytes = new List<byte>();

        bytes.Add((byte)(id.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(id));

        bytes.Add((byte)(name.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(name));

        
        return bytes.ToArray();
    }

    public static UsernameMessage deserialize(byte[] data)
    {
        UsernameMessage um = new UsernameMessage();
        int pos = 0;

        int stringlength = data[pos];
        pos++;

        byte[] stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        um.id = System.Text.Encoding.ASCII.GetString(stringdata);
        pos += stringlength;

        stringlength = data[pos];
        pos++;

        stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        um.name = System.Text.Encoding.ASCII.GetString(stringdata);

        return um;

    }

    public static void process(byte[] data, string ip)
    {
        Debug.Log("Username processing obsolete");
        //UsernameMessage um = UsernameMessage.deserialize(data);
        //PlayerMessageManager.handleNewPlayerSelection(um);
    }
}