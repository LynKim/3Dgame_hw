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