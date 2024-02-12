using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ProcessPersons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]PersonTracker personTracker;
    List<TrackedPerson> trackedPersons;

    [SerializeField] SplineMarker splineMarker;
    [SerializeField] float scaleX=2048;
    void Start()
    {
        trackedPersons=new List<TrackedPerson>();
    }

    // Update is called once per frame
    void Update()
    {
        trackedPersons=personTracker.GetAllTrackedPersons();
        
        if(trackedPersons.Count>0)
        {
Debug.Log(trackedPersons[0].Center);
splineMarker.SetNormalMarkerPosition(trackedPersons[0].Center.x/scaleX);
            /*
            Debug.Log("Found:"+ trackedPersons.Count);
            foreach(TrackedPerson person in trackedPersons)
            {
                Debug.Log("ID:"+person.ID+" center:"+person.Center+ "keypoints:"+person.Keypoints.Length);
            }
            */
        }
    }
}
