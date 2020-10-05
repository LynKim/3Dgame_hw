using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRound : MonoBehaviour
{
    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Moon;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;
   
    // Start is called before the first frame update
    void Start () {
        Sun.position = Vector3.zero;
        Mercury.position = new Vector3 (4, 0, 0);
        Venus.position = new Vector3 (6, 0, 0);
        Earth.position = new Vector3 (8, 0, 0);
            Moon.position = new Vector3 (10, 0, 0);
        Mars.position = new Vector3 (12, 0, 0);
        Jupiter.position = new Vector3 (16, 0, 0);
        Saturn.position = new Vector3 (20, 0, 0);
        Uranus.position = new Vector3 (24, 0, 0);
        Neptune.position = new Vector3 (28, 0, 0);
        
    }

    // Update is called once per frame
    void Update () {
        Vector3 a1 = new Vector3 (0, 9, 2);
        Vector3 a2 = new Vector3 (0, 257, 135);
        Vector3 a3 = new Vector3 (0, 45, 339);
        Vector3 a4 = new Vector3 (0, 4, 9);
        Vector3 a5 = new Vector3 (0, 8, 19);
        Vector3 a6 = new Vector3 (0, 11, 9);
        Vector3 a7 = new Vector3 (0, 6, 137);
        Vector3 a8 = new Vector3 (0, 3, 13);
        
        
        Mercury.RotateAround (Sun.position, a1, 20*Time.deltaTime);
        Mercury.Rotate (Vector3.up*50*Time.deltaTime);

        Venus.RotateAround (Sun.position, a2, 10*Time.deltaTime);
        Venus.Rotate (Vector3.up*30*Time.deltaTime);

        Earth.RotateAround (Sun.position, a3, 10*Time.deltaTime);
        Earth.Rotate (Vector3.up*30*Time.deltaTime);
        Moon.transform.RotateAround (Earth.position, Vector3.up, 359 * Time.deltaTime);

        Mars.RotateAround (Sun.position, a4, 8*Time.deltaTime);
        Mars.Rotate (Vector3.up*30*Time.deltaTime);

        Jupiter.RotateAround (Sun.position, a5, 7*Time.deltaTime);
        Jupiter.Rotate (Vector3.up*30*Time.deltaTime);

        Saturn.RotateAround (Sun.position, a6, 6*Time.deltaTime);
        Saturn.Rotate (Vector3.up*30*Time.deltaTime);

        Uranus.RotateAround (Sun.position, a7, 5*Time.deltaTime);
        Uranus.Rotate (Vector3.up*30*Time.deltaTime);

        Neptune.RotateAround (Sun.position, a8, 4*Time.deltaTime);
        Neptune.Rotate (Vector3.up*30*Time.deltaTime);

       }
}
