using UnityEngine;
using System.Collections;

public class Event1 : MonoBehaviour {
	public static bool BOSS1_DEFEATED = false;
	public GUISkin KOSSkin;
	public string[] information = new string[5];
	private int index = 0;
	public string speaker;
	public Player pscript;
	public Transform notGameOverSpawnPlace;

	void Update() {
		if(pscript == null) {
			pscript = GameObject.FindWithTag("Player").GetComponent<Player>();
		}
	}
	void OnGUI () {
		GUI.skin = KOSSkin;
		if(BOSS1_DEFEATED && !Battle.inBattle) {
			Menu.GAME_PAUSED = true;
			NPC.inDialouge = true;

			GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/16),speaker + ": " + information[index]);
			
			if(index < information.Length -1)
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"Next"))
					index++;
			if(index == information.Length -1) {
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 3/8, Screen.width/4, Screen.height/16),"The End?") 
				   && GameState.STATE != GameState.gameState.ending2) {
					ScreenFade.FADING = true;
					GameState.STATE = GameState.gameState.ending2;
					AudioManager.ENDING_MUSIC_CHANGE = true;
				}
				if(GUI.Button(new Rect(Screen.width*3/8,Screen.height * 4/8, Screen.width/4, Screen.height/16),"Not the End?")) {
					Menu.GAME_PAUSED = false;
					NPC.inDialouge = false;
					pscript.player.charMesh.transform.position = new Vector3(notGameOverSpawnPlace.position.x,
					                                                         notGameOverSpawnPlace.position.y + 5,
					                                                         notGameOverSpawnPlace.position.z);
					AreaSwitch.LAST_SPAWN = notGameOverSpawnPlace.position;
					Destroy (gameObject);
				}
			}
		}
	}
}
