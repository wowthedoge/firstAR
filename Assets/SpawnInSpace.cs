using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.XR.CoreUtils;

public class SpawnInSpace : MonoBehaviour
{

    [SerializeField]
    public GameObject spherePrefab;  // Reference to the sphere prefab
    [SerializeField]
    public XROrigin xrOrigin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceSphereAtCameraPosition();
        }
    }

    private void PlaceSphereAtCameraPosition()
    {
        // Get the camera's position in AR space
        Vector3 cameraPosition = xrOrigin.Camera.transform.position;

        // Instantiate the sphere at the camera's position
        Instantiate(spherePrefab, cameraPosition, Quaternion.identity);
    }
}
