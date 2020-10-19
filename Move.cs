using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mydirector;
using Myaction;

public class Move :SSActionManager {

	private CCMoveToAction BoatMove;     
	private CCSequenceAction CharaMove;     

	public FirstController sceneController;

	protected new void Start()
	{
		sceneController = (FirstController)Director.getInstace ().current;
		sceneController.actionManager = this;
	}
	public void moveBoat(GameObject boat, Vector3 target, float speed)
	{
		BoatMove = CCMoveToAction.GetSSAction(target, speed);
		this.RunAction(boat, BoatMove, this);
	}

	public void moveCharacter(GameObject role, Vector3 middle_pos, Vector3 end_pos, float speed)
	{
		SSAction action1 = CCMoveToAction.GetSSAction(middle_pos, speed);
		SSAction action2 = CCMoveToAction.GetSSAction(end_pos, speed);
		CharaMove = CCSequenceAction.GetSSAction(1, 0, new List<SSAction> { action1, action2 });
		this.RunAction(role, CharaMove, this);
	}
}
