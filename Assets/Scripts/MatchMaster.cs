using UnityEngine;
using System.Collections.Generic;

public class MatchMaster : MonoBehaviour
{
	public LayerMask matchSearchLayerMask;
	public Grid grid;

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

		if( matchedDots.Count < 3 )
		{
			// early return
			return;
		}

		for( int i = 0; i < matchedDots.Count; i++ )
		{
			Destroy( matchedDots[ i ].gameObject );
		}

		StartCoroutine( grid.Repopulate() );
	}
}
