using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;

//housekeeping class to group parameters in the inspector
[Serializable]
public class CarouselSettings 
{
    public float rotationalVelocity = 80; // degrees/second (I think)
    public float radiusInTime = 0.25f; // seconds
    public float radiusOutTime = 0.13f; // seconds 
    public float carouselRadius = 10; // units
    public float activeRadius = 5; //units
    public float arcPerStation = 30; //degrees
}

public class StationCarousel : MonoBehaviour {
    public Station[] stations;
    public GameObject substanceObject; //an object to represent the substance.
    public Transform substanceLocation; // this is where the substance lives while not at a station
    private Substance substance = new Substance(1); // this is the substance, duh

    public HumourDisplay vesselHumourDisplay;

    //Carousel animation members.
    [SerializeField] private CarouselSettings settings = new CarouselSettings();
    [SerializeField] int currentStationIndex = 0;
    private AnimationRunner animator = new AnimationRunner();

    public bool Locked {get; private set;} //animation lock

    void Awake()
    {
        //Enforce the settings.
        ResetCarousel();
    }
    
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
            if(tapTime < tapThreshold && move.magnitude <= minSwipeMagnitude)
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
                        PreviousStation();
                    }
                    else
                    {
                        NextStation();
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
            return Quaternion.Euler(new Vector3(transform.localRotation.x, -settings.arcPerStation * index, transform.localRotation.z));
    }
    private IEnumerator AnimateToIndex(int rotateToIndex)
    {
        if (Locked) yield break;
        Locked = true;

        //Rotation animation
        Quaternion finishRotation = GetStationRotation(rotateToIndex);
        float normalizedTotalTime = (settings.arcPerStation * Mathf.Abs(currentStationIndex - rotateToIndex)) / settings.rotationalVelocity;
        Action<float> Slerp = (t) => {
            iTween.RotateTo(this.gameObject, iTween.Hash("rotation", finishRotation.eulerAngles, "easeType", iTween.EaseType.easeInOutSine, "time", t));
        };
        
        //Current station lerps to carousel circumference
        StartCoroutine(animator.RunAnimation(settings.radiusOutTime, LerpStationToRadius(settings.carouselRadius)));
        //Carousel slerps to destination station
        yield return StartCoroutine(animator.RunAnimation(normalizedTotalTime,Slerp));
        currentStationIndex = rotateToIndex;
        //New station lerps to active distance
        yield return StartCoroutine(animator.RunAnimation(settings.radiusInTime, LerpStationToRadius(settings.activeRadius)));

        Locked = false;
    }
    private IEnumerator AnimateSelect()
    {
        if (Locked) yield break;
        Locked = true;

        var station = stations[currentStationIndex];
        //Animate into position.
        var startPosition = substanceLocation.transform.position;
        yield return StartCoroutine(animator.RunAnimation(0.25f, t =>
        {
            substanceObject.transform.localPosition = Vector3.Lerp(startPosition, station.substanceLocation.transform.position, t);
        }));
        //Do the thing.
		yield return StartCoroutine(stations[currentStationIndex].PerformAction(substance));

        UpdateVesselArt();

        //Animate out of position.
        yield return StartCoroutine(animator.RunAnimation(0.25f, t =>
        {
            substanceObject.transform.localPosition = Vector3.Lerp(station.substanceLocation.transform.position, substanceLocation.position, t);
        })); ;


        Locked = false;
    }

    void UpdateVesselArt() {
        vesselHumourDisplay.DisplaySubstance(substance);
    }

    private Action<float> LerpStationToRadius(float endRadius)
    {
        var station = stations[currentStationIndex];
        var startPosition = station.transform.localPosition; 
        var finalPosition = new Vector3(startPosition.x, startPosition.y, endRadius);
        Action<float> ReturnStation = (t) =>
        {
            iTween.MoveTo(station.gameObject, iTween.Hash("position", finalPosition, "time", t, "easeType", iTween.EaseType.easeOutQuint, "islocal", true));
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
            rotator.localRotation = Quaternion.Euler(new Vector3(rotator.localRotation.x, settings.arcPerStation * i, rotator.localRotation.z));
            station.transform.localPosition = new Vector3(0, 0, settings.carouselRadius);
        }
        stations[currentStationIndex].transform.localPosition = new Vector3(0, 0, settings.activeRadius);
        transform.localRotation = GetStationRotation(currentStationIndex);
    }
}
