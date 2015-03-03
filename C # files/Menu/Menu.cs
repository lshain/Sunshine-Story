using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public static bool GAME_PAUSED = false;
	private bool showPauseGUI;
	public GUISkin KOSSkin;
	private string pauseMessage;
	// Use this for initialization
	void Start () {
		pauseMessage = "Pause Game";
	}
	
	// Update is called once per frame
	void Update () {
		if(!GAME_PAUSED) {
			pauseMessage = "Pause Game";
		}
		if(GAME_PAUSED) {
			pauseMessage = "Resume Game";
		}
	}

	void OnGUI() {
		GUI.skin = KOSSkin;
		if(!Battle.inBattle) {
			if(GUI.Button(new Rect(Screen.width * 5/16 + 20, Screen.height * 18/20, Screen.width * 1/16, Screen.height * 1/16), pauseMessage) && !NPC.inDialouge) {
				GAME_PAUSED = !GAME_PAUSED;
				showPauseGUI = !showPauseGUI;
			}
			if(showPauseGUI) {
				GUI.Box(new Rect(Screen.width * 3/8, Screen.height * 3/8, Screen.width * 1/4, Screen.height * 1/8), "GAME PAUSED");
				if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 4/8 + 10, Screen.width * 1/8, Screen.height * 1/16), "QUIT?")) {
					Application.Quit();
				}
			}
		}
	}
}
