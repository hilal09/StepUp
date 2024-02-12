using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class TrackedPerson
{
    public int ID { get; set; }
    public Vector2 Center { get; set; }
    public int FramesNotDetected { get; set; }
    public int FramesDetected { get; set; }

    public Utils.Keypoint[] Keypoints { get; set; }
 }

 public class TrackedCenter
{
    public Vector2 Center { get; set; }

    public Utils.Keypoint[] Keypoints { get; set; }
 }


public class PersonTracker:MonoBehaviour
{
    [Range(1.0f, 1000.0f)]
    [SerializeField] float distanceThreshold=8;

    [Range(1, 50)]
    [SerializeField] int minFramesDetected=5;
    [Range(1, 50)]
    [SerializeField] int maxFramesNotDetected=5;
    
    [Range(1, 17)]
    [SerializeField] int minValidKeyPoints=5;

    [Range(0.0f, 1.0f)]
    [SerializeField] float minKeyPointScore=0.5f;


    private List<Utils.Keypoint[]>  lastFramePoses;
    private List<TrackedPerson> trackedPersons = new List<TrackedPerson>();
   //private List<TrackedPerson> personsDetectedOverXFrames = new List<TrackedPerson>();
    private int nextPersonID = 0;

    PoseEstimator poseEstimator;


    public List<TrackedPerson> GetAllTrackedPersons(){
        List<TrackedPerson> validPersons = new List<TrackedPerson>();

        foreach(TrackedPerson person in trackedPersons)
        {
            if(person.ID!=-1)
                validPersons.Add(person);
        }
        return validPersons;
    }

    void Start()
    {
        lastFramePoses=new List<Utils.Keypoint[]>();
        poseEstimator=transform.GetComponent<PoseEstimator>();

    }

    void Update()
    {
        Utils.Keypoint[][] currentFramePoses = poseEstimator.GetAllPoses();
        List<Utils.Keypoint[]> filteredPoses = new List<Utils.Keypoint[]>();

        //Personen mit wenig qualitativen Keypoits (Körperpunkten) herausfiltern.
        if(currentFramePoses!=null)
        {
            foreach (Utils.Keypoint[] personKeypoints in currentFramePoses)
            {

                int validPointsCounter=0;
                foreach(Utils.Keypoint keypoint in personKeypoints)
                {
                    if(keypoint.score>minKeyPointScore)
                    {
                        validPointsCounter++;
                    }
                }
                if (validPointsCounter >= minValidKeyPoints)
                {
                    filteredPoses.Add(personKeypoints);
                }
            }
        }
        List<TrackedCenter> currentFrameCenterPoints = new List<TrackedCenter>();

        //Für jede Person den Mittelpunkt bzw. Mittelwert aller Punkte berechnen
        if(filteredPoses.Count>0)
        {
            foreach (Utils.Keypoint[] pose in filteredPoses)
            {
                currentFrameCenterPoints.Add(new TrackedCenter{Center=GetCentroid(pose), Keypoints=pose});
            }
        }

        if (trackedPersons.Count>0)
        {
            trackedPersons = trackedPersons.OrderBy(p => p.FramesDetected).ToList(); //OrderByDescending
            
            //Schleife Rückwärts zählen damit beim verkleinern der Schleife RemoveAt(i) kein Fehler passiert
            for(int i = trackedPersons.Count - 1; i >= 0; i--)
            {
                var person = trackedPersons[i];
                int closestCentroidIndex = GetClosestCenterIndex(person.Center, currentFrameCenterPoints,distanceThreshold);
                if (closestCentroidIndex != -1)
                {
                    person.Center = currentFrameCenterPoints[closestCentroidIndex].Center;
                    person.Keypoints = currentFrameCenterPoints[closestCentroidIndex].Keypoints;
                    currentFrameCenterPoints.RemoveAt(closestCentroidIndex); //gefunden also entfernen
                    person.FramesNotDetected = 0;  // Person detektiert, counter zurück setzen
                    person.FramesDetected++;

                    if(person.FramesDetected >= minFramesDetected && person.ID == -1)  // ID erst setzen wenn mindestens x frames detektiert, sonst bleibt id=-1
                    {
                        person.ID = nextPersonID++; 
                    }
                }
                else
                {
                    person.FramesNotDetected++;  // Person wurde diesen Frame nicht detektiert
                    if (person.FramesNotDetected > maxFramesNotDetected)  
                    {
                        trackedPersons.RemoveAt(i);  // x Frames nicht detektiert, Person löschen
                    }
                }
            }
        }

        // Alle nicht gefundenen können neue Personen sein.
        foreach (var centroid in currentFrameCenterPoints)
        {
            trackedPersons.Add(new TrackedPerson { ID = -1, Center = centroid.Center, Keypoints=centroid.Keypoints, FramesDetected = 1 });
        }


        //personsDetectedOverXFrames = trackedPersons.Where(p => p.FramesDetected > minFramesDetected).ToList();

        //frame speichern
        //lastFramePoses = filteredPoses;
    }

    void OnDrawGizmos()
    {
        DebugDrawTrackedPersons();
    }

    void DebugDrawTrackedPersons()
    {
        foreach (var person in trackedPersons)
        {
           // Debug.Log(person.Centroid+" ID: "+person.ID.ToString());
            Gizmos.DrawSphere(person.Center, 5f);
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.cyan;  
            UnityEditor.Handles.Label(person.Center + Vector2.up * 0.5f, "ID: " + person.ID.ToString(), style);
        }
    }

    Vector2 GetCentroid(Utils.Keypoint[] keypoints)
    {
        Vector2 totalPosition = Vector2.zero;
        int count = 0;

        foreach (var keypoint in keypoints)
        {

            if (keypoint.score > minKeyPointScore) //hier eventuell eine kleinere Score zulassen
            {
                totalPosition += keypoint.position; 
                count++; 
            }
        }

        if (count > 0)
        {
            return (totalPosition / count)*poseEstimator.Scale; 
        }
        else
        {
            return Vector2.zero; 
        }

    }

 
    public int GetClosestCenterIndex(Vector2 referencePosition, List<TrackedCenter> centerPositions, float distanceThreshold)
    {
        if (centerPositions.Count == 0)  
        {
            return -1;
        }

        int closestIndex = 0;  
        float closestDistance = Vector2.Distance(referencePosition, centerPositions[0].Center);  

       
        for (int i = 1; i < centerPositions.Count; i++)
        {
            float currentDistance = Vector2.Distance(referencePosition, centerPositions[i].Center);
            if (currentDistance < closestDistance)  // Die kürzeste Distanz speichern
            {
                closestDistance = currentDistance;  
                closestIndex = i; 
            }
        }
        if(distanceThreshold<closestDistance) 
        {
            return -1;
        }

        return closestIndex; 
    }
}