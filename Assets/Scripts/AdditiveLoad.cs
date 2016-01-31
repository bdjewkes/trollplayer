using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AdditiveLoad : MonoBehaviour
{
	//public List<string> scenesToLoad;

	void Start()
	{
		StartCoroutine(LoadScenes());
	}

	IEnumerator LoadScenes()
	{
		Camera mainCamera = Camera.main;
		//skip bootstrap(the first scene), so start at 1
		for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			SceneManager.LoadScene(i,LoadSceneMode.Additive);
		}

		yield return null; //we need to wait a frame before removing cameras
		destroyOtherCameras(mainCamera.gameObject);
		Destroy(gameObject); //remove this gameobject when done
	}


	[ContextMenu("remove other cameras")]
	void destroyOtherCamerasTest()
	{
		Camera mainCamera = Camera.main;
		destroyOtherCameras(mainCamera.gameObject);
	}

	void destroyOtherCameras(GameObject keepMyCamera)
	{
		foreach(var cam in FindObjectsOfType<Camera>())
		{
			//Debug.Log("camera:" + cam.gameObject.name);
			if(cam.gameObject == keepMyCamera) continue;

			Destroy(cam.gameObject);
		}
	}

	//could add an onvaidate ifdefed so that it auto-saves the scene names in build settings, but that seems like overkill

}
