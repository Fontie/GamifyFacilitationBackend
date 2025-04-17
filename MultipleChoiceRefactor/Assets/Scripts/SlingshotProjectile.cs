using System;
using UnityEngine;

public class SlingshotProjectile : MonoBehaviour
{
    public Transform slingshotAnchor;
    public float launchForceMultiplier = 1000f;

    public bool active = false; // Check if the projectile is pulled back
    private Rigidbody rb;

    private Vector3 startPos;
    public SlingshotVisual slingshotVisual; // for visuals

    private Transform holdingController = null;

    private float lastToggleTime = -Mathf.Infinity;
    public float toggleCooldown = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        rb.isKinematic = true; // Start frozen
    }

    void Update()
    {
        if (holdingController != null)
        {
            // Follow controller position (slingshot stretching)
            transform.position = holdingController.position;

            // On release (grip up)
            if (!IsGripHeld(holdingController))
            {
                Launch();
            }
        }
    }

    public void TryGrab(Transform controller)
    {
        if (Time.time - lastToggleTime < toggleCooldown)
            return;

        lastToggleTime = Time.time;
        active = true;
        holdingController = controller;
        rb.isKinematic = true;

        Debug.Log("Slingshot grabbed");
    }

    public void Launch()
    {
        if (!active) return;

        Vector3 pullVector = slingshotAnchor.position - transform.position;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(pullVector * launchForceMultiplier);

        if (slingshotVisual != null)
        {
            slingshotVisual.ResetSling();
        }

        holdingController = null;
        active = false;
    }

    public void ResetProjectile()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = startPos;
    }

    private bool IsGripHeld(Transform controller)
    {
        var input = Input.GetKeyDown(KeyCode.R);//controller.GetComponent<WebXRController>();
        return input;//input != null && input.GetButton(WebXRController.ButtonTypes.Grip);
    }
}
