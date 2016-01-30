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
    public float radiusInTime;
    public float radiusOutTime;
    public float carouselRadius;
    public float activeRadius;


    AnimationRunner animator = new AnimationRunner();

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

        //Rotation animation
        Quaternion startRotation = transform.localRotation;
        var startRotEuler = startRotation.eulerAngles;
        Quaternion finishRotation = Quaternion.Euler(new Vector3(startRotEuler.x, -(arcPerStation * rotateToIndex), startRotEuler.z));
        float normalizedTotalTime = (arcPerStation * Mathf.Abs(currentStationIndex - rotateToIndex)) / rotationalVelocity;
        Action<float> Slerp = (t) => {
            transform.localRotation = Quaternion.Slerp(startRotation, finishRotation, t);
        };

        yield return StartCoroutine(animator.RunAnimation(radiusOutTime, LerpStationToRadius(carouselRadius)));
        yield return StartCoroutine(animator.RunAnimation(normalizedTotalTime,Slerp));
        currentStationIndex = rotateToIndex;
        yield return StartCoroutine(animator.RunAnimation(radiusInTime, LerpStationToRadius(activeRadius)));

        animating = false;
    }

    private Action<float> LerpStationToRadius(float endRadius)
    {
        var station = stations[currentStationIndex];
        var startPosition = station.transform.localPosition; 
        var finalPosition = new Vector3(startPosition.x, startPosition.y, endRadius);

        var stationToReturn = stations[currentStationIndex];
        Action<float> ReturnStation = (t) =>
        {
            stationToReturn.transform.localPosition = Vector3.Lerp(startPosition, finalPosition, t);
        };
        return ReturnStation;
    }
    
}
