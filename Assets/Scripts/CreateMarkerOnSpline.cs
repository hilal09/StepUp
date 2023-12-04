using System;
using UnityEngine;

public class CreateMarkerOnSpline : MonoBehaviour
{
    public SplinePath splinePath;
    public float speed = 1f;
    public GameObject marker=null;
    private float progress = 0f;
    GameObject markerSet;
    public Sprite yourFootstepSprite;
   
   void Update()
   {
    createMarker(progress);
   }
    public void setSplinePosition(float splinePosition){
        
        progress=splinePosition;
        Vector3 position = splinePath.GetPoint(splinePosition);

        transform.SetPositionAndRotation(position,Quaternion.identity);
    }

    private void createMarker(float position)
    {
        if(position>=0.5 && !markerSet)
        {
            Debug.Log("CREATE MARKER");
            markerSet= GameObject.Instantiate(marker,transform.position, Quaternion.identity);
           // Attach the footstep image to the markerSet GameObject
            markerSet.GetComponent<SpriteRenderer>().sprite = yourFootstepSprite;
        }

        if(position<0.4)
        {
                        
            if(markerSet){
                Debug.Log("DESTROY MARKER");
                Destroy(markerSet);
                markerSet=null;
            }
        }
    }
    

}
