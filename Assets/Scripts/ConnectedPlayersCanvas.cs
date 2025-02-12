﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class ConnectedPlayersCanvas : MonoBehaviour {




    public InputField player1;
    public InputField player2;
    public InputField player3;
    public InputField player4;
    public Button startBtn;

    //Set all textfields to false.
    void Start ()
    {
        player1.interactable = false;
        player2.interactable = false;
        player3.interactable = false;
        player4.interactable = false;
        startBtn.interactable = false;

    }
	
    //Text fields become usable if controller is connected.
	// Update is called once per frame
	void Update ()
    {
        if(XCI.GetNumPluggedCtrlrs()>0)
        {
            player1.interactable = true;
            startBtn.interactable = true;
        }
        else
        {
            player1.interactable = false;
            startBtn.interactable = false;
        }
        if (XCI.GetNumPluggedCtrlrs() > 1)
        {
            player2.interactable = true;
        }
        else
        {
            player2.interactable = false;
        }
        if (XCI.GetNumPluggedCtrlrs() > 2)
        {
            player3.interactable = true;
        }
        else
        {
            player3.interactable = false;
        }
        if (XCI.GetNumPluggedCtrlrs() > 3)
        {
            player4.interactable = true;
        }
        else
        {
            player4.interactable = false;
        }

    }
}
