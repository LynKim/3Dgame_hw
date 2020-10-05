using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;
 
namespace game
{
    public enum State { Start, SE_move, ES_move, End, Win, Lose };
 
    public interface UserAction {
        void priestSOnSide();
        void priestEOnSide();
        void devilSOnSide();
        void devilEOnSide();
        void moveBoat();
        void offBoatL();
        void offBoatR();
        void reset();
    }
 
    public class SSDirector : System.Object, UserAction
    {
        private static SSDirector _instance;
 
        public Controller currentScenceController;
        public State state = State.Start;
        private Model game_obj;
 
        public static SSDirector GetInstance()
        {
            if(_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
 
        public Model getModel()
        {
            return game_obj;
        }
        
        internal void setModel(Model someone)
        {
            if(game_obj == null)
            {
                game_obj = someone;
            }
        }
 
        public void priestSOnSide()
        {
            game_obj.priS();
        }
        public void priestEOnSide()
        {
            game_obj.priE();
        }
        public void devilSOnSide()
        {
            game_obj.delS();
        }
        public void devilEOnSide()
        {
            game_obj.delE();
        }
        public void moveBoat()
        {
            game_obj.moveBoat();
        }
        public void offBoatL()
        {
            game_obj.getOffTheBoat(0);
        }
        public void offBoatR()
        {
            game_obj.getOffTheBoat(1);
        }
        public void reset()
        {
            game_obj.Reset();
        }
    }
 }
 
public class Controller : MonoBehaviour {
 
    // Use this for initialization
    void Start()
    {
        SSDirector one = SSDirector.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

