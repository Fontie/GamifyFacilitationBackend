using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Unity.VisualScripting;
using System.Runtime.InteropServices;
using UnityEngine.UIElements;
using System.Globalization;
using Unity.Mathematics;
//using Newtonsoft.Json;

public class RequestPlayerProgress : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetDataForUnity();

    public GameObject playerObject;

    void Start()
    {
        GetDataForUnity();
    }

    // This function will receive the JSON data from JavaScript
    public void ReceiveData(string jsonData)
    {
        Debug.Log("Received Data in Unity: " + jsonData);

        // Convert JSON string to UserData object
        UserData data = JsonUtility.FromJson<UserData>(jsonData);

        Debug.Log(data);

        // Split the string into an array of substrings
        string[] values = data.overworldCoords.Split(",");

        Debug.Log(values[1]);

        playerObject.GetComponent<PlayerMovement>().accessLevel = data.accesslevel;


        // Parse the string values into floats
        if (
            float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
            float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y) &&
            float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
        {
            // Teleport away! Weeeeee!!!
            playerObject.transform.position = new Vector3(x, y, z);
            Debug.Log(playerObject.transform.position);
            StartCoroutine(SetPlayerPositionAfterFrame(x,y,z));

        }
        else
        {
            Debug.LogError("Invalid position format from database: " + values);
        }
    }

    public IEnumerator SetPlayerPositionAfterFrame(float x ,float y, float z)
    {
        yield return null; // wait one frame

        playerObject.transform.position = new Vector3(x, y, z);
        Debug.Log(playerObject.transform.position);
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string overworldCoords;
        public int accesslevel;

    }
}
