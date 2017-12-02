﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimescaleController : MonoBehaviour 
{
	public PlayerController playerControllerScript_P1;
	public GameController gameControllerScript;

	[Header ("Read Only")]
	public float TimeScaleView;
	public float FixedTimeStepView;
	public TextMeshProUGUI TimeScaleText;
	public TextMeshProUGUI FixedTimeStepText;
	public float MinimumTimeScale = 0.2f;
	public float MaximumTimeScale = 2.5f;

	[Header ("Time Manipulation")]
	public bool UpdateTargetTimeScale;
	public float TargetTimeScale = 1;
	public float TargetTimeScaleSmoothing = 10;
	public float TargetTimeScaleMult = 1;
	public float TargetTimeScaleAdd;
	public float TargetTimeScaleIncreaseRate = 0.01f;
	public Transform ReferencePoint;
	public float Distance;
	public Transform PlayerOne;
	public bool useTwoPlayers;
	public Transform PlayerTwo;

	[Header ("Override")]
	public bool isOverridingTimeScale;
	public float OverridingTimeScale;
	public float OverrideTimeScaleTimeRemaining;
	public float OverrideTimeScaleSmoothing = 10;

	void Start () 
	{
		
	}

	void Update () 
	{
		TimeScaleView = Time.timeScale;
		FixedTimeStepView = Time.fixedDeltaTime;
		CheckOverrideTimeScale ();
		CheckTargetTimeScale ();
		UpdateTimeScaleUI ();
	}

	void CheckTargetTimeScale ()
	{
		if (UpdateTargetTimeScale == true) 
		{
			if (isOverridingTimeScale == false)
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, TargetTimeScale, TargetTimeScaleSmoothing * Time.unscaledDeltaTime);
				UpdateMainTargetTimeScale ();
			}

			if (isOverridingTimeScale == true)
			{
				Time.timeScale = Mathf.Lerp (Time.timeScale, OverridingTimeScale, OverrideTimeScaleSmoothing * Time.unscaledDeltaTime);
			}
		}
	}

	void UpdateMainTargetTimeScale ()
	{
		if (useTwoPlayers == false) 
		{
			if (isOverridingTimeScale == false) 
			{
				Distance = PlayerOne.transform.position.y - ReferencePoint.position.y;

				TargetTimeScaleAdd += TargetTimeScaleIncreaseRate * Time.unscaledDeltaTime;
				TargetTimeScale = Mathf.Clamp (TargetTimeScaleMult * Distance + TargetTimeScaleAdd, MinimumTimeScale, MaximumTimeScale);
					
				// Updates fixed time step based on time scale.
				Time.fixedDeltaTime = Time.timeScale * 0.01f;
				Time.maximumParticleDeltaTime = Time.timeScale * 0.02f;
			}
		}
	}

	void CheckOverrideTimeScale ()
	{
		if (OverrideTimeScaleTimeRemaining > 0) 
		{
			if (playerControllerScript_P1.isPaused == false) 
			{
				OverrideTimeScaleTimeRemaining -= Time.unscaledDeltaTime;
			}

			if (isOverridingTimeScale == false) 
			{
				isOverridingTimeScale = true;
			}
		}

		if (OverrideTimeScaleTimeRemaining <= 0) 
		{
			if (isOverridingTimeScale == true) 
			{
				isOverridingTimeScale = false;
			}
		}
	}

	void UpdateTimeScaleUI ()
	{
		TimeScaleText.text = "TimeScale: " + System.Math.Round(Time.timeScale, 2);
		FixedTimeStepText.text = "FixedTimeStep: " + System.Math.Round (Time.fixedDeltaTime * 2, 5);
	}

}
