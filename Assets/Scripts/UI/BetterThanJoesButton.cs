using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BetterThanJoesButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick( PointerEventData e )
	{
		if( GameMode.CurrentMode == GameMode.Mode.MOVES )
		{
			GameMode.SetGameMode( GameMode.Mode.TIMER );
		}
		else if( GameMode.CurrentMode == GameMode.Mode.TIMER )
		{
			GameMode.SetGameMode( GameMode.Mode.MOVES );
		}

		SceneManager.LoadScene( "Game" );
	}
}
