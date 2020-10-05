using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;
 
public class View : MonoBehaviour {
 
    SSDirector one;
    UserAction action;
 
	// Use this for initialization
	void Start () {
        one = SSDirector.GetInstance();
        action = SSDirector.GetInstance() as UserAction;
	}
 
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 30;
        if (one.state == State.Win)
        {
            if(GUI.Button(new Rect(500, 200, 300, 100), "WIN\n(click here to reset)"))
            {
                action.reset();
            }
        }
        if (one.state == State.Lose)
        {
            if(GUI.Button(new Rect(500, 200, 300, 100), "LOSE\n(click here to reset)"))
            {
                action.reset();
            }
        }
 
        if(GUI.Button(new Rect(600, 80, 100, 50), "GO"))
        {
            action.moveShip();
        }
        if (GUI.Button(new Rect(500, 400, 75, 50), "OFF"))
        {
            action.offShipL();
        }
        if (GUI.Button(new Rect(700, 400, 75, 50), "OFF"))
        {
            action.offShipR();
        }
        if (GUI.Button(new Rect(220, 200, 75, 50), "ON"))
        {
            action.priestSOnB();
        }
        if (GUI.Button(new Rect(350, 200, 75, 50), "ON"))
        {
            action.devilSOnB();
        }
        if (GUI.Button(new Rect(980, 200, 75, 50), "ON"))
        {
            action.devilEOnB();
        }
        if (GUI.Button(new Rect(850, 200, 75, 50), "ON"))
        {
            action.priestEOnB();
        }
    }
}

