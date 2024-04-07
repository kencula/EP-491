using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class BeamManagerL : MonoBehaviour
{
    CsoundUnity csound;
    public Autohand.Hand hand;

    //Respawn point of prefab
    [SerializeField] GameObject obj1;
    [SerializeField] GameObject obj2;

    // Start is called before the first frame update
    void Start()
    {
        csound = GetComponent<CsoundUnity>();

        //find reference to hand
        GameObject[] hands = GameObject.FindGameObjectsWithTag("Left Hand");
        hand = hands[0].GetComponent<Autohand.Hand>();
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


    }

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
        //Destroy(grab.gameObject.transform.parent.gameObject);
        //obj1.transform.position = Vector3.zero;
        //obj2.transform.position = Vector3.zero;
        //Instantiate(prefab, new Vector3(0, 0.6f, 0), Quaternion.identity);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Squeeze");
    }
    void OnUnsqueezed(Autohand.Hand hand, Grabbable grab)
    {
        //Called when the "Unsqueeze" event is called, this event is tied to a secondary controller input through the HandControllerLink component on the hand
        
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
