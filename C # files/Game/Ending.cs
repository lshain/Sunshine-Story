using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {
	public GUISkin KOSSkin;
	public Texture2D[] endingPics = new Texture2D[3];
	public string[] endingStory = new string[3];
	private bool startEnd = false;
	// Update is called once per frame
	void OnGUI () {
		GUI.skin = KOSSkin;
		if (GameState.STATE == GameState.gameState.ending1 && ScreenFade.FADING_PAUSE) {
			startEnd = true;
		}
		if (GameState.STATE == GameState.gameState.ending2 && ScreenFade.FADING_PAUSE) {
			startEnd = true;
		}
		if (GameState.STATE == GameState.gameState.ending3 && ScreenFade.FADING_PAUSE) {
			startEnd = true;
		}

		if (GameState.STATE == GameState.gameState.ending1 && startEnd) {
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),endingPics[0]);
			GUI.Box(new Rect(Screen.width * 1/4,Screen.height * 3/4,Screen.width * 1/2,Screen.height*1/12),endingStory[0]);
			if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Quit")) {
				Application.Quit();
			}
		}
		if (GameState.STATE == GameState.gameState.ending2 && startEnd) {
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),endingPics[1]);
			GUI.Box(new Rect(Screen.width * 1/4,Screen.height * 3/4,Screen.width * 1/2,Screen.height*1/12),endingStory[1]);
			if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Quit")) {
				Application.Quit();
			}
		}
		if (GameState.STATE == GameState.gameState.ending3 && startEnd) {
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), endingPics[2]);
			GUI.Box(new Rect(Screen.width * 1/4,Screen.height * 3/4,Screen.width * 1/2,Screen.height*1/12),endingStory[2]);
			if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Quit")) {
				Application.Quit();
			}
		}
	}
}
