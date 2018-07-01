﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LocalSceneLoader : MonoBehaviour 
{
	[Tooltip ("Scene loader from Init.")]
	public SceneLoader sceneLoaderScript;
	[Tooltip ("Scene is going to laod another scene.")]
	public bool SceneLoadCommit;

	public InitManager initManagerScript;

	void Start ()
	{
		initManagerScript = GameObject.Find ("MANAGERS").GetComponent<InitManager> ();
		sceneLoaderScript = GameObject.Find ("SceneLoader").GetComponent<SceneLoader> ();
	}
		
	public void LoadScene (string sceneName)
	{
		if (SceneLoadCommit == false)
		{
			SceneLoadCommit = true;
			sceneLoaderScript.SceneName = sceneName;
			StartCoroutine (SceneLoadSequence ());

			if (sceneName == "menu") 
			{
				initManagerScript.LoadingMissionText.text = "MAIN MENU";
			}
		}

		return;
	}

	IEnumerator SceneLoadSequence ()
	{
		sceneLoaderScript.StartLoadSequence ();
		yield return null;
	}
}