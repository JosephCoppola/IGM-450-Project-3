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
		StartCoroutine( GameOverDelay() );
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
		if( timer.TimeRemaining <= 10f )
		{
			if( timeNumTxt.color == Color.black )
			{
				timeNumTxt.color = Color.red;
			}
		}
		else if( timeNumTxt.color == Color.red )
		{
			timeNumTxt.color = Color.black;
		}
	}

	private IEnumerator GameOverDelay()
	{
		yield return new WaitForSeconds( 3.0f );
		gameOverUI.SetActive (true);
	}
}
