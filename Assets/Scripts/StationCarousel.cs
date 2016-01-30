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

    public void NextStation()
    {
        //stations dont wrap
        if(currentStationIndex < stations.Length) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex + 1));
    }
    public void PreviousStation()
    {
        if (currentStationIndex > 0) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex - 1));

    }
    private IEnumerator AnimateToCurrentIndex(int rotateToIndex)
    {
        Quaternion startRotation = transform.localRotation;
        var startRotEuler = startRotation.eulerAngles;
        Quaternion finishRotation = Quaternion.EulerAngles(new Vector3(startRotEuler.x, arcPerStation * currentStationIndex, startRotEuler.z));

        yield return null;
        float totalTime = rotationalVelocity / (arcPerStation * currentStationIndex - rotateToIndex);

            
/*        
        while()
        {
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation, target))
            yield return null;

        }*/ 
    }







}
