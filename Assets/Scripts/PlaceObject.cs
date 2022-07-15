using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToPlace;
    private ARSessionOrigin sessionOrigin;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits;
    private ARAnchorManager anchorManager;
    private List<ARAnchor> anchors;
    private Pose placementPose;
    private bool poseIsValid = false;
    public InputAction tapAction;
    public InputAction touchAction;
    private Vector2 touchPosition = new Vector2(0.5f * Screen.width, 0.5f * Screen.height);
    

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        anchorManager = FindObjectOfType<ARAnchorManager>();
        tapAction.Enable();
        touchAction.Enable();
        tapAction.performed += SpawnObject;
        touchAction.performed += TouchPerformed;
        hits = new List<ARRaycastHit>();
        anchors = new List<ARAnchor>();
    }

    private void OnDestroy()
    {
        tapAction.performed -= SpawnObject;
        touchAction.performed -= TouchPerformed;
        tapAction.Disable();
        touchAction.Disable();
        anchors.Clear();
    }

    private void Update()
    {
        if (raycastManager == null)
            return;

        UpdatePlacementPose();
    }

    private void TouchPerformed(InputAction.CallbackContext context) 
    {
        touchPosition = context.ReadValue<Vector2>();
    }

    private void SpawnObject(InputAction.CallbackContext context)
    {
        if (hits[0] == null)
        {
            return;
        }

        GameObject newObj = Instantiate(objectToPlace);
        newObj.transform.position = placementPose.position;
        newObj.transform.rotation = placementPose.rotation;
        MeshRenderer renderer = newObj.GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            UnityEngine.Random.InitState(DateTime.UtcNow.Millisecond);
            renderer.material.color = UnityEngine.Random.ColorHSV();
        }
        ARPlane plane = hits[0].trackable.GetComponent<ARPlane>();
        ARAnchor anchor = anchorManager.AttachAnchor(plane, placementPose);
        anchors.Add(anchor);
    }

    void UpdatePlacementPose()
    {
        poseIsValid = raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
        Vector3 cameraForward = sessionOrigin.camera.transform.forward;
        
        if (poseIsValid)
        {
            placementPose.position = hits[0].pose.position;
        }
        else 
        {
            placementPose.position = sessionOrigin.transform.position + (cameraForward * 0.4f);
        }
        Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        placementPose.rotation = Quaternion.LookRotation(cameraBearing);
    }
}
