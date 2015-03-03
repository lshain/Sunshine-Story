using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {
	public GUISkin KOSSkin;
	private bool showMessage = false;
	private string message;
	private string message1 = "Narrator: The hero can't have gone that way, they needed to go elsewhere.";
	private string message2 = "Narrator: I'm pretty sure the hero didn't want to go there.";
	private string message3 = "Narrator: After a few moments, the hero chose to go the other way.";
	private string message4 = "Narrator: Nothing interesting here, better get back to the story.";

	void OnTriggerEnter(Collider other) {
		if(other.collider.gameObject.CompareTag("Player")) {
			showMessage = true;
			selectMessage();
		}
	}
	void OnTriggerExit(Collider other) {
		showMessage = false;
	}

	void OnGUI() {
		GUI.skin = KOSSkin;
		if(showMessage) {
			GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/16), message);
			}
	}

	void selectMessage() {
		int caseSwitch = Random.Range(1,5);
		switch (caseSwitch)
		{
		case 1:
			message = message1;
			break;
		case 2:
			message = message2;
			break;
		case 3:
			message = message3;
			break;
		case 4:
			message = message4;
			break;
		default:
			message = message1;
			break;
		}
	}
}
