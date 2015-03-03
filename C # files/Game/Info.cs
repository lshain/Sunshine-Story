using UnityEngine;
using System.Collections;

public class Info : MonoBehaviour {
	bool showInfo = false;
	private int index = 0;
	public string[] information = new string[5];
	public GUISkin KOSSkin;

	void OnTriggerEnter(Collider other) {
		if(other.collider.gameObject.CompareTag("Player")) {
			showInfo = true;
		}
	}
	void OnTriggerExit(Collider other) {
		if(other.collider.gameObject.CompareTag("Player")) {
			showInfo = false;
			index = 0;
		}
	}

	void OnGUI() {
		GUI.skin = KOSSkin;
		if(showInfo && !Menu.GAME_PAUSED && !Inventory.inInventory) {
			GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/16), information[index]);

			if(index < information.Length -1)
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"Next"))
			  		index++;
			if(index == information.Length -1)
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"OK"))
					showInfo = false;
		}
	}
}
