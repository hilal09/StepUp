using System;
using UnityEngine;

public class MoveAlongSpline : MonoBehaviour
{
    public SplinePath splinePath;
    public float speed = 1f;
    public AnimationCurve easingCurve;

    private float progress = 0f;

    bool isforward=true;

    void Update()
    {
        if(isforward)
        {
            if(!moveForward(speed))
            {
                isforward=false;
            }
        }
        else
        {
            if(!moveBackward(speed))
            {
                isforward=true;
            }
        }
    }
    public void setSplinePosition(float splinePosition){
        progress=splinePosition;
        Vector3 position = splinePath.GetPoint(splinePosition);
        transform.position = position;
    }

    public bool moveForward(float _speed){
        bool movingForward=true;
        progress += Time.deltaTime * _speed;
        if (progress >= 1f)
        {
            progress = 1f;
            movingForward=false;
        }

        float easedProgress = easingCurve.Evaluate(progress);
        Vector3 position = splinePath.GetPoint(easedProgress);
        transform.position = position;

        return movingForward;
    }

    public bool moveBackward(float _speed){
        bool movingBackward=true;
        progress -= Time.deltaTime * _speed;
        if (progress <= 0f)
        {
            progress = 0f;
            movingBackward=false;
        }
        float easedProgress = easingCurve.Evaluate(progress);
        Vector3 position = splinePath.GetPoint(easedProgress);
        transform.position = position;

        return movingBackward;
    }

}
