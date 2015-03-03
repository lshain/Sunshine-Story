using UnityEngine;
using System.Collections;

public class BeginGameText : MonoBehaviour {
	public GUISkin KosSkin;
	void OnGUI() {
		GUI.skin = KosSkin;
		if(GameState.STATE == GameState.gameState.title) {
			if(GUI.Button(new Rect(20, Screen.height * 17/20, 100,Screen.height * 1/20),"Begin Game")) {
				ScreenFade.LOADINGLVL = true;
				ScreenFade.LEVELtoLOAD = 1;
				ScreenFade.FADING = true;
			}
			if(GUI.Button(new Rect(20, Screen.height * 18/20, 100,Screen.height * 1/20),"Exit Game")) {
				Application.Quit();
			}
		}
	}
}
