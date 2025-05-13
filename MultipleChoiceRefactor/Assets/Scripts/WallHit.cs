using UnityEngine;

public class WallHit : MonoBehaviour
{
    public Color color1 = Color.white;
    public Color color2 = Color.red;

    private bool isToggled = false;
    private Renderer rend;

    public int answerIndex; // Set in inspector or dynamically
    public QuizManager quizManager; // Assign this in inspector or dynamically

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = color1;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Visual feedback
            isToggled = !isToggled;
            rend.material.color = isToggled ? color2 : color1;

            // Call quiz manager to check the answer
            if (quizManager != null)
            {
                quizManager.CheckAnswer(answerIndex);
            }

            // Reset the throwable
            var resetScript = collision.gameObject.GetComponent<ThrowableReset>();
            if (resetScript != null)
            {
                resetScript.ResetPosition();
            }
        }
    }
}
