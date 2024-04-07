using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    public GameObject sphere1;
    public GameObject sphere2;

    private LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Check if both sphere transforms are assigned
        if (sphere1 == null || sphere2 == null)
        {
            Debug.LogError("Objects Unassaigned: unable to render line");
            return;
        }

        // Set the number of vertices in the LineRenderer to 2 (start and end points)
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Update the positions of the LineRenderer's vertices to connect the two spheres
        lineRenderer.SetPosition(0, sphere1.transform.position);
        lineRenderer.SetPosition(1, sphere2.transform.position);
    }
}
