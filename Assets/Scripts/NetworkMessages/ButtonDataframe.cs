using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ButtonDataframe
{

    public ButtonMessage buttonMessage;
    public int recievetime;
    public ButtonDataframe(ButtonMessage bm)
    {
        buttonMessage = bm;
        recievetime = System.DateTime.Now.Millisecond;
    }
}