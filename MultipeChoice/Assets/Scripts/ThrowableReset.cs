using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ThrowableReset : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void ResetPosition()
    {
        StartCoroutine(ResetAfterRelease());
    }

    private IEnumerator ResetAfterRelease()
    {
        // Force release from hand
        if (grabInteractable.isSelected)
        {
            var interactor = grabInteractable.firstInteractorSelecting;
            grabInteractable.interactionManager.SelectExit(interactor, grabInteractable);
        }

        // Disable physics & interaction briefly
        grabInteractable.enabled = false;
        rb.isKinematic = true;

        // Wait one frame to fully release and detach
        yield return null;

        // Now reset transform
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Clear any motion
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Wait another frame just in case
        yield return null;

        // Reactivate physics and interaction
        rb.isKinematic = false;
        grabInteractable.enabled = true;
    }
}
