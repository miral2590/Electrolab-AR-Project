using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectPlacementController : MonoBehaviour
{
    public GameObject placementIndictor;
    public GameObject spawnObject;

  
    public GameObject machine, cor;

    public GameObject[] uiElements;

    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;

    private float rotationSpeed;

    private bool isPlacementPoseValid, canRotate, isObjectPlaced;

    private static ObjectPlacementController instance;

    public static ObjectPlacementController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ObjectPlacementController>();
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        canRotate = isObjectPlaced = false;
        placementIndictor.SetActive(false);
        spawnObject.SetActive(false);
        aRRaycastManager = transform.GetComponent<ARRaycastManager>();

        rotationSpeed = 10f;

        foreach(GameObject go in uiElements)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isObjectPlaced)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (isPlacementPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                placementIndictor.SetActive(false);
                spawnObject.SetActive(true);
                spawnObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
                isObjectPlaced = true;

                foreach (GameObject go in uiElements)
                {
                    go.SetActive(true);
                }
            }

            if (isPlacementPoseValid && canRotate)
            {
                machine.transform.RotateAround(cor.transform.position, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        isPlacementPoseValid = hits.Count > 0;

        if(isPlacementPoseValid)
        {
            placementPose = hits[0].pose;

            var cameraFarward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraFarward.x, 0, cameraFarward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if(isPlacementPoseValid)
        {
            placementIndictor.SetActive(true);

            placementIndictor.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndictor.SetActive(false);
        }
    }

    public void ControlRotation()
    {
        canRotate = !canRotate;
        // false  = !false (true)
        // 2nd time

        // true = !true (false)
    }
}
