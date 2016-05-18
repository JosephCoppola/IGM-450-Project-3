using UnityEngine;
using System.Collections;

public class TimerModeScript : MonoBehaviour
{
	public const float MAX_TIMER_SECONDS = 60.0f;

	private float currTime = MAX_TIMER_SECONDS;
	private float lastSecond;
	private float playSoundTime = 10.0f;
	private bool useTimerMode;

	private SoundSystem soundSystem;

	public float TimeRemaining
	{
		get { return currTime; }
	}

	void Start()
	{
		lastSecond = playSoundTime;
		useTimerMode = ( GameMode.CurrentMode == GameMode.Mode.TIMER );

		soundSystem = GetComponent<SoundSystem>();
	}

	void Update()
	{
		if( useTimerMode )
		{
			currTime -= Time.deltaTime;

			//print( currTime );

			if( currTime <= playSoundTime )
			{
				if( currTime > 0f )
				{
				if( currTime < lastSecond )
					{
						lastSecond = Mathf.FloorToInt( currTime );
						soundSystem.PlayOneShot( "dragOverSound", 1.0f, 3.0f );
					}
				}
				else
				{
					soundSystem.PlayOneShot( "releaseDragSound", 1.0f, 3.0f );
					currTime = 0f;
					EventManager.TriggerEvent( "GameOver" );
					this.enabled = false;
				}
			}
		}
	}

	public void AddScoreToTime( int numMatched )
	{
		if( currTime > 0f )
		{
			currTime += ( 1.2f * ( (float)numMatched - 3.0f ) + 1.0f );
			currTime = Mathf.Clamp( currTime, 0, MAX_TIMER_SECONDS );

			lastSecond = Mathf.FloorToInt( currTime );
		}
	}
}
