using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoad : MonoBehaviour
{
	private const string OUTRO_SCENE_NAME = "outro";
    private const string OUTRO_SCENE_FAILURE_NAME = "outroFailure";
	private const string INTRO_SCENE_NAME = "intro";
	private IEnumerator _loadSceneRoutineReference;
    private bool _failed = false;

	//public List<string> scenesToLoad;
	private Camera _mainCamera;

	private void Start()
	{
		_mainCamera = Camera.main;
		LoadScenes();
	}

	private void LoadScenes()
	{
		if(_loadSceneRoutineReference != null)
		{
			Debug.LogError("tried to load game while game is load, ignoring for now");
			return;
		}

		_loadSceneRoutineReference = LoadScenesRoutine();
		StartCoroutine(_loadSceneRoutineReference);
	}

	[ContextMenu("Test Reload game")]
	private void ReloadGame()
	{
		if(_loadSceneRoutineReference != null)
		{
			Debug.LogError("Reloading game while game is loading -- race condition");
			return;
		}
		Debug.LogError("Reloaing game!");
		for (var i = 0; i < SceneManager.sceneCount; i++)
		{
			SceneManager.UnloadScene(i);
		}

		SceneManager.LoadScene(0);
	}

	private IEnumerator LoadScenesRoutine()
	{
		yield return StartCoroutine(waitForIntroToFinish());

		yield return StartCoroutine(waitForGameToFinish());

		yield return StartCoroutine(waitForOuttroToFinish(_failed ? OUTRO_SCENE_FAILURE_NAME: OUTRO_SCENE_NAME));

		Destroy(gameObject); //remove this gameobject when done
		_loadSceneRoutineReference = null; //for symmetry, make sure to set this to nullso that we cans trt onloading
		SceneManager.LoadScene(0);
	}

	//todo use ref action instead of copying code like this
	private IEnumerator waitForIntroToFinish()
	{
		SceneManager.LoadScene(INTRO_SCENE_NAME, LoadSceneMode.Additive);
		yield return null; //paranoia in waiting for loading before trying to grab a reference
		destroyOtherCameras(_mainCamera.gameObject);

		var introDone = false;
		Action introDoneDelegate = () => { introDone = true; };

		destroyOtherCameras(_mainCamera.gameObject);
		FindObjectOfType<IntroController>().OnIntroDone += introDoneDelegate;
		while (!introDone)
		{
			yield return null;
		}
		FindObjectOfType<IntroController>().OnIntroDone -= introDoneDelegate;
		SceneManager.UnloadScene(INTRO_SCENE_NAME);
	}

	private IEnumerator waitForOuttroToFinish(string outroName)
	{
		SceneManager.LoadScene(outroName, LoadSceneMode.Additive);
		yield return null; //paranoia in waiting for loading before trying to grab a reference
		destroyOtherCameras(_mainCamera.gameObject);

		var outtroDone = false;
		Action outtroDoneDelegate = () => { outtroDone = true; };

		destroyOtherCameras(_mainCamera.gameObject);
		FindObjectOfType<OuttroController>().OnOuttroDone += outtroDoneDelegate;
		while (!outtroDone)
		{
			yield return null;
		}
		FindObjectOfType<OuttroController>().OnOuttroDone -= outtroDoneDelegate;
		SceneManager.UnloadScene(outroName);
	}

	private IEnumerator waitForGameToFinish()
	{
        List<int> scenesToNotLoadForMainScene = new List<int>
		{
			0,
			1,
			5,
			6
			//SceneManager.GetSceneAt(0).name,
			//SceneManager.GetSceneByName(OUTRO_SCENE_NAME).name,
			//SceneManager.GetSceneByName(INTRO_SCENE_NAME).name
		};
		Debug.Log(SceneManager.sceneCountInBuildSettings);
		//skip bootstrap(the first scene), so start at 1
		for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			if(scenesToNotLoadForMainScene.Contains(i))
				continue;
			SceneManager.LoadScene(i, LoadSceneMode.Additive);
		}
		yield return null; //we need to wait a frame before removing cameras
		destroyOtherCameras(_mainCamera.gameObject);
		var gameMeta = FindObjectOfType<GameMeta>();
		while (!gameMeta.gameIsDone)
		{
			yield return null;
		}

        yield return new WaitForSeconds(1); // give a little break

        _failed = gameMeta.Failed();
		for(var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			if(scenesToNotLoadForMainScene.Contains(i))
				continue;
			SceneManager.UnloadScene(i);
		}
		Debug.Log("game finished coroutine done");
	}

	[ContextMenu("remove other cameras")]
	private void destroyOtherCamerasTest()
	{
		var mainCamera = Camera.main;
		destroyOtherCameras(mainCamera.gameObject);
	}

	private void destroyOtherCameras(GameObject keepMyCamera)
	{
		foreach (var cam in FindObjectsOfType<Camera>())
		{
			//Debug.Log("camera:" + cam.gameObject.name);
			if(cam.gameObject == keepMyCamera) continue;

			Destroy(cam.gameObject);
		}
	}

	//could add an onvaidate ifdefed so that it auto-saves the scene names in build settings, but that seems like overkill
}