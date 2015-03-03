using UnityEngine;
using System.Collections;

public class AreaSwitch : MonoBehaviour {
	bool showMessage = false;
	public bool isExit = true;
	private string exitMessage;
	public GUISkin KOSSkin;
	public Transform nextAreaSpawnPoint;
	private bool needToSwitch = false;
	public static Vector3 LAST_SPAWN;
	void Start() {
		if(isExit)
			exitMessage = "Move to the next area?";
		if(!isExit)
			exitMessage = "Move to previous area?";
	}
	void OnTriggerEnter(Collider other) {
		if(other.collider.gameObject.CompareTag("Player"))
			showMessage = true;
	}
	void OnTriggerExit(Collider other) {
		if(other.collider.gameObject.CompareTag("Player"))
			showMessage = false;
	}
	void OnGUI() {
		GUI.skin = KOSSkin;
		if(showMessage && !Menu.GAME_PAUSED && !Inventory.inInventory) {
			if(GUI.Button (new Rect(Screen.width*3/8, Screen.height/4, Screen.width/4, Screen.height/16),exitMessage)) {
				ScreenFade.FADING = true;
				needToSwitch = true;
			}
		}
	}

	void Update() {
		if(needToSwitch && ScreenFade.FADING_PAUSE)
			switchArea();
	}

	void switchArea() {
		GameObject.FindWithTag("Player").transform.position = new Vector3(nextAreaSpawnPoint.position.x,
		                                                                  nextAreaSpawnPoint.position.y + 5,
		                                                                  nextAreaSpawnPoint.position.z);
		LAST_SPAWN = GameObject.FindWithTag("Player").transform.position;
		GameObject.FindWithTag("MainCamera").transform.position = new Vector3(LAST_SPAWN.x, LAST_SPAWN.y + 10, LAST_SPAWN.z);

		needToSwitch = false;
	}
}
