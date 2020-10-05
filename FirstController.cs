using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController {
    void Awake() {
        Debug.Log ("load sunt...\n");
        SSDirector director = SSDirector.getInstance ();
        director.setFPS (60);
        director.currentSceneController = this;
        director.currentSceneController.LoadResources ();
    }

    public void LoadResources() {
        GameObject sunset = Instantiate<GameObject> (
                                Resources.Load <GameObject> ("Perfabs/Sun"),
                                Vector3.zero, Quaternion.identity);
        sunset.name = "sunset";
        Debug.Log ("load sunset...\n");
    }
    public void Pause(){
    }
    public void Resume(){
    }
    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}