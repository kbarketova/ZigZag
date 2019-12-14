using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, IGravityChanges
{
    public float timeTillFall = 1;

    private Rigidbody _rb;
 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FallDown());
        }

    }

    public IEnumerator FallDown()
    {
        yield return new WaitForSecondsRealtime(timeTillFall);
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.useGravity = true;      
    }
}
