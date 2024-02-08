using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ProcesPostion : MonoBehaviour
{
    [SerializeField] SplineMarker splineMarker;
    [SerializeField] float scaleX=2048;

    [SerializeField] SplineExtrude splineExtrude;
 
   
    void Update()
    {
       splineMarker.SetNormalMarkerPosition(transform.position.x/scaleX);
    //    splineExtrude.Range=new Vector2(0,transform.position.x/scaleX);
    //    splineExtrude.Rebuild();
    }
}
