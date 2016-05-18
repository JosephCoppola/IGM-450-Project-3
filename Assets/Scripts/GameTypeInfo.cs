using UnityEngine;
using System.Collections;

public class GameTypeInfo : MonoBehaviour {

	public bool timed;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}

	void OnLevelWasLoaded(int level) {
		if (level == 0) {
			Destroy (gameObject);
		}
		else if (level == 1) {
			if (timed) {
				GameMode.SetGameMode (GameMode.Mode.TIMER);
			} 
			else {
				GameMode.SetGameMode (GameMode.Mode.MOVES);
			}
		}
	}
}
