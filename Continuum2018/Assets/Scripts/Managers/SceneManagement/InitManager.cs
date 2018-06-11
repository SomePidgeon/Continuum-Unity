﻿using UnityEngine;
using UnityEngine.PostProcessing;
using TMPro;

public class InitManager : MonoBehaviour 
{
	public SaveAndLoadScript saveAndLoadScript;
	public PostProcessingBehaviour postProcess;
	public TextMeshProUGUI LoadingMissionText;

	void Start ()
	{
		LoadingMissionText.text = "MAIN MENU";
		saveAndLoadScript.LoadSettingsData ();
		saveAndLoadScript.LoadPlayerData ();

		if (saveAndLoadScript.QualitySettingsIndex == 0) 
		{
			postProcess.enabled = false;
		
		}

		if (saveAndLoadScript.QualitySettingsIndex == 1) 
		{
			postProcess.enabled = true;
		}
	}
}