using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class controllor : MonoBehaviour, IUserAction, ISceneController
{ 
	public scoreRecorder recorder;    
	public arrowFactory arrow_factory;   
	public ArrowFlyActionManager action_manager;
	public UserGUI user_gui;

	private GameObject arrow;                
	private GameObject target;  
	private bool game_start = false;
	private string wind = "";   
	private float wind_directX;     
	private float wind_directY;  
	public int GetScore(){return recorder.score;}
	public string GetWind(){return wind;}
	public void Restart(){SceneManager.LoadScene(0);}
	public void BeginGame(){game_start = true;}
	void Update (){if(game_start) arrow_factory.FreeArrow ();}
	public void LoadResources(){target = Instantiate(Resources.Load("Prefabs/target", typeof(GameObject))) as GameObject;}
	public bool haveArrowOnPort() {return (arrow != null);}
	void Start ()
	{
		SSDirector director = SSDirector.GetInstance();
		director.CurrentScenceController = this;
		arrow_factory = singleton<arrowFactory>.Instance;
		recorder = gameObject.AddComponent<scoreRecorder>() as scoreRecorder;
		user_gui = gameObject.AddComponent<UserGUI> () as UserGUI;
		action_manager = gameObject.AddComponent<ArrowFlyActionManager>() as ArrowFlyActionManager;
		LoadResources();
	}
	public void create(){
		if (arrow == null) {

			wind_directX = Random.Range(-4, 4);
			wind_directY = Random.Range(-4, 4);
			CreateWind();
			arrow = arrow_factory.GetArrow ();
		}
	}
	public void MoveBow(Vector3 mousePos)
	{
		if (!game_start){return;}
		arrow.transform.LookAt(mousePos * 30);
	}
	public void Shoot(Vector3 mousePos)
	{
		if(game_start)
		{
			Vector3 wind = new Vector3(wind_directX, wind_directY, 0);
			action_manager.ArrowFly(arrow, wind,mousePos*30);
			arrow = null;
		}
	}
	public void CreateWind()
	{
		string Horizontal = "", Vertical = "", level = "";
		if (wind_directX > 0)
		{
			Horizontal = "西";
		}
		else if (wind_directX <= 0)
		{
			Horizontal = "东";
		}
		if (wind_directY > 0)
		{
			Vertical = "南";
		}
		else if (wind_directY <= 0)
		{
			Vertical = "北";
		}
		if ((wind_directX + wind_directY) / 2 > -1 && (wind_directX + wind_directY) / 2 < 1)
		{
			level = "1 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -2 && (wind_directX + wind_directY) / 2 < 2)
		{
			level = "2 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -3 && (wind_directX + wind_directY) / 2 < 3)
		{
			level = "3 级";
		}
		else if ((wind_directX + wind_directY) / 2 > -5 && (wind_directX + wind_directY) / 2 < 5)
		{
			level = "4 级";
		}

		wind = Horizontal + Vertical + "风" + " " + level;
	}

}
