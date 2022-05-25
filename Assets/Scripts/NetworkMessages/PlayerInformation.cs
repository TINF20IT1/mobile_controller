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

    public void handleButton(ButtonDataframe bd)
    {
        if(bd.buttonMessage.trigger)
        {
            buttons.Add(bd);
            return;
        }


        for(int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i].buttonMessage.trigger) continue;

            bool alreadyPressed = false;
            if(buttons[i].buttonMessage.key == bd.buttonMessage.key)
            {
                alreadyPressed = true;
                if(!bd.buttonMessage.pressed)
                    buttons.RemoveAt(i);
                return;
            }

            if(!alreadyPressed && bd.buttonMessage.pressed)
            {
                buttons.Add(bd);
                return;
            }
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
