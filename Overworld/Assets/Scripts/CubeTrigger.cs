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
    private bool canAccessLevel = false;
    public GameObject playerObject; 

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
        if (canAccessLevel && Input.GetKeyDown(KeyCode.Return)) // Detect Enter key
        {
            PerformAction();
        }
    }

    public void PerformAction()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerMovement playerScript = playerObject.GetComponent<PlayerMovement>();


        enterLevel(playerScript.name.ToString(), playerScript.xx, playerScript.yy, playerScript.zz, levelURL.ToString());

        //Dont do it here, you need to wait for the progress to save
        //TODO: Make seperate function just to save the progress without going into a different page!!!
         //OpenInSamePage(levelURL);
#else
        Application.OpenURL(levelURL);
        #endif
    }



}
