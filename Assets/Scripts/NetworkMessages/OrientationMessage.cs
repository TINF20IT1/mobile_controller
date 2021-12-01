using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

public class OrientationMessage : NetworkMessage
{

    public string id;
    public float rotx, roty, rotz;

    public int sendtime;

    public OrientationMessage(){}

    public static OrientationMessage generate()
    {
        OrientationMessage om = new OrientationMessage();

        om.id = SystemInfo.deviceUniqueIdentifier;
        om.rotx = -Input.acceleration.y * 90;
        om.rotz = Input.acceleration.x * 90;  
        om.roty = Input.compass.magneticHeading;

        #if UNITY_EDITOR
        om.rotx = 0.1f;
        om.roty = (Time.realtimeSinceStartup * 20) % 360;
        om.rotz = 10.5f;
        #endif

        om.sendtime = System.DateTime.Now.Millisecond;

        return om;
    }
    
    public byte[] serialize()
    {
        List<byte> bytes = new List<byte>();

        bytes.Add((byte)(id.Length));
        bytes.AddRange(Encoding.ASCII.GetBytes(id));
        bytes.AddRange(BitConverter.GetBytes(sendtime));
        bytes.AddRange(BitConverter.GetBytes(rotx));
        bytes.AddRange(BitConverter.GetBytes(roty));
        bytes.AddRange(BitConverter.GetBytes(rotz));
        
        return bytes.ToArray();
    }

    public static OrientationMessage deserialize(byte[] data)
    {
        OrientationMessage om = new OrientationMessage();
        int pos = 0;
        int stringlength = data[pos];
        pos++;

        byte[] stringdata = new ArraySegment<byte>(data,pos,stringlength).ToArray();
        om.id = System.Text.Encoding.ASCII.GetString(stringdata);
        pos += stringlength;


        om.sendtime = BitConverter.ToInt32(data,pos);
        pos += 4;

        om.rotx = BitConverter.ToSingle(data,pos);
        pos += 4;

        om.roty = BitConverter.ToSingle(data,pos);
        pos += 4;

        om.rotz = BitConverter.ToSingle(data,pos);
        pos += 4;

        return om;

    }

    public static void process(byte[] data, string ip)
    {
        OrientationDataframe od = new OrientationDataframe(OrientationMessage.deserialize(data));
        PlayerMessageManager.handleNewDataframe(od);
    }
}