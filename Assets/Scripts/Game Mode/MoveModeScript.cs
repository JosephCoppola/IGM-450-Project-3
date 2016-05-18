using UnityEngine;
using System.Collections;

public class MoveModeScript : MonoBehaviour
{
	public const int MATCH_MODE_MOVES = 30;

	private int numMoves = 0;
	private bool useMatchMode = false;

	private SoundSystem soundSystem;

	public int NumMoves
	{
		get { return numMoves; }
	}

	void Awake()
	{
		if( GameMode.CurrentMode == GameMode.Mode.MOVES )
		{
			useMatchMode = true;
			numMoves = MATCH_MODE_MOVES;
		}
	}

	void Start()
	{
		soundSystem = GetComponent<SoundSystem>();
		EventManager.AddEventListener( "CompletedMove", OnMoveCompleted );
	}

	private void OnMoveCompleted()
	{
		numMoves += useMatchMode ? -1 : 1; // Subtract if match mode

		if( useMatchMode && numMoves <= 0 )
		{
			soundSystem.PlayOneShot( "releaseDragSound", 1.0f, 3.0f );
			EventManager.TriggerEvent( "GameOver" );
		}

		EventManager.TriggerEvent( "MovesChange" );
	}
}
