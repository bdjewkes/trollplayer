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

    public float minSwipeThreshold = 50;
    Vector3 mouseStart;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) mouseStart = Input.mousePosition;
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            Vector3 move = Input.mousePosition - mouseStart;
            if (Input.GetMouseButtonUp(0) 
                && move.magnitude > minSwipeThreshold 
                && (Mathf.Abs(move.x) > Mathf.Abs(move.y)))
            {
                if (move.x > 0)
                {
                    NextStation();
                }
                else
                {
                    PreviousStation();
                }
            }

        }
    }

    [ContextMenu("TestNext")]
    private void NextStation()
    {
        //stations dont wrap
        if(currentStationIndex >= stations.Length - 1) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex + 1));
    }
    [ContextMenu("TestPrevious")]
    private void PreviousStation()
    {
        if (currentStationIndex < 1) return;
        StartCoroutine(AnimateToCurrentIndex(currentStationIndex - 1));

    }




    private Quaternion GetStationRotation(int index)
    {
            return Quaternion.Euler(new Vector3(transform.localRotation.x, -arcPerStation * index, transform.localRotation.z));
    }
            
    private bool animating; //animation lock
    private IEnumerator AnimateToCurrentIndex(int rotateToIndex)
    {
        if (animating) yield break;
        animating = true;

        //Rotation animation
        Quaternion startRotation = transform.localRotation;
        var startRotEuler = startRotation.eulerAngles;
        Quaternion finishRotation = GetStationRotation(rotateToIndex);
        float normalizedTotalTime = (arcPerStation * Mathf.Abs(currentStationIndex - rotateToIndex)) / rotationalVelocity;
        Action<float> Slerp = (t) => {
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", finishRotation.eulerAngles, "easeType", iTween.EaseType.easeOutBounce, "time", t));
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
            iTween.MoveTo(station, iTween.Hash("position", finalPosition, "time", t, "easeType", iTween.EaseType.easeInOutBounce, "islocal", true));
        };
        return ReturnStation;
    }
    
    [ContextMenu("Reset")]
    private void ResetCarousel()
    {
        for(int i = 0; i < stations.Length; i++)
        {
            Debug.Log("Setting index " + i);
            var station = stations[i];
            var rotator = station.transform.parent;
            rotator.localRotation = Quaternion.Euler(new Vector3(rotator.localRotation.x, arcPerStation * i, rotator.localRotation.z));
            station.transform.localPosition = new Vector3(0, 0, carouselRadius);
        }
        stations[currentStationIndex].transform.localPosition = new Vector3(0, 0, activeRadius);
        transform.localRotation = GetStationRotation(currentStationIndex);
        
    }
    
}
