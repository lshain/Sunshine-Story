using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip[] music;
	public static bool ENDING_MUSIC_CHANGE;

	public void WorldMusic() {
		audio.clip = music[0];
		audio.volume = 0.15f;
		audio.Play();
	}
	public void BattleMusic() {
		audio.clip = music[1];
		audio.volume = 1.0f;
		audio.Play();
	}
	public void GameOverMusic() {
		audio.clip = music[2];
		audio.volume = 0.5f;
		audio.PlayOneShot(audio.clip);
	}
	public void BossMusic() {
		audio.clip = music[3];
		audio.volume = 0.4f;
		audio.Play();
	}
	public void GoodEndingMusic() {
		audio.clip = music[4];
		audio.volume = 0.8f;
		audio.Play();
	}

	public void StopMusic() {
		audio.Stop();
	}

	void Update() {
		if (GameState.STATE == GameState.gameState.ending1 && ENDING_MUSIC_CHANGE) {
			GameOverMusic();
			ENDING_MUSIC_CHANGE = false;
		}
		if (GameState.STATE == GameState.gameState.ending2 && ENDING_MUSIC_CHANGE) {
			GameOverMusic();
			ENDING_MUSIC_CHANGE = false;
		}
		if (GameState.STATE == GameState.gameState.ending3 && ENDING_MUSIC_CHANGE) {
			GoodEndingMusic();
			ENDING_MUSIC_CHANGE = false;
		}
	}
}
