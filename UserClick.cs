using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mydirector;
using Mycontroller;
public class UserClick : MonoBehaviour {
	
	private UserAction action;
	Mycontroller.CharacterController character_ctrl;
	public void setController(Mycontroller.CharacterController character){
		character_ctrl = character;
	}
		
	void Start () {
		action = Director.getInstace().current as UserAction;
	}

	void OnMouseDown(){
		if (gameObject.name == "boat") {
			action.moveBoat ();
		} else {
			action.moveCharacter (character_ctrl);
		}
	}
}
