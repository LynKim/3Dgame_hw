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
