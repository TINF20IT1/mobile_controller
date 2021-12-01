using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

public class ButtonMessage : NetworkMessage
{

    public string id;

    public bool pressed;
    public bool trigger;
    public string key;

    public int sendtime;

    public ButtonMessage(){}

    public static ButtonMessage generate(string key_, bool trigger_ = true, bool pressed_ = true)
    {
        ButtonMessage bm = new ButtonMessage();

        bm.id = SystemInfo.deviceUniqueIdentifier;
        bm.sendtime = System.DateTime.Now.Millisecond;
        bm.trigger = trigger_;
        bm.pressed = pressed_;
        bm.key = key_;

        return bm;
    }
    
    public byte[] serialize()
    {
        List<byte> bytes = new List<byte>();

        bytes.Add((byte)(id.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(id));
        bytes.AddRange(BitConverter.GetBytes(sendtime));
        bytes.Add((byte)(key.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(key));
        bytes.AddRange(BitConverter.GetBytes((char)(((trigger)?2:0) + ((pressed)?1:0))));
        
        return bytes.ToArray();
    }

    public static ButtonMessage deserialize(byte[] data)
    {
        ButtonMessage bm = new ButtonMessage();
        int pos = 0;
        int stringlength = data[pos];
        pos++;

        byte[] stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        bm.id = System.Text.Encoding.ASCII.GetString(stringdata);
        pos += stringlength;


        bm.sendtime = BitConverter.ToInt32(data,pos);
        pos += 4;

        stringlength = data[pos];
        pos++;

        stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        bm.key = System.Text.Encoding.ASCII.GetString(stringdata);
        pos += stringlength;

        byte lastdata = data[pos];
        bm.pressed = (lastdata % 2 == 1);
        bm.trigger = (lastdata > 1);

        return bm;

    }

    public static void process(byte[] data, string ip)
    {
        ButtonDataframe bd = new ButtonDataframe(ButtonMessage.deserialize(data));
        PlayerMessageManager.handleNewButtonframe(bd);
    }
}