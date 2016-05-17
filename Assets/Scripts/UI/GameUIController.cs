using UnityEngine;
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
	}

	void OnPauseClick() {
		paused = !paused;
		gameObject.SetActive (gameObject.activeSelf);
	}
}
