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

    private int objectCount;
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
        objectCount = 0;

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

        /*
         * 
         * if(objectCount <= 2)
         * {
         *      1st object => Same as before
         *      2nd object => if objectCount == 1
         *      {
         *           Show UI panel
         *           -> Buttons -> Table(Value 1)
         *           Table.SetActive(true)
         *      }
         *      
         *      3rd object => if objectCount == 2
         *      {
         *           Show UI panel
         *           -> Buttons -> Machine(Value 2)
         *           Machine.SetActive(true)
         *      }
         * }
         * 
         * 
         *  -----------
         *      -----
         *      -----
         *      -----
         * 
         * 
        */




        if (objectCount <= 2)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();

            if (isPlacementPoseValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                /*placementIndictor.SetActive(false);
                spawnObject.SetActive(true);
                spawnObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

                objectCount++;
                isObjectPlaced = true;

                foreach (GameObject go in uiElements)
                {
                    go.SetActive(true);
                }*/

               
               /* UiPanel.SetActive(true);*/
            }

            

            if (isPlacementPoseValid && canRotate)
            {
                machine.transform.RotateAround(cor.transform.position, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void SpawnObject(int _index) // _index = 0
    {
       /* 3dObject[_index].SetActive(true);
        3dObject[_index].transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

        objectCount++;

        UiPanel.SetActive(false);*/
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
