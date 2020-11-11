using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
	void LoadResources();
}

public interface IUserAction
{
	void MoveBow(Vector3 mousePos);
	void Shoot (Vector3 mousePos);
	int GetScore();
	void Restart();
	string GetWind();
	void BeginGame();
	void create ();
	bool haveArrowOnPort();
}
	