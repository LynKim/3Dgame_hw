using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
	private IUserAction action;
	GUIStyle style = new GUIStyle();
	GUIStyle style2 = new GUIStyle();
	private bool game_start = false;       
	void Start ()
	{
		action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
		style.normal.textColor = new Color(0, 0, 0, 1);
		style.fontSize = 16;
		style2.normal.textColor = new Color(1, 1, 1);
		style2.fontSize = 25;
	}
	void Update()
	{
		if(game_start)
		{
			if (action.haveArrowOnPort ()) {
				if (Input.GetMouseButton(0)) {
					Vector3 mousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
					action.MoveBow(mousePos);
				}

				if (Input.GetMouseButtonUp(0)) {
					Vector3 mousePos = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
					action.Shoot(mousePos);
				}
			}
			if (Input.GetKeyDown(KeyCode.Space)) action.create();
		}
	}
	private void OnGUI()
	{
		if(game_start)
		{
				GUI.Label(new Rect(10, 5, 200, 50), "分数:", style);
				GUI.Label(new Rect(55, 5, 200, 50), action.GetScore().ToString(), style);
				GUI.Label(new Rect(Screen.width - 170, 30, 200, 50), "风向: ", style);
				GUI.Label(new Rect(Screen.width - 110, 30, 200, 50), action.GetWind(), style);
			if (GUI.Button(new Rect(Screen.width - 170, 0, 100, 30), "重新开始"))
				{
					action.Restart();
					return;
				}
		}
		else
		{
			GUI.Label(new Rect(Screen.width / 2 - 80, Screen.width / 2 - 320, 100, 100), "Arrow Shooting", style2);
			if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.width / 2 - 150, 100, 50), "游戏开始"))
			{
				game_start = true;
				action.BeginGame();
			}
		}
	}
}