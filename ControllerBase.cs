using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mydirector;

namespace Mycontroller{

	//Boat
	public class BoatController {
		private GameObject boat;
		private Vector3 From = new Vector3 (4,0,0);
		private Vector3 To = new Vector3 (8, 0, 0);
		private Vector3[] from_positions;
		private Vector3[] to_positions;
		int is_from;
		CharacterController[] characters = new CharacterController[2];
		private Move move;
		public float speed = 30;

		public BoatController() {
			is_from = 1;

			from_positions = new Vector3[] { new Vector3 (4.5F,0.5F,0), new Vector3 (3.5F,0.5F,0) };
			to_positions = new Vector3[] { new Vector3 (8.5F, 0.5F, 0), new Vector3 (7.5F, 0.5F, 0) };

			boat = Object.Instantiate (Resources.Load ("Prefab/boat", typeof(GameObject)), From, Quaternion.identity, null) as GameObject;
			boat.name = "boat";

			move = boat.AddComponent (typeof(Move)) as Move;
			boat.AddComponent (typeof(UserClick));
		}
		public bool is_empty(){
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] != null) {
					return false;
				}
			}
			return true;
		}

		public Vector3 Move_to() {
			if (is_from == -1) {
				is_from = 1;
				return From;
			} else {
				is_from = -1;
				return To;
			}
		}

		public int getEmptyIndex() {
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null) {
					return i;
				}
			}
			return -1;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos;
			int emptyIndex = -1;
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null) {
					emptyIndex = i;
				}
			}
			if (is_from == -1) {
				pos = to_positions[emptyIndex];
			} else {
				pos = from_positions[emptyIndex];
			}
			return pos;
		}

		public void GetOnBoat(CharacterController characterCtrl) {
			int index = -1;
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null) {
					index = i;

				}
			}
			characters [index] = characterCtrl;
		}

		public CharacterController GetOffBoat(string characters_name) {
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] != null && characters [i].getName () == characters_name) {
					CharacterController cc = characters [i];
					characters [i] = null;
					return cc;
				}
			}
			return null;
		}

		public GameObject getGameobj() {
			return boat;
		}

		public int get_is_from() { 
			return is_from;
		}

		public int[] getCharacterNum() {
			int[] count = {0, 0};
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null)
					continue;
				if (!characters [i].Is_Devil ()) {
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}

		public void reset() {
			if (is_from == -1) {
				Move_to ();
			}
			boat.transform.position = From;
			characters = new CharacterController[2];
		}
	}



	//coat
	public class CoastController{
		private GameObject coast;
		private Vector3 From = new Vector3 (0, 0, 0);
		private Vector3 To = new Vector3(12,0,0);
		private Vector3[] positions;
		private int is_from;
		private CharacterController[] characters;
		public CoastController(string Where){
			positions = new Vector3[] {new Vector3 (2.5F,1.25F,0),new Vector3 (1.5F,1.25F,0), new Vector3 (0.5F,1.25F,0),new Vector3 (-0.5F,1.25F,0),new Vector3 (-1.5F,1.25F,0),new Vector3 (-2.5F,1.25F,0)
			};
			characters = new CharacterController[6];
			if (Where == "from") {
				coast = Object.Instantiate (Resources.Load ("Prefab/stone", typeof(GameObject)), From, Quaternion.identity, null) as GameObject;
				coast.name = "from";
				is_from = 1;
			} else if(Where == "to"){
				coast = Object.Instantiate (Resources.Load ("Prefab/stone", typeof(GameObject)), To, Quaternion.identity, null) as GameObject;
				coast.name = "to";
				is_from = -1;
			}
		}
		public Vector3 getEmptyPos(){
			int index = -1;
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null) {
					index = i;
					break;
				}
			}
			Vector3 pos = positions [index];

			if(is_from==-1)
				pos.x +=12;

			return pos;
		}
		public void getOnCoast(CharacterController character_ctrl){
			int index = -1;
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null) {
					index = i;
					break;
				}
			}
			characters [index] = character_ctrl;
		}

		public CharacterController getOffCoast(string character_name){
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] != null && characters [i].getName () == character_name) {
					CharacterController cc = characters [i];
					characters [i] = null;
					return cc;
				}
			}
			return null;
		}
		public int get_is_from(){
			return is_from;
		}
		public int[] get_character_num(){
			int[] num = { 0, 0 };
			for (int i = 0; i < characters.Length; i++) {
				if (characters [i] == null)
					continue;
				if (!characters[i].Is_Devil ()) {
					num [0]++;
				} else {
					num [1]++;
				}
			}
			return num;
		}
		public void reset(){
			characters = new CharacterController[6];
		}
	}


	//character
	public class CharacterController{
		private GameObject character;
		private Move move;
		private UserClick click;
		private bool is_devil;
		private bool is_on_boat = false;
		private CoastController coast;
		public float move_speed = 30;
		// Use this for initialization
		public CharacterController(string name){
			if(name == "priest"){
				character = Object.Instantiate(Resources.Load("Prefab/priest",typeof(GameObject)),Vector3.zero,Quaternion.identity,null) as GameObject;
				is_devil = false;
			}else if(name == "devil"){
				character = Object.Instantiate(Resources.Load("Prefab/devil",typeof(GameObject)),Vector3.zero,Quaternion.identity,null) as GameObject;
				is_devil = true;
			}
			move = character.AddComponent(typeof(Move))as Move;
			click = character.AddComponent(typeof(UserClick))as UserClick;
			click.setController(this);
		}
		public void setName(string _name){
			character.name = _name;
		}
		public string getName(){
			return character.name;
		}
		public bool Is_Devil(){
			return is_devil;
		}
		public bool Is_On_Boat(){
			return is_on_boat;
		}
		public void setPos(Vector3 pos){
			character.transform.position = pos;
		}
		public Vector3 getPos(){
			return character.transform.position;
		}

		public GameObject getGameobj(){
			return character;
		}


		public CoastController getCoastController() {
			return coast;
		}
		public void getOnBoat(BoatController boatCtrl) {
			coast = null;
			character.transform.parent = boatCtrl.getGameobj().transform;
			is_on_boat = true;
		}

		public void getOnCoast(CoastController coastCtrl) {
			coast = coastCtrl;
			character.transform.parent = null;
			is_on_boat = false;
		}
		public void reset() {
			coast = (Director.getInstace ().current as FirstController).fromCoast;
			getOnCoast (coast);
			setPos (coast.getEmptyPos());
			coast.getOnCoast (this);
		}

	}
}
