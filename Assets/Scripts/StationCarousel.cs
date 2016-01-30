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
    private int currentStationIndex = 0;

    public float rotationalVelocity;

    [ContextMenu("TestNext")]
    public void NextStation()
    {
        //stations dont wrap
        if(currentStationIndex < stations.Length) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex + 1));
    }
    [ContextMenu("TestPrevious")]
    public void PreviousStation()
    {
        if (currentStationIndex > 0) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex - 1));

    }
    private IEnumerator AnimateToCurrentIndex(int rotateToIndex)
    {
        //Slide the station your on back onto the carousel.
        //Rotate to the next station, at rotateToIndex
        //Slide the new station to the activ

        Quaternion startRotation = transform.localRotation;
        var startRotEuler = startRotation.eulerAngles;
        Quaternion finishRotation = Quaternion.EulerAngles(new Vector3(startRotEuler.x, arcPerStation * currentStationIndex, startRotEuler.z));

        float startTime = Time.time;
        float totalTime = rotationalVelocity / (arcPerStation * currentStationIndex - rotateToIndex);
        while(Time.time < startTime + totalTime)
        {
            transform.rotation = Quaternion.Slerp(startRotation, finishRotation, (Time.time / (startTime + totalTime)));
            yield return null;
        } 
    }







}
