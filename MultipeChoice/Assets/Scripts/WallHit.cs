using UnityEngine;

public class WallHit : MonoBehaviour
{
    public Color color1 = Color.white;
    public Color color2 = Color.red;

    private bool isToggled = false;
    private Renderer rend;

    public ThrowableReset throwable; // Assign in inspector

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color1;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object hitting it is the throwable
        if (collision.gameObject.CompareTag("Throwable"))
        {
            // Toggle the color
            isToggled = !isToggled;
            rend.material.color = isToggled ? color2 : color1;

            // Reset the throwable
            var resetScript = collision.gameObject.GetComponent<ThrowableReset>();
            if (resetScript != null)
            {
                resetScript.ResetPosition();
            }
        }
    }
}