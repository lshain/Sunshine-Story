using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public enum gameState {title, intro, game, battle, gameover, ending1, ending2, ending3};
	public static gameState STATE = gameState.title;

	public static bool isSunWarrior = false;
	public static bool isSkyRogue = true;
	public static bool isStarMage = false;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	void OnLevelWasLoaded(int level) {
		if (level == 0)
			STATE = gameState.title;
		else if (level == 1)
			STATE = gameState.intro;
		else if (level == 2)
			STATE = gameState.game;
		
	}
}
