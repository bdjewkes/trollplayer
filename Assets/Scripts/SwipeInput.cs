using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;

public class SwipeInput : MonoBehaviour {
    public float minSwipeThreshold = 10;
    private static SwipeInput _instance;
    public static SwipeInput Instance
    {
        get
        {
            if (_instance == null) GetOrCreateInstance();
            return _instance;
        }
    }
    private static void GetOrCreateInstance()
    {
        _instance = FindObjectOfType<SwipeInput>();
        if (_instance == null)
        {
            var obj = new GameObject();
            _instance = obj.AddComponent<SwipeInput>();
            _instance.name = "SwipeInput";
        }
    }


    Vector3 lastMouse;
    void LateUpdate()
    {
        lastMouse = Input.mousePosition;
    }
   
    /// <summary>
    /// Returns true if a swipe occured. Positive direction
    /// indicates a positive x swipe, negative indicates 
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    public static bool GetSwipeDirection(ref bool direction)
    {
        var move = (Input.mousePosition - Instance.lastMouse);
        //Only swipe if the threshold has been overcome, and it is mostly in the 
        //x direction.
        if (move.magnitude < Instance.minSwipeThreshold && (Mathf.Abs(move.x) < Mathf.Abs(move.y)))
        {
            return false;
        }
        if (move.x > 1)
        {
            direction = true;
            return true;
        }
        else
        {
            direction = false;
            return true;
        }
    }




}
