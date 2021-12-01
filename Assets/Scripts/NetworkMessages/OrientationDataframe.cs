using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class OrientationDataframe
{

    public OrientationMessage orientationMessage;
    public int recievetime;
    public OrientationDataframe(OrientationMessage om)
    {
        orientationMessage = om;
        recievetime = System.DateTime.Now.Millisecond;
    }

    public string Serialize()
    {
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        MemoryStream memStr = new MemoryStream();

        try
        {
            bf.Serialize(memStr, this);
            memStr.Position = 0;

            return Convert.ToBase64String(memStr.ToArray());
        }
        finally
        {
            memStr.Close();
        }
    }

    public static OrientationDataframe Deserialize(string stream)
    {
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        byte[] byteArray = Convert.FromBase64String(stream);
        MemoryStream memStr = new MemoryStream(byteArray);

        try
        {
            return (OrientationDataframe)bf.Deserialize(memStr);
        }
        finally
        {
            memStr.Close();
        }
    }

    public Vector3 getRotation()
    {
        return new Vector3(orientationMessage.rotx,orientationMessage.roty,orientationMessage.rotz);
    }

    public override string ToString()
    {
        return orientationMessage.id + ": " + getRotation().ToString() + "\n";
    }
}
