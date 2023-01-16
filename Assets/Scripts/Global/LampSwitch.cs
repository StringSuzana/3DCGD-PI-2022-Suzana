using Characters;
using UnityEngine;

public class LampSwitch : MonoBehaviour
{
    public GameObject LampOn;
    public GameObject LampOff;
    public float activationDistance = 7f;

    private void Update()
    {
        // Find all objects of type IPlayer within a certain distance
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, activationDistance);
        bool playerDetected = false;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<IPlayer>() != null)
            {
                playerDetected = true;
                break;
            }
        }

        // Enable or disable lamps based on player detection
        if (playerDetected)
        {
            LampOn.SetActive(true);
            LampOff.SetActive(false);
        }
        else
        {
            LampOn.SetActive(false);
            LampOff.SetActive(true);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}