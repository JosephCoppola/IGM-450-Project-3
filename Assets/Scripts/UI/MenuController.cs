using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	[SerializeField]
	private GameObject gameInfoPrefab;

	public void OnTimedClick()
	{
		GameMode.SetGameMode(GameMode.Mode.TIMER);
		SceneManager.LoadScene ("Game");
	}

	public void OnMovesClick()
	{
		GameMode.SetGameMode(GameMode.Mode.MOVES);
		SceneManager.LoadScene ("Game");
	}
}
