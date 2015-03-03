using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
	public string speaker;
	bool speaking = false;
	private int index = 0;
	public string[] information = new string[5];
	public GUISkin KOSSkin;
	public static bool inDialouge;
	public bool deleteAfterSpeach = false;
	private GameObject npcEffect = new GameObject();
	private bool spokenTo = false;
	public string effectName;
	public bool leadsToBadEnd = false;
	
	void OnTriggerEnter(Collider other) {
		if(other.collider.gameObject.CompareTag("Player") && !spokenTo) {
			speaking = true;
			Menu.GAME_PAUSED = true;
			inDialouge = true;
		}
	}
	
	void OnGUI() {
		GUI.skin = KOSSkin;
		if(speaking && !Inventory.inInventory) {
			GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/16),speaker + ": " + information[index]);
			
			if(index < information.Length -1)
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"Next"))
					index++;
			if(index == information.Length -1) {
				if(!leadsToBadEnd) {
					if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"OK")) {
						speaking = false;
						Menu.GAME_PAUSED = false;
						inDialouge = false;
						spokenTo = true;
						if(deleteAfterSpeach) {
							npcEffect = Resources.Load("Effects/" + effectName) as GameObject;
							npcEffect = Instantiate(npcEffect) as GameObject;
							npcEffect.transform.position = gameObject.transform.position;
							Destroy (gameObject);
						}
					}
				}
				if(leadsToBadEnd) {
					if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"Fight")) {
						speaking = false;
						Menu.GAME_PAUSED = false;
						inDialouge = false;
						spokenTo = true;
						if(deleteAfterSpeach) {
							npcEffect = Resources.Load("Effects/" + effectName) as GameObject;
							npcEffect = Instantiate(npcEffect) as GameObject;
							npcEffect.transform.position = gameObject.transform.position;
							Destroy (gameObject);
						}
					}
					if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 4/8, Screen.width/4, Screen.height/16),"Leave") && 
					   GameState.STATE != GameState.gameState.ending1) {
						ScreenFade.FADING = true;
						GameState.STATE = GameState.gameState.ending1;
						AudioManager.ENDING_MUSIC_CHANGE = true;
					}
				}
			}
		}
	}
}
