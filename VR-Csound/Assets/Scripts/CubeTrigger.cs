using UnityEngine;
using Autohand;

public class CubeTrigger : MonoBehaviour
{
    //Respawn point of prefab
    [SerializeField] GameObject spawnpoint;
    [SerializeField] GameObject prefab;

    // Function to release the object when the hand enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a hand
        if (other.CompareTag("Flashlight"))
        {
            Debug.Log("Dropping");
            // Release the object held by the hand
            Destroy(other.gameObject.transform.parent.gameObject);
            Instantiate(prefab,spawnpoint.transform);
        }
    }
}
