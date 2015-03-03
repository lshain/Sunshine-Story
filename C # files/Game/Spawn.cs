using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	public GameObject PlayerChar;
	public Transform SpawnPoint;
	private GameState gsscript;

	void Start() {
		gsscript = GetComponent<GameState>();
	}

	void Update() {
		PlayerChar = GameObject.FindGameObjectWithTag("Player");
		if(PlayerChar == null) {
			loadChar();
			Debug.Log ("Loading Player");
		}
	}

	void loadChar() {
		if(GameState.isStarMage) {
			GameObject clone = Instantiate (Resources.Load("Player/StarMage")) as GameObject;
			clone.transform.position = SpawnPoint.position;
			clone.layer = 8;
			clone.transform.GetChild(1).gameObject.layer = 8;
			Debug.Log("Spawned in Star Mage");
		}
		if(GameState.isSkyRogue) {
			GameObject clone = Instantiate (Resources.Load("Player/SkyRogue")) as GameObject;
			clone.transform.position = SpawnPoint.position;
			clone.layer = 8;
			clone.transform.GetChild(1).gameObject.layer = 8;
			Debug.Log("Spawned in Sky Rogue");
		}
		if(GameState.isSunWarrior) {
			GameObject clone = Instantiate (Resources.Load("Player/SunWarrior")) as GameObject;
			clone.transform.position = SpawnPoint.position;
			clone.layer = 8;
			clone.transform.GetChild(1).gameObject.layer = 8;
			Debug.Log("Spawned in Sun Warrior");
		}
	}
}
