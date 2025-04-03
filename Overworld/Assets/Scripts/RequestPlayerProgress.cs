using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class RequestPlayerProgress : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetDataForUnity();

    void Start()
    {
        GetDataForUnity();
    }

    // This function will receive the JSON data from JavaScript
    public void ReceiveData(string jsonData)
    {
        Debug.Log("Received Data in Unity: " + jsonData);
    }
}
