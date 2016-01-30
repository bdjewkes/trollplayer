using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;

public class StationCarousel : MonoBehaviour {

    //we'll change this to an array of 'Stations'
    public GameObject[] stations;
    public float arcPerStation;
    public int currentStationIndex = 0;

    public float rotationalVelocity;


    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            NextStation();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PreviousStation();
        }
    }




    [ContextMenu("TestNext")]
    public void NextStation()
    {
        //stations dont wrap
        if(currentStationIndex >= stations.Length - 1) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex + 1));
    }
    [ContextMenu("TestPrevious")]
    public void PreviousStation()
    {
        if (currentStationIndex < 0) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex - 1));

    }

    private bool animating; //animation lock
    private IEnumerator AnimateToCurrentIndex(int rotateToIndex)
    {
        if (animating) yield break;
        animating = true;
        

        //Slide the station your on back onto the carousel.
        //Rotate to the next station, at rotateToIndex
        //Slide the new station to the activ



        Quaternion startRotation = transform.localRotation;
        var startRotEuler = startRotation.eulerAngles;
        Quaternion finishRotation = Quaternion.Euler(new Vector3(startRotEuler.x, -(arcPerStation * rotateToIndex), startRotEuler.z));

        float startTime = Time.time;
        float normalizedTotalTime = (arcPerStation * Mathf.Abs(currentStationIndex - rotateToIndex)) / rotationalVelocity;
        while(Time.time < startTime + normalizedTotalTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, finishRotation, ((Time.time - startTime) / (normalizedTotalTime)));
            yield return null;
        }
        currentStationIndex = rotateToIndex;
        animating = false;
    }







}
