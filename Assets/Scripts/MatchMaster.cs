using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchMaster : MonoBehaviour
{
	public const int SCORE_MULT = 100;

	public LayerMask matchSearchLayerMask;

	private TimerModeScript timer;
	private SoundSystem soundSystem;
	private float neighborSearchRadius = 0.85f;
	private int score;

	public static MatchMaster Instance
	{
		get;
		private set;
	}

	public int Score
	{
		get { return score; }
	}

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		timer = GetComponent<TimerModeScript>();
		soundSystem = GetComponent<SoundSystem>();

		score = 0;
	}

	public void CheckMatch( DotScript startingDot )
	{
		DotColor.ColorValue targetColor = startingDot.ColorValue;
		List<DotScript> matchedDots = new List<DotScript>();
		matchedDots.Add( startingDot );

		for( int i = 0; i < matchedDots.Count; i++ )
		{
			Vector2 pos = matchedDots[ i ].transform.position;
			Collider2D[] cols = Physics2D.OverlapCircleAll( pos, neighborSearchRadius, matchSearchLayerMask );
			for( int j = 0; j < cols.Length; j++ )
			{
				DotScript dot = cols[ j ].gameObject.GetComponent<DotScript>();

				if( dot.ColorValue == targetColor )
				{
					if( !matchedDots.Contains( dot ) )
					{
						matchedDots.Add( dot );
					}
				}
			}
		}

		if( matchedDots.Count >= 3 )
		{
			StartCoroutine( TriggerMatchSequence( matchedDots ) );
		}
	}

	private void AddScoreForMatch( int numMatched )
	{
		int amt = (int)( SCORE_MULT * ( 0.5f * numMatched - 0.5f ) );
		score += amt;
		timer.AddScoreToTime( numMatched );
	}

	private IEnumerator TriggerMatchSequence( List<DotScript> matchedDots )
	{
		int numMatched = matchedDots.Count;
		for( int i = 0; i < numMatched; i++ )
		{
			matchedDots[ i ].OnMatched();
			soundSystem.PlayOneShot( "dragOverSound", 0.5f, 1.25f );
			yield return new WaitForSeconds( 0.05f );
			//Destroy( matchedDots[ i ].gameObject );
		}

		yield return new WaitForSeconds( 0.2f );

		AddScoreForMatch( numMatched );
		EventManager.TriggerEvent( "RepopGrid" );
	}
}
