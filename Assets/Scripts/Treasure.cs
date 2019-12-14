using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IGroundLayerInteraction, IGravityChanges
{
    [Header("Settings")]
    public int value = 1;
    public float timeTillFall = 1;
    public LayerMask ground;

    private Rigidbody _rb;

    private void Update()
    {
        CheckGroundLayer(transform, ground);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.Collect(value);
            Destroy(gameObject);
        }
    }

    public void CheckGroundLayer(Transform current, LayerMask ground)
    {
        var isGrounded = Physics.Raycast(current.position, Vector3.down, 1.0f, ground);
        if (!isGrounded)
        {
            StartCoroutine(FallDown());
        }
    }

    public IEnumerator FallDown()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.useGravity = true;

        yield return new WaitForSecondsRealtime(timeTillFall);
        Destroy(gameObject);
    }
}
