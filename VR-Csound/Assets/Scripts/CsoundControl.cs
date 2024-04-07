using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsoundControl : MonoBehaviour
{

    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    [SerializeField] GameObject object3;
    CsoundUnity csound;

    private Vector3 obj1Location;
    private Vector3 obj2Location;
    private Vector3 obj3Location;
    void Start()
    {
        csound = GetComponent<CsoundUnity>();
    }

    void Update()
    {
        if (!csound.IsInitialized) return;
        // your code

        obj1Location = object1.transform.position;
        csound.SetChannel("freq", ConvertRange(-1.5f, 1.5f, 20, 500, obj1Location.x));
        csound.SetChannel("mod", ConvertRange(-1.5f, 1.5f, 0, 10, obj1Location.z));
        csound.SetChannel("index", ConvertRange(0, 2, 0, 20, obj1Location.y));

        obj2Location = object2.transform.position;
        csound.SetChannel("feedback", ConvertRange(-1.5f, 1.5f, 0, 1, obj2Location.x));
        csound.SetChannel("verb", ConvertRange(-1.5f, 1.5f, 0.5f, 1, obj2Location.z));
        csound.SetChannel("vib", ConvertRange(0, 1.5f, 0, 10, obj2Location.y));

        obj3Location = object3.transform.position; 
        csound.SetChannel("filterFreq", ConvertRange(-1.5f, 1.5f, 5000, 10000, obj3Location.x));
        csound.SetChannel("reson", ConvertRange(-1.5f, 1.5f, 0, 1, obj3Location.z));
        csound.SetChannel("dist", ConvertRange(0, 2, 0, 5, obj3Location.y));
    }
    public static float ConvertRange(
    float originalStart, float originalEnd, // original range
    float newStart, float newEnd, // desired range
    float value) // value to convert
    {
        float scale = (newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }
}
