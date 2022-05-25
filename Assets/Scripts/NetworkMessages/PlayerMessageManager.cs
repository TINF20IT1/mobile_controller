using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerMessageManager
{
    public static Dictionary<string, string> usernames = new Dictionary<string, string>();
    public static Dictionary<string, Queue<OrientationDataframe>> rotationList = new Dictionary<string, Queue<OrientationDataframe>>();
    public static Dictionary<string, List<ButtonDataframe>> buttonList = new Dictionary<string, List<ButtonDataframe>>();
    public static int buffsize = 5;


    public static void handleNewUsername(UsernameMessage od)
    {
        usernames[od.id] = od.name;
    }

    public static void handleNewDataframe(OrientationDataframe od)
    {
        if(!rotationList.ContainsKey(od.orientationMessage.id))
        {
            rotationList[od.orientationMessage.id] = new Queue<OrientationDataframe>();
        }

        rotationList[od.orientationMessage.id].Enqueue(od);
        if(rotationList[od.orientationMessage.id].Count > buffsize)
        {
            rotationList[od.orientationMessage.id].Dequeue();
        }
    }

    public static void handleNewButtonframe(ButtonDataframe bd)
    {
        if(!buttonList.ContainsKey(bd.buttonMessage.id))
        {
            buttonList[bd.buttonMessage.id] = new List<ButtonDataframe>();
        }

        if(!bd.buttonMessage.trigger && !bd.buttonMessage.pressed)
        {
            for(int i = 0; i < buttonList[bd.buttonMessage.id].Count; i++)
            {
                // Current ButtonMessage of button with id "id" was not tiggered
                bool bool1 = !buttonList[bd.buttonMessage.id][i].buttonMessage.trigger;

                // Current ButtonMessage of button with id "id" has 
                bool bool2 = buttonList[bd.buttonMessage.id][i].buttonMessage.key == bd.buttonMessage.key;
                if(bool1 && bool2)
                {
                    buttonList[bd.buttonMessage.id].RemoveAt(i);
                }
            }
            return;
        }
        buttonList[bd.buttonMessage.id].Add(bd);

    }

    public static bool buttonTriggered(string user, string key)
    {
        if(!buttonList.ContainsKey(user)) return false;

        for(int i = 0; i < buttonList[user].Count; i++)
        {
            if(buttonList[user][i].buttonMessage.trigger && buttonList[user][i].buttonMessage.key == key)
            {
                buttonList[user].RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public static bool buttonHeld(string user, string key)
    {
        if(!buttonList.ContainsKey(user)) return false;

        for(int i = 0; i < buttonList[user].Count; i++)
        {
            if(!buttonList[user][i].buttonMessage.trigger && buttonList[user][i].buttonMessage.key == key)
            {
                return true;
            }
        }
        return false;
    }

    public static Vector3 getRotation(string userID)
    {
        float x_sin_sum = 0;
        float x_cos_sum = 0;
        float y_sin_sum = 0;
        float y_cos_sum = 0;
        float z_sin_sum = 0;
        float z_cos_sum = 0;

        foreach(OrientationDataframe od in rotationList[userID].ToArray())
        {
            Vector3 rot = od.getRotation();
            x_sin_sum += Mathf.Sin(rot.x * Mathf.Deg2Rad);
            x_cos_sum += Mathf.Cos(rot.x * Mathf.Deg2Rad);
            y_sin_sum += Mathf.Sin(rot.y * Mathf.Deg2Rad);
            y_cos_sum += Mathf.Cos(rot.y * Mathf.Deg2Rad);
            z_sin_sum += Mathf.Sin(rot.z * Mathf.Deg2Rad);
            z_cos_sum += Mathf.Cos(rot.z * Mathf.Deg2Rad);
        }

        float x_avg = Mathf.Atan2(x_sin_sum,x_cos_sum) * Mathf.Rad2Deg;
        float y_avg = Mathf.Atan2(y_sin_sum,y_cos_sum) * Mathf.Rad2Deg;
        float z_avg = Mathf.Atan2(z_sin_sum,z_cos_sum) * Mathf.Rad2Deg;

        return new Vector3(x_avg,y_avg,z_avg);
    }

    public static Vector3 turns = Vector3.zero;
    
    public static Vector3 closestVector3(Vector3 input, Vector3 prev)
    {
        return new Vector3( getClosestValue(input.x,prev.x, Vector3.right),
                            getClosestValue(input.y,prev.y, Vector3.up),
                            getClosestValue(input.z,prev.z, Vector3.forward));
    }
    public static float getClosestValue(float input, float prev, Vector3 axis)
    {
        float[] distances = {   Mathf.Abs(input - prev),
                                Mathf.Abs(input + 360 - prev),
                                Mathf.Abs(input - 360 - prev)};

        float absmin = distances.Min();
        if(absmin == distances[1]) turns += axis * 360;
        if(absmin == distances[2]) turns -= axis * 360;

        return input + Vector3.Dot(axis, turns);
    }

    public static float getLatenz(string userID)
    {
        float sum = 0;

        int count = 0;
        foreach(OrientationDataframe od in rotationList[userID].ToArray())
        {
            float latenz = (od.recievetime - od.orientationMessage.sendtime);
            if(latenz > 0) count++;
            sum += latenz;
        }

        return sum / count;
    }


    public static OrientationDataframe[] getData(string userID)
    {
        return rotationList[userID].ToArray();
    }

    public static string[] getUsers()
    {
        return rotationList.Keys.ToArray();
    }

    public static Dictionary<string, string> getUsernames()
    {
        return usernames;
    }

    public static string getUsername(string id)
    {
        if(usernames.ContainsKey(id)) return usernames[id];
        return "";
    }
}