using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;

public class StationCarousel : MonoBehaviour {
    public Substance substance; // this is the substance, duh


    public Station[] stations;
    public float arcPerStation;
    public int currentStationIndex = 0;

    //Carousel animation members.
    public float rotationalVelocity;
    public float radiusInTime;
    public float radiusOutTime;
    public float carouselRadius;
    public float activeRadius;
    AnimationRunner animator = new AnimationRunner();

    //Input timing members
    private float tapThreshold = 0.25f;
    private float minSwipeMagnitude = 50;
    Vector3 mouseStart;
    private float tapTime;
    void Update()
    {
        tapTime += Time.deltaTime;
        Vector3 move = Input.mousePosition - mouseStart;
            
        if (Input.GetMouseButtonDown(0)) {
            mouseStart = Input.mousePosition;
            tapTime = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
                
            //Check if its a tap.
            if(tapTime < tapThreshold)
            {
                SelectStation(); 
            }
            //Check if its a swipe
            else if(move.magnitude > minSwipeMagnitude)
            {
                //Horizontal swipe:
                if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
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
                //Vertical swipe:
                else
                {
                    if (move.y > 0)
                    {
                        JumpToMeasureStation();//Swipe up;
                    }
                    else { JumpToReturnIndex(); } 
                } 
            }
        }
    }

    [ContextMenu("TestNext")]
    private void NextStation()
    {
        //stations dont wrap
        if(currentStationIndex >= stations.Length - 1) return;
        returnToIndex = currentStationIndex;
        StartCoroutine(AnimateToIndex(currentStationIndex + 1));
    }
    [ContextMenu("TestPrevious")]
    private void PreviousStation()
    {
        if (currentStationIndex < 1) return;
        returnToIndex = currentStationIndex;
        StartCoroutine(AnimateToIndex(currentStationIndex - 1));
    }

    private int returnToIndex;
    private void JumpToMeasureStation()
    {
        //Measure index is implemented as a magic number...
        var measureIndex = stations.Length - 2;
        if(currentStationIndex != measureIndex) returnToIndex = currentStationIndex;
        StartCoroutine(AnimateToIndex(measureIndex));
    }
    private void JumpToReturnIndex()
    {
        StartCoroutine(AnimateToIndex(returnToIndex));
    }
    private void SelectStation()
    {
        StartCoroutine(AnimateSelect());
    }

    private Quaternion GetStationRotation(int index)
    {
            return Quaternion.Euler(new Vector3(transform.localRotation.x, -arcPerStation * index, transform.localRotation.z));
    }
            
    private bool animating; //animation lock
    private IEnumerator AnimateToIndex(int rotateToIndex)
    {
        if (animating) yield break;
        animating = true;

        //Rotation animation
        Quaternion finishRotation = GetStationRotation(rotateToIndex);
        float normalizedTotalTime = (arcPerStation * Mathf.Abs(currentStationIndex - rotateToIndex)) / rotationalVelocity;
        Action<float> Slerp = (t) => {
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", finishRotation.eulerAngles, "easeType", iTween.EaseType.easeOutBounce, "time", t));
        };
        
        //Current station lerps to carousel circumference
        yield return StartCoroutine(animator.RunAnimation(radiusOutTime, LerpStationToRadius(carouselRadius)));
        //Carousel slerps to destination station
        yield return StartCoroutine(animator.RunAnimation(normalizedTotalTime,Slerp));
        currentStationIndex = rotateToIndex;
        //New station lerps to active distance
        yield return StartCoroutine(animator.RunAnimation(radiusInTime, LerpStationToRadius(activeRadius)));

        animating = false;
    }

    private IEnumerator AnimateSelect()
    {
        if (animating) yield break;
        animating = true;


        //Animate into position.
        //Do the thing.
        stations[currentStationIndex].PerformAction(substance);
        //Animate out of position.


        animating = false;
    }



    private Action<float> LerpStationToRadius(float endRadius)
    {
        var station = stations[currentStationIndex];
        var startPosition = station.transform.localPosition; 
        var finalPosition = new Vector3(startPosition.x, startPosition.y, endRadius);
        Action<float> ReturnStation = (t) =>
        {
            iTween.MoveTo(station.gameObject, iTween.Hash("position", finalPosition, "time", t, "easeType", iTween.EaseType.easeInOutBounce, "islocal", true));
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
