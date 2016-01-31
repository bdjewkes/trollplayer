using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoad : MonoBehaviour
{
	private IEnumerator _loadSceneRoutineReference;
	//public List<string> scenesToLoad;

	private void Start()
	{
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
	public void ReloadGame()
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
		var mainCamera = Camera.main;
		//skip bootstrap(the first scene), so start at 1
		for (var i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			SceneManager.LoadScene(i, LoadSceneMode.Additive);
		}

		yield return null; //we need to wait a frame before removing cameras
		destroyOtherCameras(mainCamera.gameObject);
		Destroy(gameObject); //remove this gameobject when done
		_loadSceneRoutineReference = null; //for symmetry, make sure to set this to nullso that we cans trt onloading
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