using UnityEngine;
using System.Collections;

public class MoveModeScript : MonoBehaviour
{
	public const int MATCH_MODE_MOVES = 30;

	private int numMoves = 0;
	private bool useMatchMode = false;

	public int NumMoves
	{
		get { return numMoves; }
	}

	void Start()
	{
		if( GameMode.CurrentMode == GameMode.Mode.MOVES )
		{
			useMatchMode = true;
			numMoves = MATCH_MODE_MOVES;
		}

		EventManager.AddEventListener( "CompletedMove", OnMoveCompleted );
	}

	private void OnMoveCompleted()
	{
		numMoves += useMatchMode ? -1 : 1; // Subtract if match mode

		if( useMatchMode && numMoves <= 0 )
		{
			EventManager.TriggerEvent( "GameOver" );
		}

		//print( numMoves );
	}
}
