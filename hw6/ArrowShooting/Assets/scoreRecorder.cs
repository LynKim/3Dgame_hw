using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreRecorder : MonoBehaviour {

	public int score;
	void Start(){score = 0;}
	public void Record(int point){score = point + score;}
}