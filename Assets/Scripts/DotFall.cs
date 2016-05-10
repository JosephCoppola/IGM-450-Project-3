using UnityEngine;
using System.Collections;

public class DotFall : MonoBehaviour
{
	private DotScript dotScript;
	private float speed = 5.0f;
	private float accel = 5.0f;

	void Start()
	{
		dotScript = GetComponent<DotScript>();
		dotScript.enabled = false;
	}

	void Update()
	{
		if( transform.position != transform.parent.position )
		{
			if( dotScript.enabled )
			{
				dotScript.CancelDrag();
				dotScript.enabled = false;
			}
			float adjSpeed = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards( transform.position, transform.parent.position, adjSpeed );
			speed += accel * Time.deltaTime;
		}
		else if( !dotScript.enabled )
		{
			dotScript.enabled = true;
			speed = 5.0f;
		}
	}
}
