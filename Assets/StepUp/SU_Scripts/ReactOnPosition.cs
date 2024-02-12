using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactOnPosition : MonoBehaviour
{
    SplineMarker splineMarker;
    bool isActivated=false;
    Animator animator;

    float threshold = 0.1f; 
    void Start()
    {
        splineMarker=GameObject.Find("MarkerPainter").transform.GetComponent<SplineMarker>();

        animator=transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {

        if (splineMarker.position >= transform.position.x / 750 + threshold)
        {
            if (!isActivated)
            {
                animator.SetTrigger("ScaleIn");
                isActivated = true;
            }
        }

        if (splineMarker.position < transform.position.x / 750 - threshold)
        {
            if (isActivated)
            {
                animator.SetTrigger("ScaleOut");
                isActivated = false;
            }
        }

    }
}
