using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceHoop : MonoBehaviour
{
    [SerializeField]
    GameObject m_HoopPrefab;
    [SerializeField]
    GameObject m_BallPrefab;

    public GameObject placedHoop
    {
        get { return m_HoopPrefab; }
        set { m_HoopPrefab = value; }
    }

    public GameObject spawnedHoop { get; private set; }

    public GameObject placedBall
    {
        get { return m_BallPrefab; }
        set { m_BallPrefab = value; }
    }

    public GameObject spawnedBall { get; private set; }
    private bool PoseIsValid = false;

    public static event Action onPlacedObject;

    private bool isPlaced = false;
    
    ARRaycastManager m_RaycastManager;
    public GameObject indicator;
    private Pose PlacementPose;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (GameOver_hoop.gameStarted == true)
        {
            if (isPlaced == false)
            {
                UpdatePose();
                UpdateIndicator();
            }
            else
            {
                indicator.SetActive(false);
                return;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = s_Hits[0].pose;

                        spawnedHoop = Instantiate(m_HoopPrefab, hitPose.position, Quaternion.AngleAxis(180, Vector3.up));
                        spawnedHoop.transform.parent = transform.parent;

                        isPlaced = true;
                        if (onPlacedObject != null)
                        {
                            onPlacedObject();
                        }
                    }
                }
            }
        }
        
    }

    void UpdatePose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        m_RaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        PoseIsValid = hits.Count > 0;
        if (PoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }

    void UpdateIndicator()
    {
        if (PoseIsValid)
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
