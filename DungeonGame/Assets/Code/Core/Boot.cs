using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    public GameObject UICanvas;
    public SceneAsset Game;
    public GameObject EventSystem;
    private void Awake()
    {
        DontDestroyOnLoad(UICanvas);
        DontDestroyOnLoad(EventSystem);
        SceneManager.LoadScene(Game.name);
    }
}
