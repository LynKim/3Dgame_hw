using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mycontroller;

namespace Mydirector{
	
	public class Director:System.Object {
		private static Director instance;
		public SceneController current{ get ; set; }
		public static Director getInstace(){
			if (instance == null) {
				instance = new Director ();
			}
			return instance;
		}
	}


	public interface UserAction {
		void moveBoat();
		void moveCharacter(Mycontroller.CharacterController characterCtrl);
		void restart();
	}


	public interface SceneController{
		void loadResources();
	}
}