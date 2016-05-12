using UnityEngine;
using System.Collections;

public static class GameMode
{
	public enum Mode
	{
		MOVES,
		TIMER
	}

	public static Mode CurrentMode
	{
		get;
		private set;
	}

	public static void SetGameMode( Mode mode )
	{
		CurrentMode = mode;
	}
}
