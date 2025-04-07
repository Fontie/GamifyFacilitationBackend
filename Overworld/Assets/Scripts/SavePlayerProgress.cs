using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.InteropServices;


public class SavePlayerProgressScript
{
    [DllImport("__Internal")]
    private static extern void SavePlayerProgress();
    
    public GameObject playerObject;

    void Start()
    {
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();
        SavePlayerProgress();
    }
}
