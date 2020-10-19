using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mycontroller;

public class Judge : MonoBehaviour {
	private CoastController From;
	private CoastController To;
	private BoatController Boat;
	public Judge(CoastController from_coast,CoastController to_coast,BoatController boat)
	{
		this.From = from_coast;
		this.Boat = boat;
		this.To = to_coast;
	}
	public int check()
	{
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = From.get_character_num ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = To.get_character_num ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		
			return 2;

		int[] boatCount = Boat.getCharacterNum ();
		if (Boat.get_is_from () == -1) {	
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {	
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;	
	}
}
