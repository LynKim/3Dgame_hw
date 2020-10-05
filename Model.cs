using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;
 
public class Model : MonoBehaviour {
 
    Stack<GameObject> start_priests = new Stack<GameObject>();
    Stack<GameObject> end_priests = new Stack<GameObject>();
    Stack<GameObject> start_devils = new Stack<GameObject>();
    Stack<GameObject> end_devils = new Stack<GameObject>();
 
    GameObject[] ship = new GameObject[2];
    GameObject ship_obj;
    public float speed = 100f;
 
    SSDirector one;
 
    //以上的向量都是游戏对象的初始位置，以后移动后的位置
    Vector3 shipStartPos = new Vector3(-2f, -1f, 0);
    Vector3 shipEndPos = new Vector3(6f, -1f, 0);
    Vector3 bankStartPos = new Vector3(-10f, -5f, 0);
    Vector3 bankEndPos = new Vector3(14f, -5f, 0);
 
    float gap = 1.5f;
    Vector3 priestsStartPos = new Vector3(-10.5f, 0.5f, 0);
    Vector3 priestsEndPos = new Vector3(13.5f, 0.5f, 0);
    Vector3 devilsStartPos = new Vector3(-6f, 0.5f, 0);
    Vector3 devilsEndPos = new Vector3(18f, 0.5f, 0);
    
 
    // Use this for initialization
    void Start () {
		one = SSDirector.GetInstance();
        one.setModel(this);
        loadSrc();
	}
	
	// Update is called once per frame
	void Update () {
        setposition(start_priests, priestsStartPos);
        setposition(end_priests, priestsEndPos);
        setposition(start_devils, devilsStartPos);
        setposition(end_devils, devilsEndPos);
 
        if(one.state == State.SE_move)
        {
            ship_obj.transform.position = Vector3.MoveTowards(ship_obj.transform.position, shipEndPos, Time.deltaTime * speed);
            if(ship_obj.transform.position == shipEndPos)
            {
                one.state = State.End;
            }
        }
        else if(one.state == State.ES_move)
        {
            ship_obj.transform.position = Vector3.MoveTowards(ship_obj.transform.position, shipStartPos, Time.deltaTime * speed);
            if(ship_obj.transform.position == shipStartPos)
            {
                one.state = State.Start;
            }
        }
        else
        {
            check();
        }
	}
 
    //加载游戏对象
    void loadSrc()
    {   
        //bank
        Instantiate(Resources.Load("Prefabs/bank"), bankStartPos, Quaternion.identity);
        Instantiate(Resources.Load("Prefabs/bank"), bankEndPos, Quaternion.identity);
 
        //ship
        ship_obj = Instantiate(Resources.Load("Prefabs/ship"), shipStartPos, Quaternion.identity) as GameObject;
        
        //prisets and devils
        for(int i = 0; i < 3; i++)
        {
            start_priests.Push(Instantiate(Resources.Load("Prefabs/Priest")) as GameObject);
            start_devils.Push(Instantiate(Resources.Load("Prefabs/Devil")) as GameObject);
        }
    }
 
    void setposition(Stack<GameObject> aaa, Vector3 pos)
    {
        GameObject[] temp = aaa.ToArray();
        for(int i = 0; i < aaa.Count; i++)
        {
            temp[i].transform.position = pos + new Vector3(-gap * i, 0, 0);
        }
    }
 
    //上船的操作
    void getOnTheShip(GameObject obj)
    {
        obj.transform.parent = ship_obj.transform;
        if(shipNum() != 0)
        {
            if(ship[0] == null)
            {
                ship[0] = obj;
                obj.transform.localPosition = new Vector3(-0.4f, 1, 0);
            }
            else
            {
                ship[1] = obj;
                obj.transform.localPosition = new Vector3(0.4f, 1, 0);
            }
        }
    }
    //判断船上是否有空位，以及上船的对象将坐再哪个位置上
    int shipNum()
    {
        int num = 0;
        for(int i = 0; i < 2; i++)
        {
            if(ship[i] == null)
            {
                num++;
            }
        }
        return num;
    }
 
    //船移动时的操作
    public void moveShip()
    {
        if(shipNum() != 2)
        {
            if(one.state == State.Start)
            {
                one.state = State.SE_move;
            }
            else if(one.state == State.End)
            {
                one.state = State.ES_move;
            }
        }
    }
 
    //下船的操作
    public void getOffTheShip(int side)
    {
        if(ship[side] != null)
        {
            ship[side].transform.parent = null;
            if(one.state == State.Start)
            {
                if(ship[side].tag == "Priest")
                {
                    start_priests.Push(ship[side]);
                }
                else
                {
                    start_devils.Push(ship[side]);
                }
            }
            else if(one.state == State.End)
            {
                if(ship[side].tag == "Priest")
                {
                    end_priests.Push(ship[side]);
                }
                else
                {
                    end_devils.Push(ship[side]);
                }
            }
            ship[side] = null;
        }
    }
 
    void check()
    {   
        if(end_devils.Count == 3 && end_priests.Count == 3)
        {
            one.state = State.Win;
            return;
        }
 
        int bp = 0, bd = 0;
        for(int i = 0; i < 2; i++)
        {
            if(ship[i] != null && ship[i].tag == "Priest")
            {
                bp++;
            }
            else if(ship[i] != null && ship[i].tag == "Devil")
            {
                bd++;
            }
        }
 
        int sp = 0, sd = 0, ep = 0, ed = 0;
        if(one.state == State.Start)
        {
            sp = start_priests.Count + bp;
            ep = end_priests.Count;
            sd = start_devils.Count + bd;
            ed = end_devils.Count;
        }
        else if(one.state == State.End)
        {
            sp = start_priests.Count;
            ep = end_priests.Count + bp;
            sd = start_devils.Count;
            ed = end_devils.Count + bd;
        }
 
        if((sp != 0 && sp < sd) || (ep != 0 && ep < ed))
        {
            one.state = State.Lose;
        }
    }
 
    //定义游戏对象从岸上到船上的变化
    public void priS()
    {
        if(start_priests.Count != 0 && shipNum() != 0 && one.state == State.Start)
        {
            getOnTheShip(start_priests.Pop());
        }
    }
    public void priE()
    {
        if(end_priests.Count != 0 && shipNum() != 0 && one.state == State.End)
        {
            getOnTheShip(end_priests.Pop());
        }
    }
    public void delS()
    {
        if(start_devils.Count != 0 && shipNum() != 0 && one.state == State.Start)
        {
            getOnTheShip(start_devils.Pop());
        }
    }
    public void delE()
    {
        if(end_devils.Count != 0 && shipNum() != 0 && one.state == State.End)
        {
            getOnTheShip(end_devils.Pop());
        }
    }
 
    //重置游戏
    public void Reset_all()
    {
        ship_obj.transform.position = shipStartPos;
 
        int num1 = end_devils.Count, num2 = end_priests.Count;
        for(int i = 0; i < num1; i++)
        {
            Debug.Log(i);
            start_devils.Push(end_devils.Pop());
        }
 
        for (int i = 0; i < num2; i++)
        {
            start_priests.Push(end_priests.Pop());
        }
 
        
 
        getOffTheShip(0);
        getOffTheShip(1);
 
    }
}

