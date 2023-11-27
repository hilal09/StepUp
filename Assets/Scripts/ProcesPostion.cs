using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class ProcessPosition : MonoBehaviour
{
    public CreateMarkerOnSpline createMarkerOnSpline;
    int xresolution=640;


    void Update()
    {
        createMarkerOnSpline.setSplinePosition(transform.position.x/xresolution);
    }
}
