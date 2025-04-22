using System;
using UnityEngine;


public class SlingshotProjectile : MonoBehaviour
{
    public Transform slingshotAnchor;
    public float launchForceMultiplier = 1000f;

    private bool grab;
    public Boolean active = false; //Check if the projectile is pulled back
    private Rigidbody rb;

    private Vector3 startPos;
    public SlingshotVisual slingshotVisual;//for visuals

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;

        rb.isKinematic = true; // Start frozen



    }

    public void IAmMakingThisFunctionVerySpecificSoThatSendMessageCanFindItEasier()
    {
        Debug.Log("I reached the other object, HUZAAA!!!");
        Launch();
    }

    private float lastToggleTime = -Mathf.Infinity; // Tracks when the function was last run,
                                                    // point of this is that the slingshot doesnt catch the ball again after shooting
    public float toggleCooldown = 2f;
    public void SetActive()
    {
        // Check if enough time has passed
        if (Time.time - lastToggleTime < toggleCooldown)
        {
            return; // Not enough time passed � ignore call
        }

        lastToggleTime = Time.time; // Update last called time

        // Toggle logic
        if (active)
        {
            Debug.Log("active: FALSE");
            active = false;
        }
        else
        {
            Debug.Log("active: TRUE");
            active = true;
        }

    }

    void Launch()
    {
        Vector3 pullVector = slingshotAnchor.position - transform.position;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(pullVector * launchForceMultiplier);

        if (slingshotVisual != null)
        {
            slingshotVisual.ResetSling();
            SetActive();
        }
    }

    public void ResetProjectile()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.position = startPos;
    }
}
