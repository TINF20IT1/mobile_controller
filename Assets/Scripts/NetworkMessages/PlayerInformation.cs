using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation
{
    public string userid;
    public string name;
    public Color color;
    public int character;

    public Queue<OrientationDataframe> orientations;
    public List<ButtonDataframe> buttons;
    public static int buffsize = 5;


    public PlayerInformation(string userID)
    {
        userid = userID;
        orientations = new Queue<OrientationDataframe>();
        buttons = new List<ButtonDataframe>();
    }

    public void addNewOrientation(OrientationDataframe od)
    {
        orientations.Enqueue(od);
        if(orientations.Count > buffsize)
            orientations.Dequeue();
    }

    public void handleSelection(PlayerSelection ps)
    {
        character = ps.character;
        name = ps.name;
        color = new Color((float)(ps.r / 255),(float)(ps.g / 255),(float)(ps.b / 255),1);
    }

    public void handleButton(ButtonDataframe bd)
    {

        Debug.Log("recieved " + bd.buttonMessage.key);
        if(bd.buttonMessage.trigger)
        {
            buttons.Add(bd);
            return;
        }

        Debug.Log("no trigger" + bd.buttonMessage.key);


        bool alreadyPressed = false;
        for(int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i].buttonMessage.trigger) continue;

            if(buttons[i].buttonMessage.key == bd.buttonMessage.key)
            {
                alreadyPressed = true;
                if(!bd.buttonMessage.pressed)
                {
                    Debug.Log("removed" + bd.buttonMessage.key);

                    buttons.RemoveAt(i);
                    return;
                }
            }   
        }
        
        if(!alreadyPressed && bd.buttonMessage.pressed)
        {
            Debug.Log("added Button " + bd.buttonMessage.key);
            buttons.Add(bd);
            return;
        }
    }

    public bool buttonTriggered(string key)
    {

        for(int i = 0; i < buttons.Count; i++)
        {
            if(!buttons[i].buttonMessage.trigger) continue;

            if(buttons[i].buttonMessage.key == key)
            {
                buttons.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public bool buttonHeld(string key)
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i].buttonMessage.trigger) continue;

            if(buttons[i].buttonMessage.key == key)
            {
                return true;
            }
        }
        return false;
    }

    
}
