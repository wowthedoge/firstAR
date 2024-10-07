using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;
using UnityEngine.Serialization;
using Unity.XR.CoreUtils;

public class PathDrawer : MonoBehaviour
{
    [SerializeField]
    public LineRenderer lineRenderer;
    [SerializeField]
    public XROrigin xrOrigin;
    private List<Vector3> positions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame  
    void Update()
    {
        Vector3 cameraPosition = xrOrigin.Camera.transform.position;
        Debug.Log("cameraPosition:" + cameraPosition);
        positions.Add(cameraPosition);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
