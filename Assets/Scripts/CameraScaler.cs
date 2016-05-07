using UnityEngine;
using System.Collections;

public class CameraScaler : MonoBehaviour
{
	public float desiredWidth = 5.625f;

	private int lastWidth;

	void Start ()
	{
		SetCameraScale();
	}

	void Update ()
	{
		// Yeah, I know. Deal with it
		if (lastWidth != Screen.width)
		{
			SetCameraScale();
		}
	}

	private void SetCameraScale()
	{
		Camera cam = Camera.main;

		float ar = Camera.main.aspect;
		float size = desiredWidth / ar * 0.5f;
		cam.orthographicSize = size;

		lastWidth = Screen.width;
	}
}
