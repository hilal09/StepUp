using UnityEngine;

public class SplinePath : MonoBehaviour
{
    public Vector3[] controlPoints = new Vector3[]
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 1, 0),
        new Vector3(2, 1, 0),
        new Vector3(3, 0, 0)
    };

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = 0;//controlPoints.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t);// * (controlPoints.Length - 3);
            i = (int)t;
            t -= i;
        }
        return transform.TransformPoint(Bezier.GetPoint(controlPoints[i], controlPoints[i + 1], controlPoints[i + 2], controlPoints[i + 3], t));
    }
}

public static class Bezier
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}
