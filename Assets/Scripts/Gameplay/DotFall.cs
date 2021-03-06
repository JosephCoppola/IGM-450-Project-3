﻿using UnityEngine;
using System.Collections;

public class DotFall : MonoBehaviour
{
	private SoundSystem soundSystem;
	private DotScript dotScript;
	private Collider2D dotCollider;
	private float speed = 5.0f;
	private float accel = 5.0f;

	void Start()
	{
		soundSystem = GetComponent<SoundSystem>();
		dotCollider = GetComponent<Collider2D>();
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
				dotCollider.enabled = false;
				dotScript.enabled = false;
			}
			float adjSpeed = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards( transform.position, transform.parent.position, adjSpeed );
			speed += accel * Time.deltaTime;
		}
		else if( !dotScript.enabled )
		{
			soundSystem.PlayOneShot( "dotFallSound", 0.2f );
			dotCollider.enabled = true;
			dotScript.enabled = true;
			speed = 5.0f;
		}
	}
}
