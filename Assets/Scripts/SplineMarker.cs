using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
//test
public class SplineMarker : MonoBehaviour
{
    private SplineAnimate splineAnimate;

    [SerializeField] SplineContainer splineContainer;
    Spline spline;
    [SerializeField] GameObject markerPrefab;

    bool isInitialized=false;
    
    void Start()
    {
        spline=splineContainer.Spline;
        splineAnimate=transform.GetComponent<SplineAnimate>();

        // foreach (BezierKnot bezierKnot in spline.Knots)
        // {
        //     GameObject.Instantiate(markerPrefab, new Vector3(bezierKnot.Position.x,bezierKnot.Position.y,bezierKnot.Position.z),bezierKnot.Rotation);
        // }
    }

    public void SetNormalMarkerPosition(float position)
    {
        splineAnimate.NormalizedTime=position;
    }

    private void OnTriggerEnter(Collider other) {
        
        
        //Switchrenderer(other);
        PlayAnimation(other);

    }

    private void Switchrenderer(Collider other)
    {
        SpriteRenderer renderer=other.transform.GetComponentInChildren<SpriteRenderer>();
        renderer.enabled=!renderer.enabled;
    }

    private void PlayAnimation(Collider other)
    {
        MarkerState state=other.transform.GetComponentInChildren<MarkerState>();
        
         Animator animator=other.transform.GetComponentInChildren<Animator>();
         if(state.isActivated)
         {
            animator.SetTrigger("ScaleOut");
            state.isActivated=false;
        }
        else{
            animator.SetTrigger("ScaleIn");
            state.isActivated=true;
        }
    }

}
