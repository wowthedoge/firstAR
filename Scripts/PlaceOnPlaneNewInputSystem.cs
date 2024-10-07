using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

/// <summary>
/// For tutorial video, see my YouTube channel: <seealso href="https://www.youtube.com/@xiennastudio">YouTube channel</seealso>
/// How to use this script:
/// - Add ARPlaneManager to XROrigin GameObject.
/// - Add ARRaycastManager to XROrigin GameObject.
/// - Create Input Actions in Unity.
/// - Attach this script to XROrigin GameObject.
/// - Add the prefab that will be spawned to the <see cref="placedPrefab"/>
/// - Create a new input system called TouchControls that has the <Pointer>/press as the binding. 
/// 
/// Touch to place the <see cref="placedPrefab"/> object on the touch position.
/// Will only placed the object if the touch position is on detected trackables.
/// Move the existing spawned object on the touch position.
/// Using Unity new input system with the input system created in Unity.
/// </summary>
[HelpURL("https://youtu.be/HkNVp04GOEI")]
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlaneNewInputSystem : MonoBehaviour
{
    /// <summary>
    /// The prefab that will be instantiated on touch.
    /// </summary>
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject placedPrefab;

    /// <summary>
    /// The instantiated object.
    /// </summary>
    GameObject spawnedObject;

    /// <summary>
    /// The input touch control.
    /// </summary>
    TouchControls controls;

    /// <summary>
    /// If there is any touch input.
    /// </summary>
    bool isPressed;

    ARRaycastManager aRRaycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        
        controls = new TouchControls();
        // If there is existing touch input, change the isPressed to true.
        // If the touch input is stopped, change the isPressed to false.
        controls.control.touch.performed += _ => isPressed = true;
        controls.control.touch.canceled += _ => isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is any pointer device connected to the system.
        // Or if there is existing touch input.
        if (Pointer.current == null || isPressed == false)
            return;

        // Store the current touch position.
        var touchPosition = Pointer.current.position.ReadValue();

        // Check if the raycast hit any trackables.
        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first hit means the closest.
            var hitPose = hits[0].pose;

            // Check if there is already spawned object. If there is none, instantiated the prefab.
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                // Change the spawned object position and rotation to the touch position.
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;
            }

            // To make the spawned object always look at the camera. Delete if not needed.
            Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
            lookPos.y = 0;
            spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }

    private void OnEnable()
    {
        controls.control.Enable();
    }

    private void OnDisable()
    {
        controls.control.Disable();
    }
}
