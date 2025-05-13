using UnityEngine;
using System.Collections;

public class ThrowableReset : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    // Reference to the current controller holding this object
    private Transform currentHolder;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        // Optional debug key
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    public void AttachToHand(Transform controller)
    {
        currentHolder = controller;
        rb.isKinematic = true;
        transform.SetParent(controller);
    }

    public void DetachFromHand()
    {
        transform.SetParent(null);
        rb.isKinematic = false;

        // Optionally add velocity here from controller
        // rb.velocity = controller velocity
        // rb.angularVelocity = controller angular velocity

        currentHolder = null;
    }



    public void ResetPosition()
    {
        StartCoroutine(ResetAfterRelease());
    }

    private IEnumerator ResetAfterRelease()
    {
        // If being held, release it
        if (currentHolder != null)
        {
            DetachFromHand();
        }

        rb.isKinematic = true;

        yield return null;

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        yield return null;

        rb.isKinematic = false;
    }
}
