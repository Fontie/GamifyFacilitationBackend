using UnityEngine;
using System.Runtime.InteropServices;

public class CubeTrigger : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenInSamePage(string url);

    [DllImport("__Internal")]
    private static extern void SavePlayerProgress(string playerName, float xx, float yy, float zz);

    [DllImport("__Internal")]
    private static extern void enterLevel(string playerName, float xx, float yy, float zz, string url);




    public bool playerOnCube = false; // Track if the player is standing on the cube
    public string levelURL = "http://unity3d.com/";
    public int accessLevelNeeded = 0;
    public bool accessLevelOnSeperateTab = true;
    private bool canAccessLevel = false;
    private Vector3 originalPosition;
    public GameObject playerObject;

    private void Start()
    {
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();
        originalPosition = transform.position;
        //Debug.Log(playerScript.accessLevel);
    }

    public void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnCube = true;

            PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();
            //Debug.Log(playerScript.accessLevel);

            if (playerScript != null && playerScript.accessLevel >= accessLevelNeeded)
            {
                canAccessLevel = true;

                // Change player color
                Renderer playerRenderer = collision.gameObject.GetComponent<Renderer>();
                if (playerRenderer != null)
                {
                    playerRenderer.material.color = new Color(1, 0, 0, 1);
                }
            }
            else
            {
                canAccessLevel = false;
                Debug.Log("You cant play this yet.");
            }      
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnCube = false;
            canAccessLevel = false;
            //Debug.Log("Player left the cube.");

            // Return player color
            Renderer playerRenderer = collision.gameObject.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                playerRenderer.material.color = new Color(255, 255, 255);
            }

        }
    }

    private void Update()
    {
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();

        if (playerScript != null && playerScript.accessLevel < accessLevelNeeded)
        {
            transform.position = originalPosition + new Vector3(0f, -200f, 0f);
        }
        else
        {
            transform.position = originalPosition;
        }

        if (canAccessLevel && Input.GetKeyDown(KeyCode.Return)) // Detect Enter key
        {
            //enter level
            PerformAction();
        }
    }

    public void PerformAction()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    if (accessLevelOnSeperateTab)
    {
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();
        SavePlayerProgress(playerScript.name.ToString(), playerScript.xx, playerScript.yy, playerScript.zz);
        Application.OpenURL(levelURL);
    }
    else
    {
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();
        enterLevel(playerScript.name.ToString(), playerScript.xx, playerScript.yy, playerScript.zz, levelURL.ToString());
    }

#else
        Application.OpenURL(levelURL);
        #endif
    }



}
