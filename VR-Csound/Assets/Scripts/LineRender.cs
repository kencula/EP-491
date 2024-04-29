using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using UnityEngine.XR;

public class LineRender : MonoBehaviour
{
    public GameObject sphere1;
    public GameObject sphere2;

    private LineRenderer lineRenderer;
    float distance;

    public float minDistance = 0f; // Minimum distance for green color
    public float maxDistance = 2f; // Maximum distance for red color

    // Particles
    [SerializeField] ParticleSystem particles;
    public int numberOfParticles = 10; // Number of particles to emit
    public float particleSpeed = 5f; // Speed of emitted particles
    bool particleEnabled = false;

    public Autohand.Hand hand;


    void Start()
    {
        //find reference to hand
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Right Hand");
        hand = hands[0].GetComponent<Autohand.Hand>();

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

        // Find distance between 2 points
        distance = Vector3.Distance(sphere1.transform.position, sphere2.transform.position);

        // Calculate t value for color interpolation
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

        // Interpolate color between green and red based on distance
        Color color = Color.Lerp(Color.green, Color.red, t);

        // Apply the color to the Line Renderer's material
        lineRenderer.material.SetColor("_EmissionColor", color);


        if (particleEnabled)
        {
            // Emit particles along the line
            for (int i = 0; i < numberOfParticles; i++)
            {
                // Calculate the position along the line
                float j = (float)i / (numberOfParticles - 1); // Normalize the index
                Vector3 position = Vector3.Lerp(sphere1.transform.position, sphere2.transform.position, j);

                // Convert world position to local position
                Vector3 localPosition = transform.InverseTransformPoint(position);

                // Emit a particle at the calculated position
                EmitParticle(position, color);
            }
        }
    }


    // Event listeners
    private void OnEnable()
    {
        hand.OnSqueezed += OnSqueezed;
        hand.OnUnsqueezed += OnUnsqueezed;
    }

    private void OnDisable()
    {
        hand.OnSqueezed -= OnSqueezed;
        hand.OnUnsqueezed -= OnUnsqueezed;
    }

    void OnSqueezed(Autohand.Hand hand, Grabbable grab)
    {
        particleEnabled = true;
    }

    void OnUnsqueezed(Autohand.Hand hand, Grabbable grab)
    {
        particleEnabled = false;
    }

    void EmitParticle(Vector3 position, Color color)
    {
        // Emit a particle at the specified position
        ParticleSystem.EmitParams emitParams = new()
        {
            position = position,
            applyShapeToPosition = true,
            startColor = color
            //velocity = (position - transform.position).normalized * particleSpeed
        };
        particles.Emit(emitParams, 1);
    }
}
