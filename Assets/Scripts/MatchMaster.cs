using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchMaster : MonoBehaviour
{
	public LayerMask matchSearchLayerMask;
	public ScoreMaster scoreMaster;

	private float neighborSearchRadius = 0.85f;

	public static MatchMaster Instance
	{
		get;
		private set;
	}

	void Awake()
	{
		Instance = this;
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

	private IEnumerator TriggerMatchSequence( List<DotScript> matchedDots )
	{
		int numMatched = matchedDots.Count;
		for( int i = 0; i < numMatched; i++ )
		{
			matchedDots[ i ].OnMatched();
			yield return new WaitForSeconds( 0.05f );
			//Destroy( matchedDots[ i ].gameObject );
		}

		yield return new WaitForSeconds( 0.2f );

		scoreMaster.AddScoreForMatch( numMatched );
		EventManager.TriggerEvent( "RepopGrid" );
	}
}
