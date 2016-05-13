using UnityEngine;
using System.Collections;

public class BetterThanJoesUI : MonoBehaviour
{
	public GameObject timeTxtObj;

	public TextMesh scoreNumTxt;
	public TextMesh movesNumTxt;
	public TextMesh timeNumTxt;

	public MatchMaster matchMaster;
	public TimerModeScript timer;
	public MoveModeScript move;

	private bool showTimer;

	void Start()
	{
		showTimer = ( GameMode.CurrentMode == GameMode.Mode.TIMER );

		if( showTimer )
		{
			timeTxtObj.SetActive( true );
			UpdateTimeText();
		}

		UpdateScore();
		UpdateMoves();

		EventManager.AddEventListener( "MovesChange", UpdateMoves );
		EventManager.AddEventListener( "ScoreUp", UpdateScore );
	}

	void Update()
	{
		if( showTimer )
		{
			UpdateTimeText();
		}
	}

	private void UpdateScore()
	{
		scoreNumTxt.text = "" + matchMaster.Score;
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
