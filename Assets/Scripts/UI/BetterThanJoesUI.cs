using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BetterThanJoesUI : MonoBehaviour
{
	public GameObject timeTxtObj;
	public GameObject moveTxtObj;

	public Text scoreNumTxt;
	public Text movesNumTxt;
	public Text timeNumTxt;

	public MatchMaster matchMaster;
	public TimerModeScript timer;
	public MoveModeScript move;

	[SerializeField]
	private GameObject gameOverUI;

	private bool showTimer;

	void Start()
	{
		showTimer = ( GameMode.CurrentMode == GameMode.Mode.TIMER );

		if( showTimer )
		{
			timeTxtObj.SetActive( true );
			moveTxtObj.SetActive(false);
			UpdateTimeText();
		}

		UpdateScore();
		UpdateMoves();

		EventManager.AddEventListener( "MovesChange", UpdateMoves );
		EventManager.AddEventListener( "ScoreUp", UpdateScore );
		EventManager.AddEventListener ("GameOver", OpenGameOver);
	}

	void Update()
	{
		if( showTimer )
		{
			UpdateTimeText();
		}
	}

	private void OpenGameOver()
	{
		gameOverUI.SetActive (true);
	}

	private void UpdateScore()
	{
		scoreNumTxt.text = "Score : " + matchMaster.Score;
	}

	private void UpdateMoves()
	{
		movesNumTxt.text = "" + move.NumMoves;
	}

	private void UpdateTimeText()
	{
		timeNumTxt.text = string.Format( "{0:0.0}", timer.TimeRemaining );
	}
}
