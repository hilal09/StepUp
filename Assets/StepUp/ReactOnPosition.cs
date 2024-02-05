using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactOnPosition : MonoBehaviour
{
    // Start
    SplineMarker splineMarker;
    bool isActivated=false;
    Animator animator;
    void Start()
    {
        splineMarker=GameObject.Find("MarkerPainter").transform.GetComponent<SplineMarker>();

        animator=transform.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (splineMarker.position>transform.position.x/750)
        {
         if (!isActivated) {
            animator.SetTrigger("ScaleIn");
            isActivated=true;
        }
        }

         if (splineMarker.position<transform.position.x/750)
        {
         if (isActivated) {
            animator.SetTrigger("ScaleOut");
            isActivated=false;
        }
        }
    }
}
