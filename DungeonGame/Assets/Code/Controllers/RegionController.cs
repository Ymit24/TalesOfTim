using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionController : MonoBehaviour
{
    private static RegionController Instance;
    private static bool hasTransitioned = true;
    private static RegionLocation CurrentLocation;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (hasTransitioned)
        {
            return;
        }

        RegionLinkController[] ctrs = GameObject.FindObjectsOfType<RegionLinkController>();
        foreach (RegionLinkController ctr in ctrs)
        {
            if (ctr.From.Location.Equals(CurrentLocation.Location))
            {
                GameObject.FindObjectOfType<PlayerController>().transform.position = ctr.SpawnLocation.transform.position;
                /*  CAMERA IS NOW A CHILD OF THE PLAYER CONTROLLER  */
                //Camera.main.transform.position = ctr.transform.position;
            }
        }
    }

    public static void ActivateLink(RegionLocation destination)
    {
        hasTransitioned = false;
        CurrentLocation = destination;
        SceneAsset scene = destination.Scene;
        SceneManager.LoadScene(scene.name);
    }
}
