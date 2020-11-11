# Homework 6

### 编程实践——打飞碟（Hit UFO）游戏运动与物理兼容版

在上一次实践的基础上，增加了物理运动，做到了运动学和物理运动会的兼容。而原来的代码只实现了运动学部分。

首先需要实现物理运动部分的`Action`类和`ActionManager`类。

相比起运动学复杂的位置计算，这里只需要设定重力和给一个初速度就可以了。

```c#
public class PhyUFOFlyAction : SSAction{
    private Vector3 start_vector;                              //初速度向量
    public float power;
    private PhyUFOFlyAction() {}
    public static PhyUFOFlyAction GetSSAction(Vector3 direction, float angle, float power){
        //初始化物体将要运动的初速度向量
        PhyUFOFlyAction action = CreateInstance<PhyUFOFlyAction>();
        if (direction.x == -1) action.start_vector = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        else action.start_vector = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        action.power = power;
        return action;
    }

    public override void Update() { 
        if (this.transform.position.y < -10){
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }
    public override void Start(){
        //使用重力以及给一个初速度
        gameobject.GetComponent<Rigidbody>().velocity = power / 13 * start_vector;
        gameobject.GetComponent<Rigidbody>().useGravity = true;
    }
}
```

`ActionManager`类与之前运动学实现的`ActionManager`基本一致。

```c#
public class PhyActionManager : SSActionManager{
    public PhyUFOFlyAction fly;
    protected void Start(){  
    }
    //飞碟飞行
    public void UFOFly(GameObject disk, float angle, float power){
        fly = PhyUFOFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }

}
```

用Adapter模式写一个适配器，使其能选择运动学和物理运动来实现飞碟的飞行。

```c#
public class Adapter : MonoBehaviour,AdaActionmanager {
    public FlyActionManager fly_action_manager;
    public PhyActionManager phy_action_manager;
    public void UFOFly(GameObject disk,float angle,float power,bool op){
        if (op) phy_action_manager.UFOFly(disk,angle,power);
        else fly_action_manager.UFOFly(disk,angle,power);
    }
    void Start(){
        fly_action_manager=gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        phy_action_manager=gameObject.AddComponent<PhyActionManager>() as PhyActionManager;
    }
}

```

在上述代码中，当`op=true`时选择物理运动实现，`op=false`时选择运动学实现。

最后我们只要将`FirstController`中所有调用运动学的部分改成调用这个`Adapter`就可以了。



因为加上了刚体，飞碟会发生碰撞，飞碟飞行时容易发生变轨，对于飞碟的轨道预测有了更大难度，也就是说，这个游戏更难了。。



### 打靶游戏

规则：按空格键取箭，然后按住鼠标左键同时移动鼠标，箭头会跟着鼠标移动，此时松开左键，箭就会射出。射中后箭体会进行抖动。
2.靶上一共有5环，击中n环加n分，即5环加50分。
3.左上角会提示当前风力方向和强度，会影响箭的飞行轨迹。

首先是预制体靶和箭的设置。
靶上面要有5环，因此我在一个空对象Target下面创建了5个Cylinder子对象，各自带上Mesh Collider网格碰撞器，并且对Mesh Collider的Convex选项打勾，即为凸的网格，这样才能跟其他碰撞器产生碰撞作用。

对于箭的设置来说，首先在一个空对象Arrow下创建一个柱体Cylinder和正方体Cube构成箭身和箭头。给空对象Arrow加上刚体Rigidbody并且勾选Is Kinematic，即开始时候为运动学刚体；给箭身加碰撞器，箭头也加上加碰撞器并且勾选Is Trigger，同时挂载检测碰撞的脚本。

然后是动作管理的模块，总体来说与UFO动作管理类似，所以先把大的框架搬过来，然后对于箭飞行的部分ArrowFlyAction进行实现。因为在鼠标的拖动下实现箭的射击，所以得到鼠标的方向后将方向乘30作为射箭的力的大小进行输入，然后调用AddForce函数将箭射出。

```
public class ArrowFlyActionManager : SSActionManager
{

	private ArrowFlyAction fly; 
	public controllor scene_controller; 
	protected void Start()
	{
		scene_controller = (controllor)SSDirector.GetInstance().CurrentScenceController;
		scene_controller.action_manager = this;
	}
	public void ArrowFly(GameObject arrow,Vector3 wind,Vector3 f){
		fly = ArrowFlyAction.GetSSAction(wind, f);
		this.RunAction(arrow, fly, this);
	}
}
public class ArrowFlyAction : SSAction
{
	public Vector3 force;
	public Vector3 wind;
	private ArrowFlyAction() { }
	public static ArrowFlyAction GetSSAction(Vector3 wind,Vector3 force_)
	{
		ArrowFlyAction action = CreateInstance<ArrowFlyAction>();
		action.force = force_;
		action.wind = wind;
		return action;
	}

	public override void Update(){}
	public override void FixedUpdate()
	{
		this.gameobject.GetComponent<Rigidbody>().AddForce(wind, ForceMode.Force);
		if (this.transform.position.z > 30 || this.gameobject.tag == "hit"){
			this.destroy = true;
			this.callback.SSActionEvent(this,this.gameobject);
		}
	}
	public override void Start(){
		gameobject.transform.LookAt(force);
		gameobject.GetComponent<Rigidbody>().isKinematic = false;
		gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		gameobject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		gameobject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
	}
}

```
接着是场景控制器的设计。
场景的控制器继承了IUserAction接口，并实现其方法。箭由箭工厂产生，通过create函数调用工厂方法。在Update()方法让箭工厂检测回收掉地的箭。在shoot函数中为箭的发射添加一个冲力Impulse。为了使箭头也朝着飞出方向，所以调用transform.LookAt方法改变朝向箭飞出时获取当前风力方向和强度，然后调用动作管理类的函数给箭加一个力，使其飞行。

