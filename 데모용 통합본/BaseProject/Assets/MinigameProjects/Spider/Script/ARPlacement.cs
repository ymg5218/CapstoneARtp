using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ARPlacement : MonoBehaviour
{
    public GameObject arObjectToSpawn;
    public GameObject indicator;
    public GameObject shoot;
    
    private GameObject SpawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager raycastManager;
    private bool PoseIsValid = false;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        shoot.SetActive(false);
    }

    void Update()
    {
        if(GameOver_spider.gameStarted == true)
        { 
        if(SpawnedObject == null && PoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ARPlaceObject();
            shoot.SetActive(true);
        }
        UpdatePose();
        UpdateIndicator();
        }
    }

    void ARPlaceObject()
    {
        SpawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
    }
 
    void UpdatePose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        PoseIsValid = hits.Count > 0;
        if(PoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void UpdateIndicator()
    {
        if (SpawnedObject == null && PoseIsValid)
        {
            indicator.SetActive(true);
            indicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            indicator.SetActive(false);
        }
    }

}
