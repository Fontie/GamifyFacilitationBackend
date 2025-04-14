using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SlingshotProjectile : MonoBehaviour
{
    public Transform slingshotAnchor;
    public float launchForceMultiplier = 500f;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
        startPos = transform.position;

        rb.isKinematic = true; // Start frozen

        grab.selectExited.AddListener(Launch);
    }

    void Launch(SelectExitEventArgs args)
    {
        Vector3 pullVector = slingshotAnchor.position - transform.position;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(pullVector * launchForceMultiplier);
    }

    public void ResetProjectile()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = startPos;
    }
}
