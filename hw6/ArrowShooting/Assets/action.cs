using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject
{
	public bool enable = true;                    
	public bool destroy = false;              
	public GameObject gameobject;   
	public Transform transform;   
	public ISSActionCallback callback;

	protected SSAction() { }
	public virtual void Start(){throw new System.NotImplementedException();}
	public virtual void Update(){throw new System.NotImplementedException();}
	public virtual void FixedUpdate(){throw new System.NotImplementedException();}
}

public enum SSActionEventType : int { Started, Competeted }
public interface ISSActionCallback{void SSActionEvent(SSAction source, GameObject arrow = null);}
public class SSActionManager : MonoBehaviour, ISSActionCallback
{
	private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    
	private List<SSAction> waitingAdd = new List<SSAction>();                       
	private List<int> waitingDelete = new List<int>();                        

	protected void Update() {
		foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;                                    
		waitingAdd.Clear();
		foreach (KeyValuePair<int, SSAction> kv in actions){
			SSAction ac = kv.Value;
			if (ac.destroy) waitingDelete.Add(ac.GetInstanceID());
			else if (ac.enable) ac.Update();
		}
		foreach (int key in waitingDelete){
			SSAction ac = actions[key];
			actions.Remove(key);
			DestroyObject(ac);
		}
		waitingDelete.Clear();
	}
	protected void FixedUpdate() {
		foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
		waitingAdd.Clear();
		foreach (KeyValuePair<int, SSAction> kv in actions){
			SSAction ac = kv.Value;
			if (ac.destroy) waitingDelete.Add(ac.GetInstanceID());
			else if (ac.enable) ac.FixedUpdate();
		}
		foreach (int key in waitingDelete){
			SSAction ac = actions[key];
			actions.Remove(key);
			DestroyObject(ac);
		}
		waitingDelete.Clear();
	}
	public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
	{
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		action.callback = manager;
		waitingAdd.Add(action);
		action.Start();
	}
	public void SSActionEvent(SSAction source, GameObject arrow = null){ 
		if(arrow != null){
			ArrowTremble tremble = ArrowTremble.GetSSAction();
			this.RunAction(arrow, tremble, this);
		}else{
			controllor scene_controller = (controllor)SSDirector.GetInstance().CurrentScenceController;
		}
	}
}
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
