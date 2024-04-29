using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class BeamManager : MonoBehaviour
{
    CsoundUnity csound;
    public Autohand.Hand hand;

    bool noteOn = false;

    // Reference to flashlights
    [SerializeField] GameObject obj1;
    [SerializeField] GameObject obj2;
    [SerializeField] GameObject crystal;

    // Options
    [SerializeField] float maxPitchDistance = 4.0f;
    [SerializeField] float maxModDistance = 2f;

    // Midi Note Display
    [SerializeField] TextMeshProUGUI text;

    // Debug
    [SerializeField] int midiNote;
    [SerializeField] float freq;

    // Beam Color
    /*public LineRenderer lineRenderer;
    private Color initialColor;
    private Color stretchedColor = Color.white;*/

    // Start is called before the first frame update
    void Start()
    {
        csound = GetComponent<CsoundUnity>();

        //find reference to hand
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Right Hand");
        hand = hands[0].GetComponent<Autohand.Hand>();

        // Beam Color Init
        // Get the initial color of the material
        //initialColor = lineRenderer.material.color;
    }

    private void OnEnable()
    {
        hand.OnTriggerGrab += OnTriggerGrab;
        hand.OnTriggerRelease += OnTriggerRelease;
        hand.OnSqueezed += OnSqueezed;
        hand.OnUnsqueezed += OnUnsqueezed;
        hand.OnBeforeGrabbed += OnBeforeGrabbed;
        hand.OnGrabbed += OnGrabbed;
        hand.OnReleased += OnReleased;
    }

    private void OnDisable()
    {
        hand.OnTriggerGrab -= OnTriggerGrab;
        hand.OnTriggerRelease -= OnTriggerRelease;
        hand.OnSqueezed -= OnSqueezed;
        hand.OnUnsqueezed -= OnUnsqueezed;
        hand.OnBeforeGrabbed -= OnBeforeGrabbed;
        hand.OnGrabbed -= OnGrabbed;
        hand.OnReleased -= OnReleased;
    }

    // Update is called once per frame
    void Update()
    {
        if (!csound.IsInitialized) return;

        //Get distance between the flashlights
        float distance = Vector3.Distance(obj1.transform.position, obj2.transform.position);
        float convertedDistance = ConvertRange(0f, maxModDistance, 0f, 1f, distance);
        csound.SetChannel("distance", convertedDistance);

        // get distance between right flashlight and crystal
        float distance2 = Vector3.Distance(obj1.transform.position, crystal.transform.position);
        // convert distance into midi note (4 octaves)
        midiNote = (int)(48 * ConvertRange(0f, maxPitchDistance, 0f, 1f, distance2)) + 36;
        text.text = MidiToNoteString(midiNote);

        // convert midi note to frequency and send to csound
        freq = MidiToFrequency(midiNote);
        csound.SetChannel("freq", freq);


        // Beam Color
        // Interpolate between the initial color and white based on the input float
        //Color targetColor = Color.Lerp(initialColor, stretchedColor, convertedDistance);

        // Apply the new color to the material
        //lineRenderer.material.color = Color.white;
    }

    // Method to convert MIDI number to note string
    public string MidiToNoteString(int midiNumber)
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        int noteIndex = midiNumber % 12;
        int octave = midiNumber / 12 - 1;

        string noteName = noteNames[noteIndex];
        return noteName + octave.ToString();
    }

    float ConvertRange(
    float originalStart, float originalEnd, // original range
    float newStart, float newEnd, // desired range
    float value) // value to convert
    {
        float scale = (newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }

        float MidiToFrequency(int midiNote)
    {
        return Mathf.Pow(2f, (midiNote - 69) / 12f) * 440f;
    }

    // Autohand Events...
    public void OnTriggerGrab(Autohand.Hand hand, Grabbable grab)
    {
        //hand.Release();
        Debug.Log("Trigger");
    }
    public void OnTriggerRelease(Autohand.Hand hand, Grabbable grab)
    {
        
    }
    void OnSqueezed(Autohand.Hand hand, Grabbable grab)
    {
        //Called when the "Squeeze" event is called, this event is tied to a secondary controller input through the HandControllerLink component on the hand
        csound.SetChannel("noteon", 1);
        noteOn = true;
        Debug.Log("Squeeze");
    }
    void OnUnsqueezed(Autohand.Hand hand, Grabbable grab)
    {
        //Called when the "Unsqueeze" event is called, this event is tied to a secondary controller input through the HandControllerLink component on the hand
        csound.SetChannel("noteon", 0);
        noteOn = false;
    }
    void OnBeforeGrabbed(Autohand.Hand hand, Grabbable grab)
    {

    }

    void OnGrabbed(Autohand.Hand hand, Grabbable grab)
    {
    }
    void OnReleased(Autohand.Hand hand, Grabbable grab)
    {
    }
}
