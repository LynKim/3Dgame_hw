using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mydirector;
public class UserInterface : MonoBehaviour {
	private UserAction action;
	public int status = 0;
	GUIStyle style;
	GUIStyle button;

	void Start () {
		action = Director.getInstace ().current as UserAction;
		style = new GUIStyle ();
		style.fontSize = 40;
		style.alignment = TextAnchor.MiddleCenter;


		button = new GUIStyle ("button");
		button.fontSize = 30;
	}
	

	void OnGUI(){
		if (status == 1) {
			GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "GameOver!", style);
			if (GUI.Button (new Rect (Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", button)) {
				status = 0;
				action.restart ();
			}
		} else if (status == 2) {
			GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Win", style);
			if (GUI.Button (new Rect (Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", button)) {
				status = 0;
				action.restart ();
			}

		}
	}
}
