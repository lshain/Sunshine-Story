using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	string[] introStory = new string[4];
	public Texture2D[] introPics = new Texture2D[4];
	public GUISkin KosSkin;
	private int index;
	public ScreenFade sfscript;
	public AudioManager audscript;
	//private bool music_playing = false;


	void Start () {
		sfscript = GetComponent<ScreenFade>();
		audscript = GetComponent<AudioManager>();
		audscript.WorldMusic();
		introStory[0] = "It was a long time ago in a kingdom far across the sea";
		introStory[1] = "that a powerful wizard stole the moon from the sky";
		introStory[2] = "leaving the people of the desert beneath the blistering sunlight for 40 days.";
		introStory[3] = "With the oases drying up, the people turned towards a young...";
	}
	void Update() {
		if(ScreenFade.FADING_PAUSE && ScreenFade.LEVELtoLOAD == 2)
			audscript.StopMusic();
	}

	void OnGUI() {
		GUI.depth = 1;
		if(GameState.STATE == GameState.gameState.intro) {
			//if(!music_playing) {
				//audscript.WorldMusic();
				//music_playing = true;
			//}

			GUI.skin = KosSkin;
			if(index <= introStory.Length) {
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),introPics[index]);
			}

			bool next = false;
			GUI.Box(new Rect(Screen.width * 1/4,Screen.height * 3/4,Screen.width * 1/2,Screen.height*1/12),introStory[index]);

			if(index < introStory.Length - 1) {
				if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Next")) {
					next = true;
				}
			}
			if(index == introStory.Length - 1) {
				if(GUI.Button(new Rect(Screen.width * 3/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Sun Warrior")) {
					GameState.isSunWarrior = true;
					GameState.isSkyRogue = false;
					GameState.isStarMage = false;
					next = true;
				}
				if(GUI.Button(new Rect(Screen.width * 7/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Sky Rogue")) {
					GameState.isSkyRogue = true;
					GameState.isStarMage = false;
					GameState.isSunWarrior = false;
					next = true;
				}
				if(GUI.Button(new Rect(Screen.width * 11/16, Screen.height * 29/32, Screen.width * 1/8, Screen.height * 1/16),"Star Mage")) {
					GameState.isStarMage = true;
					GameState.isSkyRogue = false;
					GameState.isSunWarrior = false;
					next = true;
				}
			}
			if(next) {
				if(index < introStory.Length - 1) {
					index++;
					next = false;
				}
				else {
					Debug.Log ("load next scene");

					ScreenFade.LOADINGLVL = true;
					ScreenFade.LEVELtoLOAD = 2;
					ScreenFade.FADING = true;
				}
			}
		}
	}
}
