using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcesPostion : MonoBehaviour
{
    public CreateMarkerOnSpline createMarkerOnSpline;
    int xresolution=640;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        createMarkerOnSpline.setSplinePosition(transform.position.x/xresolution);
    }
}
