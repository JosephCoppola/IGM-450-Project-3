using UnityEngine;
using System.Collections;

public class ScoreMaster : MonoBehaviour
{
	public const int SCORE_INC = 100;
	private int score;

	void Start()
	{
		score = 0;
	}

	public void AddScoreForMatch( int numMatched )
	{
		score += (int)( SCORE_INC * ( 0.5f * numMatched - 0.5f ) );
	}
}
