using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcesPostion : MonoBehaviour
{
    [SerializeField] SplineMarker splineMarker;
    [SerializeField] float scaleX=1920;
 
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       splineMarker.SetNormalMarkerPosition(transform.position.x/scaleX);
    }
}
