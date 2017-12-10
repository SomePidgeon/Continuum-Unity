﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public PlayerController playerControllerScript;
	public float BulletSpeed;
	public Rigidbody BulletRb;

	[Header ("Stats")]
	public float Lifetime;
	public float MaxLifetime = 30;
	public float DestroyMaxYPos = 30;
	public float ColliderYMaxPos = 12;
	public Collider BulletCol;
	public float DestroyDelayTime = 1;
	public Transform playerPos;
	public bool movedEnough;

	[Header ("Visuals")]
	public ParticleSystem BulletOuterParticles;
	public ParticleSystem BulletCoreParticles;

	[Header ("Camera Shake")]
	public CameraShake camShakeScript;
	public float shakeDuration;
	public float shakeTimeRemaining;
	public float shakeAmount;

	[Header ("Player Vibration")]
	public float LeftMotorRumble = 0.2f;
	public float RightMotorRumble = 0.2f;
	public float VibrationDuration = 0.25f;

	void Start ()
	{
		BulletCol.enabled = false;
		movedEnough = false;
		playerControllerScript = GameObject.Find ("PlayerController").GetComponent<PlayerController> ();
		camShakeScript = GameObject.Find ("CamShake").GetComponent<CameraShake> ();
		StartCameraShake ();
		Lifetime = 0;
		InvokeRepeating ("CheckForDestroy", 0, 1);
		playerPos = GameObject.Find ("PlayerCollider").transform;
	}

	void Update ()
	{
		BulletRb.velocity = transform.InverseTransformDirection (
				new Vector3 (
					0, 
					BulletSpeed * Time.deltaTime * Time.timeScale, 
					0
				));
		
		Lifetime += Time.unscaledDeltaTime;
		CheckForDestroy ();

		CheckForColliderDeactivate ();

		CheckColActivate ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Block") 
		{
			playerControllerScript.Vibrate (LeftMotorRumble, RightMotorRumble, VibrationDuration);
		}
	}

	void CheckForColliderDeactivate ()
	{
		if (BulletRb.transform.position.y > ColliderYMaxPos) 
		{
			BulletCol.enabled = false;
		}
	}

	void CheckColActivate ()
	{
		if (movedEnough == false) 
		{
			Debug.Log (Vector3.Distance(transform.position, playerPos.position));
			if (Vector3.Distance(transform.position, playerPos.position) < 0.75f)
			{
				BulletCol.enabled = false;
			}

			if (Vector3.Distance(transform.position, playerPos.position) >= 0.75f)
			{
				BulletCol.enabled = true;
				movedEnough = true;
			}
		}
	}

	void CheckForDestroy ()
	{
		if (Lifetime > MaxLifetime) 
		{
			Destroy (gameObject);
		}

		if (BulletRb.transform.position.y > DestroyMaxYPos) 
		{
			Destroy (gameObject);
		}
	}

	void StartCameraShake ()
	{
		if (camShakeScript.shakeDuration < shakeDuration)
		{
			camShakeScript.shakeDuration = shakeDuration;
		}

		if (camShakeScript.shakeTimeRemaining < shakeTimeRemaining)
		{
			camShakeScript.shakeTimeRemaining = shakeTimeRemaining;
		}
			
		camShakeScript.shakeAmount = shakeAmount;
	}

	public IEnumerator DestroyDelay ()
	{
		BulletCol.enabled = false;
		yield return new WaitForSecondsRealtime (DestroyDelayTime);
		Destroy (gameObject);
	}
}
