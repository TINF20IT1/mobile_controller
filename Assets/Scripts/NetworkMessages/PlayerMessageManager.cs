using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerMessageManager
{
    public static Dictionary<string, PlayerInformation> players = new Dictionary<string, PlayerInformation>();

    public static void handleNewPlayerSelection(PlayerSelection od)
    {
        if(!players.ContainsKey(od.id))
            players[od.id] = new PlayerInformation(od.id);


        players[od.id].name = od.name;
        players[od.id].color = new Color(
            (float)(od.r) / (255),
            (float)(od.g) / (255),
            (float)(od.b) / (255));

        players[od.id].character = od.character;

        Debug.Log(od.id);
        Debug.Log(od.name);
        Debug.Log(players[od.id].color);
        Debug.Log(players[od.id].character);

        Debug.Log(getUsernames().Length);


    }

    public static void handleNewDataframe(OrientationDataframe od)
    {
        if(!players.ContainsKey(od.orientationMessage.id))
            players[od.orientationMessage.id] = new PlayerInformation(od.orientationMessage.id);

        players[od.orientationMessage.id].addNewOrientation(od);
    }

    public static void handleNewButtonframe(ButtonDataframe bd)
    {

        if(!players.ContainsKey(bd.buttonMessage.id))
            players[bd.buttonMessage.id] = new PlayerInformation(bd.buttonMessage.id);

        players[bd.buttonMessage.id].handleButton(bd);
    }

    public static bool buttonTriggered(string user, string key)
    {
        if(!players.ContainsKey(user)) return false;

        return players[user].buttonTriggered(key);
        
    }

    public static bool buttonHeld(string user, string key)
    {
        if(!players.ContainsKey(user)) return false;

        return players[user].buttonHeld(key);
    }


    public static Vector3 getRotation(string userID)
    {
        return Vector3.zero;
    }
    /*
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
        return new Vector3(getClosestValue(input.x,prev.x, Vector3.right),getClosestValue(input.y,prev.y, Vector3.up),getClosestValue(input.z,prev.z, Vector3.forward));
    }
    public static float getClosestValue(float input, float prev, Vector3 axis)
    {
        float[] distances = {Mathf.Abs(input - prev), Mathf.Abs(input + 360 - prev),Mathf.Abs(input - 360 - prev)};

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
            if(latenz < 0) continue;
            count++;
            sum += latenz;
        }

        return sum / count;
    }

    


    public static OrientationDataframe[] getData(string userID)
    {
        return rotationList[userID].ToArray();
    }

    */

    public static PlayerInformation[] getPlayers()
    {
        List<PlayerInformation> p2 = new List<PlayerInformation>();

        foreach(PlayerInformation p in players.Values)
        {
            p2.Add(p);
        }

        return p2.ToArray();
    }
    public static string[] getUsers()
    {
        return players.Keys.ToArray();
    }

    public static string[] getUsernames()
    {

        List<string> usernames = new List<string>();

        foreach(PlayerInformation p in players.Values)
        {
            usernames.Add(p.name);
        }
        return usernames.ToArray();
    }

    public static string getUsername(string id)
    {
        if(players.ContainsKey(id))
            return players[id].name;
        return "";
    }
}