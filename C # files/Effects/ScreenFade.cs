using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {
	public Texture2D fadeTexture;
	private float alpha = 0.0f;
	public static bool FADING = false;
	public static bool FADING_PAUSE = false;
	public static int LEVELtoLOAD;
	public static bool LOADINGLVL;
	private bool doneIn = false;
	private float count = 0.0f;
	private float fadeSpeed = 1.0f;

	void OnGUI() {
		GUI.depth = 0;
		if (alpha > 0.05f) {
		GUI.color = new Color() { a = alpha };
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),fadeTexture,ScaleMode.StretchToFill);
		}
	}

	void Update () {
		if(LOADINGLVL)
			fadeTexInOut(true, LEVELtoLOAD);
		else
			fadeTexInOut(false,1);

		if(FADING)
			Menu.GAME_PAUSED = true;
	}

	public void fadeTexInOut(bool loadingLvl, int lvlNumber) {
		if(FADING) {
			if(alpha < 0.95f && !doneIn) {
				alpha += fadeSpeed * Time.deltaTime;
			}
			if(alpha >= 0.95 && count < 3.0f) {
				alpha = 1;
				count += Time.deltaTime;
				if(count >= 3.0f) {
					doneIn = true;
					FADING_PAUSE = true;
					if(loadingLvl) {
						Application.LoadLevel(lvlNumber);
					}
				}
			}

			if(alpha > 0.05f && doneIn) {
				alpha -= fadeSpeed * Time.deltaTime;
			}
			if(alpha <= 0.05f && doneIn) {
				FADING = false;
				FADING_PAUSE = false;
				doneIn = false;
				Menu.GAME_PAUSED = false;

				alpha = 0;
				count = 0;

				LOADINGLVL = false;
			}
		}
	}
}
