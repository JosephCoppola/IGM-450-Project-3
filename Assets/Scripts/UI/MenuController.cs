using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {

	[SerializeField]
	private GameObject gameInfoPrefab;

	public void OnTimedClick()
	{
		GameTypeInfo infoToPass = Instantiate (gameInfoPrefab).GetComponent<GameTypeInfo> ();
		infoToPass.timed = true;
		SceneManager.LoadScene ("Game");
	}

	public void OnMovesClick()
	{
		GameTypeInfo infoToPass = Instantiate (gameInfoPrefab).GetComponent<GameTypeInfo> ();
		infoToPass.timed = false;
		SceneManager.LoadScene ("Game");
	}
}
