using UnityEngine;

public class VRButtonAction : MonoBehaviour
{
    public ThrowableReset throwable;

    public void objectHasBeenTouchedByVRHand()
    {
        ResetThrow();
    }
    public void ResetThrow()
    {
        Debug.Log("VR Button Pressed!");
        if (throwable != null)
        {
            throwable.ResetPosition();
        }
    }
}