```
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


```

接着实现箭的工厂类，实现了制造箭和回收箭。
同样，设置一个usingList和unusedList，进行箭的循环利用。由场景控制器中的Update()方法让箭工厂每帧检测回收掉地的箭。箭掉地后，复位。
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowFactory : MonoBehaviour {

	public GameObject arrow = null;
	private List<GameObject> usingArrowList = new List<GameObject>();
	private Queue<GameObject> unusedArrowList = new Queue<GameObject>();
	public controllor sceneControler;

	public GameObject GetArrow()
	{
		if (unusedArrowList.Count == 0) arrow = Instantiate(Resources.Load<GameObject>("Prefabs/arrow"));
		else
		{
			arrow = unusedArrowList.Dequeue();
			arrow.gameObject.SetActive(true);
		}
		arrow.transform.position = new Vector3 (0, 0, 0);
		usingArrowList.Add(arrow);
		return arrow;
	}
	public void FreeArrow()
	{
		for (int i = 0; i < usingArrowList.Count; i++) {
			if (usingArrowList[i].transform.position.y <= -8||usingArrowList[i].transform.position.y >= 8) {
				usingArrowList[i].GetComponent<Rigidbody>().isKinematic = true;
				usingArrowList[i].SetActive(false);
				usingArrowList[i].transform.position =  new Vector3 (0, 0, 0);
				unusedArrowList.Enqueue(usingArrowList[i]);
				usingArrowList.Remove(usingArrowList[i]);
				i--;


			}
		}
	}
}


```

然后就是最重要的碰撞检测类
此类挂载在箭头上。由于上面设置了箭头Is Trigger为true，即不会产生碰撞效果，所以永远不会触发OnCollisionEnter方法。因此，需要将碰撞进入方法调用void OnTriggerEnter。为了防止多次触发OnTriggerEnter方法，需要设置箭头为inactive。由于触碰后整个箭组合会变为运动学刚体而静止，所以箭身会插在靶上。最后根据射中的位置与中心点距离确定环数。
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class arrowCollider : MonoBehaviour
{
	public controllor scene_controller;
	public scoreRecorder recorder;                   

	void Start()
	{
		scene_controller = SSDirector.GetInstance().CurrentScenceController as controllor;
		recorder = singleton<scoreRecorder>.Instance;
	}

	void OnTriggerEnter(Collider c)
	{ 
		if (c.gameObject.name == "T1"||c.gameObject.name == "T2"||c.gameObject.name == "T3"||c.gameObject.name == "T4"||c.gameObject.name == "T5") {
			gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.SetActive(false);
			float point = Mathf.Sqrt (this.gameObject.transform.position.x * this.gameObject.transform.position.x + this.gameObject.transform.position.y * this.gameObject.transform.position.y);
			recorder.Record(5-(int)Mathf.Floor(point*2));
		}

	}
}

```

然后是箭中靶后的抖动部分
箭中靶后，通过回调函数告诉动作管理器，去执行箭颤抖动作。箭颤抖是通过短时间内上下快速移动实现的。

```
public class ArrowTremble : SSAction
{
	float radian = 0;
	float per_radian = 3f;
	float radius = 0.01f;
	Vector3 old_pos;
	public float left_time = 0.8f;

	private ArrowTremble() { }
	public override void Start(){old_pos = transform.position;}
	public static ArrowTremble GetSSAction(){
		ArrowTremble action = CreateInstance<ArrowTremble>();
		return action;
	}
	public override void Update(){
		left_time -= Time.deltaTime;
		if (left_time <= 0){
			transform.position = old_pos;
			this.destroy = true;
			this.callback.SSActionEvent(this);
		}
		radian += per_radian;
		float dy = Mathf.Cos(radian) * radius; 
		transform.position = old_pos + new Vector3(0, dy, 0);
	}
	public override void FixedUpdate(){}
}


```

接下来是GUI部分。
UserGUI中需要每帧读取鼠标移动的位置来传给场景控制器进行箭的移动或者发射。也需要检测是否正确按下空格取箭。其他的UI部分与上次类似。
```
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

```
然后是接口类，声明了需要在场景控制类中实现的函数。
```
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
```	