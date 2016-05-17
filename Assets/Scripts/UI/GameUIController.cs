using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameUIController : MonoBehaviour {

	[SerializeField]
	private Image headerImage;
	[SerializeField]
	private Image headerTimeModeImage;
	[SerializeField]
	private Image headerMovesModeImage;
	[SerializeField]
	private Button mainMenuButton;
	[SerializeField]
	private Button playAgainButton;
	[SerializeField]
	private Button resumeButton;
	[SerializeField]
	private Text highScoreHeaderTxt;
	[SerializeField]
	private Text highScoreTxt;
	[SerializeField]
	private Text scoreTxt;
	[SerializeField]
	private MatchMaster gameMaster;

	private bool paused;

	Color green = new Color(64f/255f, 191f/255f, 128f/255f, 1f);
	Color blue = new Color(92f/255f, 133f/255f, 214f/255f, 1f);

	void OnEnable() {
		if (GameMode.CurrentMode == GameMode.Mode.MOVES) {
			headerImage.color = blue;
			highScoreHeaderTxt.color = blue;
			playAgainButton.image.color = blue;
			resumeButton.image.color = blue;
			mainMenuButton.image.color = green;
			headerMovesModeImage.gameObject.SetActive (true);
			headerTimeModeImage.gameObject.SetActive (false);
		} 
		else {
			headerImage.color = green;
			highScoreHeaderTxt.color = green;
			playAgainButton.image.color = green;
			resumeButton.image.color = green;
			mainMenuButton.image.color = blue;
			headerMovesModeImage.gameObject.SetActive (false);
			headerTimeModeImage.gameObject.SetActive (true);
		}

		if (paused) {
			resumeButton.gameObject.SetActive (true);
			playAgainButton.gameObject.SetActive (false);
		} 
		else {
			resumeButton.gameObject.SetActive (false);
			playAgainButton.gameObject.SetActive (true);
		}

		SaveScore ();
		PopulateScoreText ();
		PopulateHighScoreText ();
	}

	private void PopulateScoreText() {
		scoreTxt.text = gameMaster.Score.ToString ();
	}

	private void PopulateHighScoreText() {
		if (GameMode.CurrentMode == GameMode.Mode.MOVES) {
			highScoreTxt.text = gameMaster.RetrieveMoveHighScore ().ToString ();
		}
		else {
			highScoreTxt.text = gameMaster.RetrieveTimeHighScore ().ToString ();
		}
	}

	private void SaveScore() {
		int highScore = 0;

		if (GameMode.CurrentMode == GameMode.Mode.MOVES) {
			highScore = gameMaster.RetrieveMoveHighScore ();

			if (gameMaster.Score > highScore) {
				PlayerPrefs.SetInt ("MoveHighScore", gameMaster.Score);
			}
		}
		else {
			highScore = gameMaster.RetrieveTimeHighScore ();

			if (gameMaster.Score > highScore) {
				PlayerPrefs.SetInt ("TimeHighScore", gameMaster.Score);
			}
		}
	}

	public void OnPauseClick() {

		if (Time.timeScale == 1.0f) {
			Time.timeScale = 0.0f;
		} 
		else {
			Time.timeScale = 1.0f;
		}

		paused = !paused;
		gameObject.SetActive (!(gameObject.activeSelf));
	}

	public void Replay() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("Game");
	}

	public void ReturnToMenu() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene ("MainMenu");
	}


}
