using UnityEngine;
using System.Collections;

public class TimerModeScript : MonoBehaviour
{
	public const float MAX_TIMER_SECONDS = 60.0f;

	private float currTime = MAX_TIMER_SECONDS;
	private bool useTimerMode;

	public float TimeRemaining
	{
		get { return currTime; }
	}

	void Start()
	{
		useTimerMode = ( GameMode.CurrentMode == GameMode.Mode.TIMER );
	}

	void Update()
	{
		if( useTimerMode )
		{
			currTime -= Time.deltaTime;

			//print( currTime );

			if( currTime <= 0f )
			{
				currTime = 0f;
				EventManager.TriggerEvent( "GameOver" );
				this.enabled = false;
			}
		}
	}

	public void AddScoreToTime( int numMatched )
	{
		currTime += ( 1.2f * ( (float)numMatched - 3.0f ) + 1.0f );
		currTime = Mathf.Clamp( currTime, 0, MAX_TIMER_SECONDS );
	}
}
