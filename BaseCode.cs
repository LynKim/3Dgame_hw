using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Diretor : System.Object
    {
        private static Diretor _instance;
        public SceneControl sceneCtrl { get; set; }

        public static Diretor getInstance()
        {
            if (_instance == null) return _instance = new Diretor();
            else return _instance;
        }
    }

    public interface ISSActionCallback
    {
        void ActionDone(SSAction source);
    }

    public interface SceneControl
    {
        void LoadPrefabs();
    }

    public interface UserAction
    {
        void BoatMove();
        void Restart();
        void ItemClick(ItemControl itemCtrl);
    }

    public class ItemControl
    {
        public GameObject item { get; set; } // the instance
        public int itemType { get; set; } // the type of the item
        public ClickGUI clickGUI; // manage click event
        public bool isOnBoat;
        public ShoreControl shoreCtrl;

        public ItemControl(string type) //Instantiate
        {
            if (type == "Priest")
            {
                item = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                itemType = 0;
            }
            else
            {
                item = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                itemType = 1;
            }

            clickGUI = item.AddComponent(typeof(ClickGUI)) as ClickGUI; // add click event
            clickGUI.itemCtrl = this;
        }
        
        public void GetOnBoat(BoatControl boatCtrl)
        {
            shoreCtrl = null;
            item.transform.parent = boatCtrl.boat.transform;
            isOnBoat = true;
        }
        public void GetOnShore(ShoreControl side)
        {
            shoreCtrl = side;
            item.transform.parent = null;
            isOnBoat = false;
        }
        public void Reset()
        {
            ShoreControl fromShore = ((Controller)Diretor.getInstance().sceneCtrl).fromShore;
            GetOnShore(fromShore);
            item.transform.position = fromShore.GetEmptyPosition();
            fromShore.GetOnShore(this);
        }
    }

    

    public class ShoreControl
    {
        public GameObject Shore;
        public Vector3 from = new Vector3(18, 2, 0);
        public Vector3 to = new Vector3(-18, 2, 0);
        public Vector3[] positions;
        public int status;
        ItemControl[] itemCtrls;

        public ShoreControl(string type)
        {
            positions = new Vector3[] {new Vector3(13, 5, 0), new Vector3(15, 5, 0), new Vector3(17, 5, 0),
                new Vector3(19, 5, 0), new Vector3(21, 5, 0), new Vector3(23, 5, 0)};

            itemCtrls = new ItemControl[6];

            if (type == "From")
            {
                Shore = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Shore", typeof(GameObject)), from, Quaternion.identity, null);
                Shore.name = "From";
                status = 1;
            }
            else
            {
                Shore = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Shore", typeof(GameObject)), to, Quaternion.identity, null);
                Shore.name = "To";
                status = -1;
            }
        }
        public int GetEmptyIndex()
        {
            for (int i = 0; i < itemCtrls.Length; i++)
                if (itemCtrls[i] == null) return i;
            return -1;
        }
        public Vector3 GetEmptyPosition()
        {
            Vector3 pos = positions[GetEmptyIndex()];
            pos.x *= status;
            return pos;
        }
        public void GetOnShore(ItemControl item)
        {
            int index = GetEmptyIndex();
            itemCtrls[index] = item;
        }
        public ItemControl GetOffShore(string name)
        {
            for (int i = 0; i < itemCtrls.Length; i++)
            {
                if (itemCtrls[i] != null && itemCtrls[i].item.name == name)
                {
                    ItemControl temp = itemCtrls[i];
                    itemCtrls[i] = null;
                    return temp;
                }
            }
            return null;
        }
        public int GetItemNum(int type)
        {
            int count = 0;
            for (int i = 0; i < itemCtrls.Length; i++)
                if (itemCtrls[i] != null && itemCtrls[i].itemType == type)
                    count++;
            return count;
        }
        public void Reset()
        {
            itemCtrls = new ItemControl[6];
        }
    }

    public class BoatControl
    {
        public GameObject boat;
        
        public Vector3 from = new Vector3(10, 2, 0);
        public Vector3 to = new Vector3(-10, 2, 0);
        public Vector3[] froms;
        public Vector3[] tos;
        public int status; // from = 1, to = 0
        public ItemControl leftSeat;
        public ItemControl rightSeat;

        public static float speed = 50; 

        public BoatControl()
        {
            status = 1;
            leftSeat = null;
            rightSeat = null;

            froms = new Vector3[] { new Vector3(9, 3, 0), new Vector3(11, 3, 0) };
            tos = new Vector3[] { new Vector3(-11, 3, 0), new Vector3(-9, 3, 0) };

            boat = (GameObject)Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), from, Quaternion.identity, null);           
            boat.name = "Boat";

            boat.AddComponent(typeof(ClickGUI)); 
        }
        
        public bool IsEmpty()
        {
            if (leftSeat == null && rightSeat == null) return true;
            else return false;
        }
        public bool IsFull()
        {
            if (leftSeat != null && rightSeat != null) return true;
            else return false;
        }
        public Vector3 GetOnBoat(ItemControl item)
        {
            if (leftSeat == null)
            {
                leftSeat = item;
                return status == -1 ? tos[0] : froms[0];
            }
            else if (rightSeat == null)
            {
                rightSeat = item;
                return status == -1 ? tos[1] : froms[1];
            }
            else return Vector3.zero;
        }
        public ItemControl GetOffBoat(ItemControl item)
        {
            
            ItemControl temp = null;
            if (leftSeat == item)
            {
                temp = leftSeat;
                leftSeat = null;
            }
            else if (rightSeat == item)
            {
                temp = rightSeat;
                rightSeat = null;
            }
            return temp;
        }
        public int GetItemNum(int type) // get the num of item, priest or devil
        {
            int count = 0;
            if (leftSeat != null && leftSeat.itemType == type) count++;
            if (rightSeat != null && rightSeat.itemType == type) count++;
            return count;
        }
        public void Reset()
        {
            
            leftSeat = null;
            rightSeat = null;
        }
    }
}